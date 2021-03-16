using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Canary.Core.Helpers
{
    public static class RuntimeEnvironment
    {
        public static string GetCurrentExecutionFolderPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
