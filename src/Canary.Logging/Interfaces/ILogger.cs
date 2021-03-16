using System;
using System.Collections.Generic;
using System.Text;

namespace Canary.Logging
{
    public interface ILogger
    {

        /// <summary>
        /// Log informational message
        /// </summary>
        /// <param name="str"></param>
        public void Info(string str);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(string e);

        /// <summary>
        /// Log warning message 
        /// </summary>
        /// <param name="str"></param>
        public void Warning(string str);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(Exception e);
    }
}
