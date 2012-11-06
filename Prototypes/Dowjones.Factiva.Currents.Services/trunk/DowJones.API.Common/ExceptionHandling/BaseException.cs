using System;
using System.Net;
using DowJones.Factiva.Currents.Aggregrator;

namespace DowJones.API.Common.ExceptionHandling
{
    /// <summary>
    /// this is the base exception class that all project exceptions should derive from
    /// </summary>
    public abstract class BaseException : ApplicationException
    {
        public long Code;

        protected BaseException() {}

        protected BaseException(string message) : base(message)
        {
            Code = ErrorConstants.UnknownError;
        }

        protected BaseException(HttpStatusCode code, string message) : base(message)
        {
            Code = (int)code;
        }

        protected BaseException(long code, string message) : base(message)
        {
            Code = code;
        }

        protected BaseException(long code) {
            Code = code;
        }

        protected BaseException(string message, Exception innerException) : base(message, innerException) {}

        protected BaseException(string message, string source) : base(message)
        {
            base.Source = source;
        }

        protected BaseException(long code, string message, string source) : base(message)
        {
            Code = code;
            base.Source = source;
        }
    }
}
