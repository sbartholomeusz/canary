using log4net;
using log4net.Config;
using log4net.Core;
using System;
using System.IO;
using System.Reflection;

namespace Canary.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Log4NetLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// Log informational message
        /// </summary>
        /// <param name="str"></param>
        public void Info(string str)
        {
            log.Info(str);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(string e)
        {
            log.Error(e);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="str"></param>
        public void Warning(string str)
        {
            log.Warn(str);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="e"></param>
        public void Error(Exception e)
        {
            log.Error(e);
        }
    }
}
