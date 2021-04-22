using Canary.Core.Helpers;
using Canary.Core.Model;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Canary.Core.Validators
{
    public class DetailRecordValidator
    {
        private static readonly int FIXED_REC_LENGTH = 119; // excluding Record Type indicator field (1 char)

        private static readonly string WITHHOLDING_TAX_INDICATOR_PATTERN = @"^[ NWXY]+$";  // TODO: Remove hardcoded values, should be stored in config
        private static readonly string TRANSACTION_CODE_PATTERN = @"^(13|50|51|52|53|54|55|56|57)$";   // TODO: Remove hardcoded values, should be stored in config
        private static readonly string VALID_CHARS_PATTERN = CommonValidationRoutines.VALID_CHARS_PATTERN;
        private ILogger _logger;

        public DetailRecordValidator(ILogger logger)
        {
            _logger = logger;
        }

        public List<ValidationMessage> Validate (DetailRecord rec)
        {
            var errors = new List<ValidationMessage>();

            _logger.Info("DetailRecordValidator.Validate: Validating Detail record ...");

            try
            {
                var recLength = string.Concat(rec.Bsb, rec.AccountNumber, rec.WithholdingTaxIndicator, rec.TransactionCode, rec.AmountInCents, rec.AccountTitle, rec.LodgementReference, rec.TraceBsb, rec.TraceAccountNumber, rec.RemitterName, rec.WithholdingTaxAmountInCents).Length;

                // Validations messages may/may not pertain to a specific line in the file
                int? parsedLineNumber = int.TryParse(rec.LineNumber, out var outVal) ? (int?)outVal : null;

                if (recLength != FIXED_REC_LENGTH)
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"Record should be {FIXED_REC_LENGTH + 1} chars in length, however found record length of {recLength} characters" });
                }

                if (!CommonValidationRoutines.IsBsbNumber(rec.Bsb))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'BSB Number' field must be in format '999-999'." });
                }

                if (!CommonValidationRoutines.IsAccountNumber(rec.AccountNumber))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Account Number' field can only contain numeric, hyphens and space characters." });
                }

                if (!string.IsNullOrWhiteSpace(rec.WithholdingTaxIndicator) && !Regex.IsMatch(rec.WithholdingTaxIndicator, WITHHOLDING_TAX_INDICATOR_PATTERN))
                {
                    // TODO: Remove hardcoded values, should be stored in config
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'WithholdingTaxIndicator' contains illegal values. Allowed values are - N, W, X and Y." });
                }

                if (string.IsNullOrWhiteSpace(rec.TransactionCode) || !Regex.IsMatch(rec.TransactionCode, TRANSACTION_CODE_PATTERN))
                {
                    // TODO: Remove hardcoded values, should be stored in config
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Transaction Code' contains illegal values. Allowed values are - 13, 50, 51, 52, 53, 54, 55, 56 or 57." });
                }

                if (!CommonValidationRoutines.IsBsbNumber(rec.TraceBsb))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Trace BSB Number' field must be in format '999-999'." });
                }

                if (!CommonValidationRoutines.IsCentsString(rec.AmountInCents))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Amount' field must contain cents." });
                }

                if (!CommonValidationRoutines.IsAccountNumber(rec.TraceAccountNumber))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Trace Account Number' field can only contain numeric, hyphens and space characters." });
                }

                if (!CommonValidationRoutines.IsCentsString(rec.WithholdingTaxAmountInCents))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"'Amount of Withholding Tax' field must contain cents." });
                }

                // Check for valid characters
                if (!(Regex.IsMatch(rec.Bsb, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.AccountNumber, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.WithholdingTaxIndicator, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.AccountTitle, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.LodgementReference, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.TraceAccountNumber, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.RemitterName, VALID_CHARS_PATTERN)))
                {
                    errors.Add(new ValidationMessage() { Type = ValidationMessage.MessageTypes.Error, LineNumber = parsedLineNumber, Message = $"Invalid characters found. File can only contain alpha numeric characters." });
                }
                //
            }
            catch (Exception e)
            {
                _logger.Error($"DetailRecordValidator.Validate: An unexpected error occurred while validating Detail Record ...");
                _logger.Error(e);
                throw;
            }

            return errors;
        }
    }
}
