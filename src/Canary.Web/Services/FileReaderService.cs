using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Canary.Web.Services
{
    public class FileReaderService
    {
        public List<FileRecord> GetFileContents(Stream file)
        {
            // TODO: Should consolidate file read logic with WPF form
            var fileContents = new List<FileRecord>();

            if (file == null)
            {
                return fileContents;
            }

            using (StreamReader sr = new StreamReader(file))
            {
                var lineNumber = 0;
                while (!sr.EndOfStream)
                {
                    // Read each line from file
                    lineNumber++;

                    fileContents.Add(new FileRecord() { LineNumber = lineNumber, Contents = sr.ReadLine() });
                }
            }

            return fileContents;
        }
    }

    public class FileRecord
    {
        public int LineNumber { get; set; }
        public string Contents { get; set; }
    }
}
