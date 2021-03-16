using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Interfaces
{
    public interface IRecord
    {
        public enum RecordType
        {
            Descriptive = 0,
            Detail = 1,
            FileTotal = 7,
            Unknown = 99
        }

        public RecordType Type { get; }

        public string LineNumber { get; set; }
    }
}
