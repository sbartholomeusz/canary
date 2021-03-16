using Canary.Core.Helpers;
using Canary.Core.Interfaces;
using Canary.Core.Model;
using Canary.Core.Validators;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Canary.Core
{
    public class AbaFileOperations
    {
        // TODO: Should this class be made static?

        private ILogger _logger = null;
        private enum RecordType
        {
            Descriptive = 0,
            Detail = 1,
            FileTotal = 7,
            Unknown = 99
        }

        public AbaFileOperations() : this(new Logger())
        { }

        public AbaFileOperations(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Validate the ABA input file.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<ValidationMessage> ValidateFile(Stream s)
        {
            var recs = new List<IRecord>();
            var messages = new List<ValidationMessage>();

            try
            {
                _logger.Info("ValidateFile: Processing ...");

                _logger.Info("ValidateFile: Parsing records ...");
                recs = ParseRecords(s);

                _logger.Info($"ValidateFile: Found {recs.Where(i => i.Type == IRecord.RecordType.Descriptive).Count()} Descriptive Records");
                _logger.Info($"ValidateFile: Found {recs.Where(i => i.Type == IRecord.RecordType.Detail).Count()} Detail Records");
                _logger.Info($"ValidateFile: Found {recs.Where(i => i.Type == IRecord.RecordType.FileTotal).Count()} File Total Records");

                _logger.Info("ValidateFile: Validating records ...");
                messages.AddRange(ValidateRecords(recs));

            }
            catch (Exception e)
            {
                _logger.Error("ValidateFile: An unexpected error occurred while processing file ...");
                _logger.Error(e);
            }

            return messages;
        }

        private List<IRecord> ParseRecords(Stream s)
        {
            var recs = new List<IRecord>();

            _logger.Info("ParseRecords: Reading stream ...");
            using (StreamReader sr = new StreamReader(s))
            {
                var lineNumber = 0;
                while (!sr.EndOfStream)
                {
                    // Read each line from file
                    lineNumber++;
                    string rawRec = sr.ReadLine();

                    // Validate line
                    var recType = GetRecordType(rawRec);

                    switch (recType)
                    {
                        case RecordType.Descriptive:
                            recs.Add(new DescriptiveRecord(rawRec, lineNumber.ToString()));
                            break;

                        case RecordType.Detail:
                            recs.Add(new DetailRecord(rawRec, lineNumber.ToString()));
                            break;

                        case RecordType.FileTotal:
                            recs.Add(new FileTotalRecord(rawRec, lineNumber.ToString()));
                            break;

                        default:
                            _logger.Error($"ParseRecords: Found unexpected Record Type - '{rawRec}'");
                            recs.Add(new UnknownRecord(rawRec, lineNumber.ToString()));
                            break;
                    }
                }
            }

            return recs;
        }

        private RecordType? GetRecordType(string record)
        {
            RecordType? recType = null;

            switch (record.Substring(0,1))
            {
                case "0":
                    recType = RecordType.Descriptive;
                    break;
                case "1":
                    recType = RecordType.Detail;
                    break;
                case "7":
                    recType = RecordType.FileTotal;
                    break;
                default:
                    recType = RecordType.Unknown;
                    break;
            }

            return recType;
        }

        private List<ValidationMessage> ValidateRecords(List<IRecord> recs)
        {
            var orderedRecs = recs.OrderBy(i => i.LineNumber);
            var descriptiveRecs = recs.Where(i => i.Type == IRecord.RecordType.Descriptive).ToList().Cast<DescriptiveRecord>();
            var detailRecs = recs.Where(i => i.Type == IRecord.RecordType.Detail).ToList().Cast<DetailRecord>();
            var fileTotalRecs = recs.Where(i => i.Type == IRecord.RecordType.FileTotal).ToList().Cast<FileTotalRecord>();
            var errors = new List<ValidationMessage>();

            try
            {
                //
                // Perform record level checks
                //
                _logger.Info($"ValidateRecords: Performing record level checks ({recs.Count} recs) ...");

                // Check descriptive records
                _logger.Info($"ValidateRecords: Validating Descriptive records ...");
                foreach (var descriptiveRec in descriptiveRecs)
                {
                    errors.AddRange(DescriptiveRecordValidator.Validate((DescriptiveRecord)descriptiveRec));
                }

                // Check detail records
                _logger.Info($"ValidateRecords: Validating Detail records ...");
                foreach (var detailRec in detailRecs)
                {
                    errors.AddRange(DetailRecordValidator.Validate((DetailRecord)detailRec));
                }

                // Check file total records
                _logger.Info($"ValidateRecords: Validating File Total records ...");
                int i;
                var creditTotalAmount = detailRecs.Where(x => x.TransactionCodeParsed != TransactionCodes.ExternallyInitiatedDebit &&
                                                         int.TryParse(x.AmountInCents, out i)).Sum(x => int.Parse(x.AmountInCents));
                var debitTotalAmount = detailRecs.Where(x => x.TransactionCodeParsed == TransactionCodes.ExternallyInitiatedDebit &&
                                                        int.TryParse(x.AmountInCents, out i)).Sum(x => int.Parse(x.AmountInCents));
                var netTotalAmount = creditTotalAmount - debitTotalAmount;
                var detailRecordCount = detailRecs.Count();

                _logger.Info($"ValidateRecords: creditTotalAmount={creditTotalAmount}");
                _logger.Info($"ValidateRecords: debitTotalAmount={debitTotalAmount}");
                _logger.Info($"ValidateRecords: netTotalAmount={netTotalAmount}");
                _logger.Info($"ValidateRecords: detailRecordCount={detailRecordCount}");

                foreach (var fileTotalRec in fileTotalRecs)
                {
                    errors.AddRange(FileTotalRecordValidator.Validate((FileTotalRecord)fileTotalRec, netTotalAmount, creditTotalAmount, debitTotalAmount, detailRecordCount));
                }


                //
                // Perform general checks across entire file
                //
                _logger.Info($"ValidateRecords: Performing file level checks ...");
                if (orderedRecs.Count() < 3)
                {
                    errors.Add(new ValidationMessage() { Message = $"File must contain at least 3 records. Found {orderedRecs.Count()} records." });
                }
                else
                {
                    if (descriptiveRecs.Count() != 1)
                    {
                        errors.Add(new ValidationMessage() { Message = $"File must only contain 1 descriptive record. Found {descriptiveRecs.Count()} records." });
                    }

                    if (detailRecs.Count() < 1)
                    {
                        errors.Add(new ValidationMessage() { Message = $"File must contain at least 1 detail record. Found {detailRecs.Count()} records." });
                    }

                    if (fileTotalRecs.Count() != 1)
                    {
                        errors.Add(new ValidationMessage() { Message = $"File must only contain 1 file total record. Found {fileTotalRecs.Count()} records." });
                    }

                    if (orderedRecs.First().Type != IRecord.RecordType.Descriptive)
                    {
                        errors.Add(new ValidationMessage() { Message = "First record must be a descriptive record" });
                    }

                    if (orderedRecs.Last().Type != IRecord.RecordType.FileTotal)
                    {
                        errors.Add(new ValidationMessage() { Message = "Last record must be a file total record" });
                    }
                }
                //

                _logger.Info($"ValidateRecords: Found {errors.Count} issues.");
            }
            catch (Exception e)
            {
                errors.Add(new ValidationMessage() { Message="Unable to validate file. An unexpected error occured while validating the file."});
                _logger.Error($"ValidateRecords: Unable to validate file. An unexpected error occured while validating the file.");
                _logger.Error(e);
                throw;
            }

            return errors;
        }
    }
}
