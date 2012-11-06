using System;
using DowJones.API.Common.ExceptionHandling;
using log4net;
using System.Configuration;

namespace DowJones.API.Common.Logging
{
    public class ApiLog
    {
        public struct LogPrefix
        {
            public const string End = "End: ";
            public const string Error = "Error: {0}";
            public const string Exception = "Exception: {0}";
            public const string Start = "Start: ";
            public const string Timer = "Timer: Elapsed Time in seconds {0}";
            public const string Value = "Value: {0}";
        }

        private static ILog _logger;
        private static ApiLog _instance;
        private static bool _stackTraceEnabled;

        private ApiLog()
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger("APILogging");
            _logger.Info(""); // blank line
            _logger.Info("Started Log4Net...");

            try
            {
                var stackTraceEnabled = ConfigurationManager.AppSettings["StackTraceEnabled"];
                _stackTraceEnabled = bool.Parse(stackTraceEnabled);
            }
            catch
            {
                _stackTraceEnabled = false;
            }

        }

        [Obsolete("Deprecated, Use APILog.Logger.Error", true)]
        public static void LogError(Exception ex)
        {

            try
            {
                if (ex != null)
                {
                    var message = string.Format("{0}:{1}:{2}:{3}", ex.TargetSite.DeclaringType.Name, ex.TargetSite.Name, ((ServiceException)ex).Code, ex.Message);
                    Logger.ErrorFormat(LogPrefix.Error, message);
                }
            }
            catch (Exception)
            {
                if (ex != null) Logger.ErrorFormat(LogPrefix.Error, ex.Message);
            }
            if (_stackTraceEnabled)
            {
                if (ex != null) Logger.ErrorFormat(LogPrefix.Error, "StackTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Format exceptionfor loggin
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatMemberInfoMessage(Exception ex)
        {
            string msg;

            try
            {
                msg = string.Format("{0}:{1}:{2}:{3}", ((ex.TargetSite)).DeclaringType.Name, ((ex.TargetSite)).Name, ((ServiceException)ex).Code, ex.Message);
                // Log request URL in case of an exception
                // msg += " Request: " + WebUtility.GetRequestURL();
                if (_stackTraceEnabled)
                {
                    msg += "\r\nStackTrace: " + ex.StackTrace;
                }
            }
            catch (Exception)
            {
                msg = ex.Message;
            }

            return msg;
        }

        public static ILog Logger
        {
            get
            {
                if (null == _instance)
                    _instance = new ApiLog();
                return _logger;
            }
        }
    }
}
