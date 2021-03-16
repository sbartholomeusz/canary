using Canary.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    /// <summary>
    /// ABA Bank File Descriptive Record Type i.e. Type=0
    /// </summary>
    public class DescriptiveRecord : IRecord
    {
        public IRecord.RecordType Type { get; }
        public string Blank1 { get; set; }
        public string ReelSequenceNumber { get; set; }
        public string UserFinancialInstitutionName { get; set; }
        public string Blank2 { get; set; }
        public string UserPreferredSpecification { get; set; }
        public string UserIdentificationNumber { get; set; }
        public string Description { get; set; }
        public string DateToBeProcessed { get; set; }
        public string Blank3 { get; set; }
        public string LineNumber { get; set; }

        /// <summary>
        /// Initialise object with file string
        /// </summary>
        /// <param name="data"></param>
        public DescriptiveRecord(string data, string lineNumber="N/A")
        {
            var typeStr = GetValueAtPosition(data, 0, 1);
            if (!String.IsNullOrWhiteSpace(typeStr) && typeStr != ((int)IRecord.RecordType.Descriptive).ToString()) return;
            Type = IRecord.RecordType.Descriptive;

            Blank1 = GetValueAtPosition(data, 1, 17);
            ReelSequenceNumber = GetValueAtPosition(data, 18, 2);
            UserFinancialInstitutionName = GetValueAtPosition(data, 20, 3);
            Blank2 = GetValueAtPosition(data, 23, 7);
            UserPreferredSpecification = GetValueAtPosition(data, 30, 26);
            UserIdentificationNumber = GetValueAtPosition(data, 56, 6);
            Description = GetValueAtPosition(data, 62, 12);
            DateToBeProcessed = GetValueAtPosition(data, 74, 6);
            Blank3 = GetValueAtPosition(data, 80, 40);

            LineNumber = lineNumber;
        }

        private string GetValueAtPosition(string str, int startPosition, int length)
        {
            string ret = String.Empty;

            try
            {
                ret = str.Substring(startPosition, length);
            }

            catch (Exception)
            {
                // Do nothing
            }

            return ret;
        }
    }
}
