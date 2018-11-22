using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Tmpps.Infrastructure.Common.Logging.Extensions
{
    /// <summary>
    /// LoggerFactoryExtensions
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        private volatile static bool isInit = false;
        private volatile static object lockObject = new object();
        private static ILogger DefaultLogger;
        /// <summary>
        /// GetDefaultLogger
        /// </summary>
        public static ILogger GetDefaultLogger(this ILoggerFactory loggerFactory)
        {
            if (!isInit)
            {
                lock(lockObject)
                {
                    if (!isInit)
                    {
                        DefaultLogger = loggerFactory.CreateLogger("default");
                        isInit = true;
                    }
                }
            }
            return DefaultLogger;
        }
    }
}