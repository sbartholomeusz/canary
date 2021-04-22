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
        private readonly ILogger _logger = null;
        private enum RecordType
        {
            Descriptive = 0,
            Detail = 1,
            FileTotal = 7,
            Unknown = 99
        }

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
                recs = ParseRecords(s); // Don't close stream, let the calling client do it

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

            // Let the calling client close the stream
            StreamReader sr = new StreamReader(s);
            
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

            return recs;
        }

        private RecordType? GetRecordType(string record)
        {
            RecordType? recType = null;

            recType = record.Substring(0, 1) switch
            {
                "0" => RecordType.Descriptive,
                "1" => RecordType.Detail,
                "7" => RecordType.FileTotal,
                _ => RecordType.Unknown,
            };
            return recType;
        }

        private List<ValidationMessage> ValidateRecords(List<IRecord> recs)
        {
            var orderedRecs = recs.OrderBy(i => i.LineNumber);
            var descriptiveRecs = recs.Where(i => i.Type == IRecord.RecordType.Descriptive).ToList().Cast<DescriptiveRecord>();
            var detailRecs = recs.Where(i => i.Type == IRecord.RecordType.Detail).ToList().Cast<DetailRecord>();
            var fileTotalRecs = recs.Where(i => i.Type == IRecord.RecordType.FileTotal).ToList().Cast<FileTotalRecord>();
            var outputMessages = new List<ValidationMessage>();

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
                    outputMessages.AddRange(new DescriptiveRecordValidator(_logger).Validate((DescriptiveRecord)descriptiveRec));
                }

                // Check detail records
                _logger.Info($"ValidateRecords: Validating Detail records ...");
                foreach (var detailRec in detailRecs)
                {
                    outputMessages.AddRange(new DetailRecordValidator(_logger).Validate((DetailRecord)detailRec));
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
                    outputMessages.AddRange(new FileTotalRecordValidator(_logger).Validate((FileTotalRecord)fileTotalRec, netTotalAmount, creditTotalAmount, debitTotalAmount, detailRecordCount));
                }


                //
                // Perform general checks across entire file
                //
                _logger.Info($"ValidateRecords: Performing file level checks ...");
                if (orderedRecs.Count() < 3)
                {
                    outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = $"File must contain at least 3 records. Found {orderedRecs.Count()} records." });
                }
                else
                {
                    if (descriptiveRecs.Count() != 1)
                    {
                        outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = $"File must only contain 1 descriptive record. Found {descriptiveRecs.Count()} records." });
                    }

                    if (detailRecs.Count() < 1)
                    {
                        outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = $"File must contain at least 1 detail record. Found {detailRecs.Count()} records." });
                    }

                    if (fileTotalRecs.Count() != 1)
                    {
                        outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = $"File must only contain 1 file total record. Found {fileTotalRecs.Count()} records." });
                    }

                    if (orderedRecs.First().Type != IRecord.RecordType.Descriptive)
                    {
                        outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = "First record must be a descriptive record" });
                    }

                    if (orderedRecs.Last().Type != IRecord.RecordType.FileTotal)
                    {
                        outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message = "Last record must be a file total record" });
                    }
                }
                //

                var infoMsgCount = outputMessages.Where(x => x.Type == ValidationMessage.MessageTypes.Information).Count();
                var warningMsgCount = outputMessages.Where(x => x.Type == ValidationMessage.MessageTypes.Warning).Count();
                var errorMsgCount = outputMessages.Where(x => x.Type == ValidationMessage.MessageTypes.Error).Count();

                if (errorMsgCount < 1 & warningMsgCount < 1)
                {
                    outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Information, Message = "No issues found" });
                }

                _logger.Info("**********************************************************");
                _logger.Info($"ValidateRecords: {infoMsgCount} errors.");
                _logger.Info($"ValidateRecords: {warningMsgCount} warnings.");
                _logger.Info($"ValidateRecords: {errorMsgCount} informational messages.");
                _logger.Info("**********************************************************");
            }
            catch (Exception e)
            {
                outputMessages.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, Message ="Unable to validate file. An unexpected error occurred while validating the file."});
                _logger.Error($"ValidateRecords: Unable to validate file. An unexpected error occurred while validating the file.");
                _logger.Error(e);
                throw;
            }

            return outputMessages;
        }
    }
}
