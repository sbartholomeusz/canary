using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Canary.Core.Helpers
{
    public static class CommonValidationRoutines
    {

        public static string VALID_CHARS_PATTERN = @"^[\s\da-zA-Z &`',\-./+$!%()*#=:?[_\]^@]*$";

        public static bool IsBsbNumber(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return Regex.IsMatch(str, @"^[0-9][0-9][0-9]-[0-9][0-9][0-9]$");
        }

        public static bool IsBlankFilled(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsLeftJustified(string str, string fillChar = " ")
        {
            if (string.IsNullOrWhiteSpace(str.Replace(fillChar, "")))
                return true;

            return !str.StartsWith(fillChar);
        }
        
        public static bool IsRightJustified(string str, string fillChar=" ")
        {
            if (string.IsNullOrWhiteSpace(str.Replace(fillChar, "")))
                return true;

            return !str.EndsWith(fillChar);
        }

        public static bool IsValidDateString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            DateTime d;
            if (!DateTime.TryParseExact(str, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            {
                return false;
            }

            return true;
        }

        public static bool IsCentsString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        public static bool IsAccountNumber(string str)
        {
            return Regex.IsMatch(str, @"^([0-9]|-| )*$");
        }

        public static bool IsZeroFilledNumeric(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }
    }
}
