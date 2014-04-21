using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
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

        public const long ControlDataIsNotValid = BaseError + 1;
        public const long ServicePathIsNotDefined = BaseError + 2;
        public const long UnableToParseError = BaseError + 3;

        public const long HttpBaseError = BaseError + 30;
        public const long BadRequest = HttpBaseError + 1;
        public const long GatewayTimeout = HttpBaseError + 2;
        public const long RequestUriTooLong = HttpBaseError + 3;
        public const long MethodNotAllowed = HttpBaseError + 4;
        public const long NotAcceptable = HttpBaseError + 5;
        public const long NotFound = HttpBaseError + 6;
        public const long NotImplemented = HttpBaseError + 7;
        public const long RequestTimeout = HttpBaseError + 8;
        public const long Unauthorized = HttpBaseError + 9;
        public const long Forbidden = HttpBaseError + 10;
        public const long RequestEntityTooLarge = HttpBaseError + 11;
        public const long ServiceUnavailable = HttpBaseError + 12;
        public const long InternalServerError = HttpBaseError + 13;


        public const long ControlDataSerializationError = HttpBaseError + 20;
        public const long UnableToParseJsonBodyForError = HttpBaseError + 21;


        #endregion

        private static readonly ILog Log = LogManager.GetLogger(typeof (JsonGatewayException));
        private readonly long _returnCode = -1;

        public JsonGatewayException(long returnCode, string message) : base(message)
        {
            _returnCode = returnCode;
            LogException();
        }

        public JsonGatewayException(long returnCode, string message, Exception ex) : base(message, ex)
        {
            _returnCode = returnCode;
            LogException();
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
            var stackTrace = StackTrace ?? new StackTrace().ToString();
            Logger.Error(string.Format("\nReturn code: {0} - Message: {1}\nStack Trace: {2}", ReturnCode, Message, stackTrace));
        }

        private bool ContainsHTML(string checkString)
        {
            return Regex.IsMatch(checkString, "<(.|\n)*?>");
        }

        public static JsonGatewayException Parse(string json)
        {
            try
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error(json);
                }

                var jsonGatewayError = JsonGatewayError.Parse(json);
                return new JsonGatewayException(jsonGatewayError.Error.Code, jsonGatewayError.Error.Message);
            }
            catch (Exception)
            {
                return null;
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