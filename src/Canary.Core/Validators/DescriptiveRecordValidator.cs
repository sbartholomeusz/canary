using Canary.Core.Helpers;
using Canary.Core.Model;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Canary.Core.Validators
{
    public static class DescriptiveRecordValidator
    {
        static string VALID_CHARS_PATTERN = CommonValidationRoutines.VALID_CHARS_PATTERN;
        static int FIXED_REC_LENGTH = 119; // excluding Record Type indicator field (1 char)
        static ILogger _logger;

        static DescriptiveRecordValidator()
        {
            _logger = new Logger();
        }

        public static List<ValidationMessage> Validate(DescriptiveRecord rec)
        {
            var errors = new List<ValidationMessage>();

            _logger.Info("DescriptiveRecordValidator.Validate: Validating Descriptive record ...");

            try
            {
                var recLength = string.Concat(rec.Blank1, rec.ReelSequenceNumber, rec.UserFinancialInstitutionName, rec.Blank2, rec.UserPreferredSpecification, rec.UserIdentificationNumber, rec.Description, rec.DateToBeProcessed, rec.Blank3).Length;

                // Validations messages may/may not pertain to a specific line in the file
                int? parsedLineNumber = int.TryParse(rec.LineNumber, out var outVal) ? (int?)outVal : null;

                if (recLength != FIXED_REC_LENGTH)
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"Record should be {FIXED_REC_LENGTH+1} chars in length, however found record length of {recLength} characters" });
                }

                if (!string.IsNullOrWhiteSpace(rec.Blank1))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Blank1' must be space filled." });
                }

                if (!CommonValidationRoutines.IsZeroFilledNumeric(rec.ReelSequenceNumber))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Reel Sequence Number' must be numeric zero filled e.g. '01'." });
                }

                if (!string.IsNullOrWhiteSpace(rec.Blank2))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Blank2' must be space filled." });
                }

                if (string.IsNullOrWhiteSpace(rec.UserPreferredSpecification))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Name of User Supplying File' field must be populated." });
                }

                if (!CommonValidationRoutines.IsLeftJustified(rec.UserPreferredSpecification, " "))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'User Preferred Specification' must be left justified and blank filled." });
                }

                if (!CommonValidationRoutines.IsZeroFilledNumeric(rec.UserIdentificationNumber))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'User Identification Number' must be numeric and zero filled." });
                }

                if (string.IsNullOrWhiteSpace(rec.Description))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Description' field must be populated." });
                }

                if (!CommonValidationRoutines.IsLeftJustified(rec.Description, " "))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Description' must be left justified, blank filled." });
                }

                if (!CommonValidationRoutines.IsValidDateString(rec.DateToBeProcessed))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Date to be processed' must be in DDMMYY format." });
                }

                if (!string.IsNullOrWhiteSpace(rec.Blank3))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"'Blank3' must be space filled." });
                }

                // Check for valid characters
                if (!(Regex.IsMatch(rec.Blank1, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.ReelSequenceNumber, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.UserFinancialInstitutionName, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.Blank2, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.UserPreferredSpecification, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.Description, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.DateToBeProcessed, VALID_CHARS_PATTERN) &&
                      Regex.IsMatch(rec.Blank3, VALID_CHARS_PATTERN)))
                {
                    errors.Add(new ValidationMessage() { LineNumber = parsedLineNumber, Message = $"Invalid characters found. File can only contain alpha numeric characters." });
                }
                //

                _logger.Info($"DescriptiveRecordValidator.Validate: Found {errors.Count} issues while validating Descriptive record at line {rec.LineNumber}");
            }
            catch (Exception e)
            {
                _logger.Error($"DescriptiveRecordValidator.Validate: An unexpected error occurred while validating Descriptive Record ...");
                _logger.Error(e);
                throw;
            }

            return errors;
        }
    }
}

