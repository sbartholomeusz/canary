using Canary.Core;
using Canary.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Canary.Web.Services
{
    public static class AbaFileValidationService
    {
        public static List<AbaFileValidationMessage> ValidateFile(Stream s)
        {
            var ret = new List<AbaFileValidationMessage>();
            var results = new AbaFileOperations(new ConsoleLogger()).ValidateFile(s);

            foreach (var result in results)
            {
                AbaFileValidationMessage.MessageTypes t;
                switch (result.Type)
                {
                    case Core.Model.ValidationMessage.MessageTypes.Information:
                        t = AbaFileValidationMessage.MessageTypes.Information;
                        break;
                    case Core.Model.ValidationMessage.MessageTypes.Warning:
                        t = AbaFileValidationMessage.MessageTypes.Warning;
                        break;
                    case Core.Model.ValidationMessage.MessageTypes.Error:
                        t = AbaFileValidationMessage.MessageTypes.Error;
                        break;
                    default:
                        t = AbaFileValidationMessage.MessageTypes.Error; // Shouldn't happen
                        break;
                }
                ret.Add(new AbaFileValidationMessage() { Type = (AbaFileValidationMessage.MessageTypes)t, LineNumber = result.LineNumber, Message = result.Message });
            }

            return ret;
        }
    }
    public class AbaFileValidationMessage
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
