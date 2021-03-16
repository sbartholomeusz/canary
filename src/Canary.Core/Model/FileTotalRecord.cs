using Canary.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    /// <summary>
    /// ABA Bank File Total Record Type i.e. Type=7
    /// </summary>
    public class FileTotalRecord : IRecord
    {
        public IRecord.RecordType Type { get; }
        public string Bsb { get; set; }
        public string Blank1 { get; set; }
        public string NetTotalInCents { get; set; }
        public string CreditTotalInCents { get; set; }
        public string DebitTotalInCents { get; set; }
        public string Blank2 { get; set; }
        public string DetailRecordCount { get; set; }
        public string Blank3 { get; set; }
        public string LineNumber { get; set; }


        /// <summary>
        /// Initialise object with file string
        /// </summary>
        /// <param name="data"></param>
        public FileTotalRecord(string data, string lineNumber="N/A")
        {
            var typeStr = GetValueAtPosition(data, 0, 1);
            if (!String.IsNullOrWhiteSpace(typeStr) && typeStr != ((int)IRecord.RecordType.FileTotal).ToString()) return;
            Type = IRecord.RecordType.FileTotal;

            Bsb = GetValueAtPosition(data, 1, 7);
            Blank1 = GetValueAtPosition(data, 8, 12);
            NetTotalInCents = GetValueAtPosition(data, 20, 10);
            CreditTotalInCents = GetValueAtPosition(data, 30, 10);
            DebitTotalInCents = GetValueAtPosition(data, 40, 10);
            Blank2 = GetValueAtPosition(data, 50, 24);
            DetailRecordCount = GetValueAtPosition(data, 74, 6);
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
