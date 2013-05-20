using log4net;

namespace FactivaRssManager
{
    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class Logger
    {
        #region Level enum

        public enum Level
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR,
            FATALERROR
        } ;

        #endregion

        private static ILog logger = LogManager.GetLogger(typeof (Logger));

        public static void Log(Level level, int eventNo, string message, params object[] args)
        {
            string logMessage = string.Format("Error({0}):", eventNo) + message;
            Log(level, logMessage, args);
        }

        public static void Log(Level level, string message, params object[] args)
        {
            string logMessage = message;
            if (logger.IsFatalEnabled && level >= Level.FATALERROR)
            {
                logger.Fatal(logMessage);
            }

            else if (logger.IsErrorEnabled && level >= Level.ERROR)
            {
                logger.Error(logMessage);
            }

            else if (logger.IsWarnEnabled && level >= Level.WARNING)
            {
                logger.Warn(logMessage);
            }

            else if (logger.IsInfoEnabled && level >= Level.INFO)
            {
                logger.Info(logMessage);
            }

            else if (logger.IsDebugEnabled && level >= Level.DEBUG)
            {
                logger.Debug(logMessage);
            }
        }

        //public static void logToEventViewer(string msg, string source)
        //{

        //    string _logToEventViwer = System.Configuration.ConfigurationManager.AppSettings.Get("LOG_TO_EVENT_VIEWER");

        //    if (_logToEventViwer.Trim() == "1")
        //    {
        //        EventLog el = new EventLog();

        //        el.Log = "Application";
        //        if (source == "")
        //            el.Source = "Asset Service";
        //        else
        //            el.Source = source;
        //        el.WriteEntry(msg);
        //        el.Close();
        //    }
        //}

        //public void PEM(string message, string source, int ecat, int elevel)
        //{
        //    try
        //    {
        //        log4net.Config.XmlConfigurator.Configure();
        //        log4net.GlobalContext.Properties["HOST"] = System.Net.Dns.GetHostName();
        //        log4net.GlobalContext.Properties["MODNAME"] = System.Configuration.ConfigurationManager.AppSettings.Get("PEM_MODNAME");
        //        log4net.ThreadContext.Properties["ECAT"] = ecat;
        //        log4net.ThreadContext.Properties["ELEVEL"] = elevel;
        //        log4net.ThreadContext.Properties["SRC"] = source;
        //        logger.Fatal(message);

        //    }
        //    catch (Exception e)
        //    {
        //        logToEventViewer(e.Message, source);
        //        throw;
        //    }
        //} //PEM
    }
}