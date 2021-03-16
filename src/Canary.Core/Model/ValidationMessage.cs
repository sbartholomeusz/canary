using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Core.Model
{
    public class ValidationMessage
    {
        public int? LineNumber { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"Line {LineNumber}: {Message}";
        }
    }
}
