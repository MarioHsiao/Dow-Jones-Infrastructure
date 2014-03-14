using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using DowJones.Json.Gateway.Converters;
using log4net;

namespace DowJones.Json.Gateway.Exceptions
{
    [Serializable]
    public class JsonGatewayException : ApplicationException
    {
        protected const long BaseError = 581000;

        #region Errors

        public const long GenericError = BaseError;
        public const long BadRequest = BaseError + 1;
        public const long GatewayTimeout = BaseError + 2;
        public const long RequestUriTooLong = BaseError + 3;
        public const long MethodNotAllowed = BaseError + 4;
        public const long NotAcceptable = BaseError + 5;
        public const long NotFound = BaseError + 6;
        public const long NotImplemented = BaseError + 7;
        public const long RequestTimeout = BaseError + 8;
        public const long Unauthorized = BaseError + 9;
        public const long Forbidden = BaseError + 10;
        public const long RequestEntityTooLarge = BaseError + 11;
        public const long ServiceUnavailable = BaseError + 12;
        public const long InternalServerError = BaseError + 13;


        public const long ServicePathIsNotDefined = BaseError + 30;

        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof (JsonGatewayException));
        private readonly long _returnCode = -1;

        public JsonGatewayException(long returnCode, string message) : base(message)
        {
            _returnCode = returnCode;
        }

        public JsonGatewayException(long returnCode, string message, Exception ex) : base(message, ex)
        {
            _returnCode = returnCode;
        }

        public virtual ILog Logger
        {
            get { return Log; }
        }

        public virtual long ReturnCode
        {
            get { return _returnCode; }
        }

        protected void LogException()
        {
            if (ReturnCode != -1 && !Logger.IsDebugEnabled) return;
            string stackTrace = StackTrace ?? new StackTrace().ToString();
            Logger.Error(string.Format("\nReturn code: {0} - Message: {1}\nStack Trace: {2}", ReturnCode, Message, stackTrace));
        }

        public static JsonGatewayException Parse(string json)
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

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info,
            StreamingContext context)
        {
            // Change the public const long of two properties, and then use the  
            // method of the base class.
            HelpLink = HelpLink.ToLower();
            Source = Source.ToUpper();

            base.GetObjectData(info, context);
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

        internal static void GetExceptionLog(StringBuilder sb, Exception exception, int depth)
        {
            sb.AppendFormat("\n==========Inner Exception level {0}==========", depth);
            sb.AppendFormat("\nInner Exception Type: {0}", exception.GetType().FullName);
            sb.AppendFormat("\nInner Exception Message: {0}", exception.Message);
            sb.AppendFormat("\nInner Exception Source: {0}", exception.Source);
            sb.AppendFormat("\nInner Exception StackTrace:\n{0}", exception.StackTrace);
        }
    }
}