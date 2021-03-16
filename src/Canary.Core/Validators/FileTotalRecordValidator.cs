using Canary.Core.Helpers;
using Canary.Core.Model;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Canary.Core.Validators
{
    public static class FileTotalRecordValidator
    {
        static int FIXED_REC_LENGTH = 119; // excluding Record Type indicator field (1 char)
        static string VALID_CHARS_PATTERN = CommonValidationRoutines.VALID_CHARS_PATTERN;
        static ILogger _logger;

        static FileTotalRecordValidator()
        {
            _logger = new Logger();
        }

        public static List<ValidationMessage> Validate(FileTotalRecord totalRec, int actualNetTotalAmount, int actualCreditTotalAmount, int actualDebitTotalAmount, int actualDetailRecordCount)
        {
            var errors = new List<ValidationMessage>();

            _logger.Info("FileTotalRecordValidator.Validate: Validating descriptive record ...");

            try
            {
                var recLength = string.Concat(totalRec.Bsb, totalRec.Blank1, totalRec.NetTotalInCents, totalRec.CreditTotalInCents, totalRec.DebitTotalInCents, totalRec.Blank2, totalRec.DetailRecordCount, totalRec.Blank3).Length;

                // Validations messages may/may not pertain to a specific line in the file
                int? parsedLineNumber = int.TryParse(totalRec.LineNumber, out var outVal) ? (int?)outVal : null;

                if (recLength != FIXED_REC_LENGTH)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"Record should be {FIXED_REC_LENGTH + 1} chars in length, however found record length of {recLength} characters" });
                }

                if (!CommonValidationRoutines.IsBsbNumber(totalRec.Bsb))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'BSB Number' field must be in format '999-999'." });
                }

                if (!CommonValidationRoutines.IsCentsString(totalRec.CreditTotalInCents))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Credit Total Amount' field must contain cents." });
                }

                if (!CommonValidationRoutines.IsCentsString(totalRec.DebitTotalInCents))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Debit Total Amount' field must contain cents." });
                }

                if (!CommonValidationRoutines.IsRightJustified(totalRec.NetTotalInCents, "0") || !CommonValidationRoutines.IsZeroFilledNumeric(totalRec.NetTotalInCents))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Net Total Amount' field must be numeric, right justified and zero-filled." });
                }
                else if (int.TryParse(totalRec.NetTotalInCents, out var indicatedNetTotalAmount) && indicatedNetTotalAmount != actualNetTotalAmount)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Net Total Amount' field must match the credit total minus the debit total." });
                }

                if (!CommonValidationRoutines.IsRightJustified(totalRec.CreditTotalInCents, "0") || !CommonValidationRoutines.IsZeroFilledNumeric(totalRec.CreditTotalInCents))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Credit Total Amount' field must be numeric, right justified and zero-filled." });
                }
                else if (int.TryParse(totalRec.CreditTotalInCents, out var indicatedCreditTotalAmount) && indicatedCreditTotalAmount != actualCreditTotalAmount)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Credit Total Amount' field must match the total value of Type 1 credit records." });
                }

                if (!CommonValidationRoutines.IsRightJustified(totalRec.DebitTotalInCents, "0") || !CommonValidationRoutines.IsZeroFilledNumeric(totalRec.DebitTotalInCents))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Debit Total Amount' field must be numeric, right justified and zero-filled." });
                }
                else if (int.TryParse(totalRec.DebitTotalInCents, out var indicatedDebitTotalAmount) && indicatedDebitTotalAmount != actualDebitTotalAmount)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Debit Total Amount' field must match the total value of Type 1 debit records." });
                }

                if (!CommonValidationRoutines.IsRightJustified(totalRec.DetailRecordCount, "0") || !CommonValidationRoutines.IsZeroFilledNumeric(totalRec.DetailRecordCount))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Detail Record Count' field must be numeric, right justified and zero-filled." });
                }
                else if (int.TryParse(totalRec.DetailRecordCount, out var indicatedDetailRecCount) && indicatedDetailRecCount != actualDetailRecordCount)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Detail Record Count' must match the total count of Type 1 records." });
                }

                // Check for valid characters
                if (!(Regex.IsMatch(totalRec.Bsb, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(totalRec.Blank1, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(totalRec.Blank2, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(totalRec.Blank3, VALID_CHARS_PATTERN)))
                {
                    errors.Add(new ValidationMessage() { LineNumber = int.Parse(totalRec.LineNumber), Message = $"Invalid characters found. File can only contain alpha numeric characters." });
                }
                //

                _logger.Info($"FileTotalRecordValidator.Validate: Found {errors.Count} issues while validating File Total record");
            }
            catch (Exception e)
            {
                _logger.Error($"FileTotalRecordValidator.Validate: An unexpected error occurred while validating File Total record ...");
                _logger.Error(e);
                throw;
            }

            return errors;
        }
    }
}
