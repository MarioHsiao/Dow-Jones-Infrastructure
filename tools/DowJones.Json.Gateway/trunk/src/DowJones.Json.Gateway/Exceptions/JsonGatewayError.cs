using System;
using System.Diagnostics;
using System.Text;
using DowJones.Json.Gateway.Converters;
using log4net;

namespace DowJones.Json.Gateway.Exceptions
{
    /*
     * {"Error":{"Code":589500,"Message":"Generic ServiceProxy Error Cannot find module 'sax'"}}
    */

    public class JsonGatewayError
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public long Code { get; set; }

        public string Message { get; set; }
    }


    public class JsonGatewayException : ApplicationException
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JsonGatewayException));
        private readonly long _returnCode = -1;
        protected const long BaseError = 581000;
        public const long GenericError = BaseError;


        public virtual ILog Logger
        {
            get { return Log; }
        }

        public virtual long ReturnCode
        {
            get { return _returnCode; }
        }

        public JsonGatewayException(long returnCode, string message) : base(message)
        {
            _returnCode = returnCode;
        }
        public JsonGatewayException(long returnCode, string message, Exception ex) : base(message, ex)
        {
            _returnCode = returnCode;
        }
        
        protected void LogException()
        {
            if (ReturnCode != -1 && !Logger.IsDebugEnabled) return;
            var stackTrace = StackTrace ?? new StackTrace().ToString();
            Logger.Error(string.Format("\nReturn code: {0} - Message: {1}\nStack Trace: {2}", ReturnCode, Message, stackTrace));
        }

        public static JsonGatewayException ParseExceptionMessage(string json)
        {
            try
            {
                var jsonGatewayError = JsonDataConverterDecoratorSingleton.Instance.Deserialize<JsonGatewayError>(json);
                return new JsonGatewayException(jsonGatewayError.Error.Code, jsonGatewayError.Error.Message);
            }
            catch (Exception)
            {
                return new JsonGatewayException(GenericError, json);
            }
        }

        internal static void GetInnerExceptionLog(StringBuilder sb, Exception innerException, int depth = 0)
        {
            while (true)
            {
                if (sb == null)
                {
                    sb = new StringBuilder();
                }

                if (innerException == null)
                {
                    return;
                }

                GetExceptionLog(sb, innerException, depth);
                innerException = innerException.InnerException;
                depth = depth + 1;
            }
        }

        static internal void GetExceptionLog(StringBuilder sb, Exception exception, int depth)
        {
            sb.AppendFormat("\n==========Inner Exception level {0}==========", depth);
            sb.AppendFormat("\nInner Exception Type: {0}", exception.GetType().FullName);
            sb.AppendFormat("\nInner Exception Message: {0}", exception.Message);
            sb.AppendFormat("\nInner Exception Source: {0}", exception.Source);
            sb.AppendFormat("\nInner Exception StackTrace:\n{0}", exception.StackTrace);
        }

    }


}
