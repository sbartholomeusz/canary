using System;

namespace Canary.Logging
{
    public class ConsoleLogger : ILogger
    {

        /// <summary>
        /// Log informational message
        /// </summary>
        /// <param name="str"></param>
        public void Info(string str)
        {
            Console.WriteLine($"INFO: {str}");
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="str"></param>
        public void Warning(string str)
        {
            Console.WriteLine($"WARNING: {str}");
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(string e)
        {
            Console.WriteLine($"ERROR: {e}");
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(Exception e)
        {
            Console.WriteLine($"ERROR: {e}");
        }
    }
}
