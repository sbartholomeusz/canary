using Canary.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    public enum TransactionCodes
    {
        ExternallyInitiatedDebit = 13,
        ExternallyInitiatedCredit = 50,
        GovernmentSecurityInterest = 51,
        FamilyAllowance = 52,
        Pay = 53,
        Pension = 54,
        Allotment = 55,
        Dividend = 56,
        NoteInterest = 57
    }

    /// <summary>
    /// ABA Bank File Detail Record Type i.e. Type=1
    /// </summary>
    public class DetailRecord : IRecord
    {
        public IRecord.RecordType Type { get; }
        public string Bsb { get; set; }
        public string AccountNumber { get; set; }
        public string WithholdingTaxIndicator { get; set; }
        public string TransactionCode { get; set; }
        public TransactionCodes TransactionCodeParsed { get; set; }
        public string AmountInCents { get; set; }
        public string AccountTitle { get; set; }
        public string LodgementReference { get; set; }
        public string TraceBsb { get; set; }
        public string TraceAccountNumber { get; set; }
        public string RemitterName { get; set; }
        public string WithholdingTaxAmountInCents { get; set; }
        public string LineNumber { get; set; }

        /// <summary>
        /// Initialise object with file string
        /// </summary>
        /// <param name="data"></param>
        public DetailRecord(string data, string lineNumber="N/A")
        {
            var typeStr = GetValueAtPosition(data, 0, 1);
            if (!String.IsNullOrWhiteSpace(typeStr) && typeStr != ((int)IRecord.RecordType.Detail).ToString()) return;
            
            Type = IRecord.RecordType.Detail;
            Bsb = GetValueAtPosition(data, 1, 7);
            AccountNumber = GetValueAtPosition(data, 8, 9);
            WithholdingTaxIndicator = GetValueAtPosition(data, 17, 1);
            TransactionCode = GetValueAtPosition(data, 18, 2);
            AmountInCents = GetValueAtPosition(data, 20, 10);
            AccountTitle = GetValueAtPosition(data, 30, 32);
            LodgementReference = GetValueAtPosition(data, 62, 18);
            TraceBsb = GetValueAtPosition(data, 80, 7);
            TraceAccountNumber = GetValueAtPosition(data, 87, 9);
            RemitterName = GetValueAtPosition(data, 96, 16);
            WithholdingTaxAmountInCents = GetValueAtPosition(data, 112, 8);
            LineNumber = lineNumber;

            if (Enum.TryParse(TransactionCode, out TransactionCodes t))
                TransactionCodeParsed = t;
        }

        /// <summary>
        /// Get substring value at specified range. Ignore any errors.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startPosition"></param>
        /// <param name="length"></param>
        /// <returns></returns>
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
