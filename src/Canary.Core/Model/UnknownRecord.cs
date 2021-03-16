using Canary.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    public class UnknownRecord :IRecord
    {
        public IRecord.RecordType Type { get; }
        public string LineNumber { get; set; }

        public UnknownRecord(string data, string lineNumber="N/A")
        {
            Type = IRecord.RecordType.Unknown;
            LineNumber = lineNumber;
        }
    }
}
