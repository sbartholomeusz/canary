using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    public class ValidationMessage
    {
        public enum MessageTypes
        {
            Information = 0,
            Warning = 1,
            Error = 2
        }

        public MessageTypes Type { get; set; }
        public int? LineNumber { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Line {LineNumber}: {Message}";
        }
    }
}
