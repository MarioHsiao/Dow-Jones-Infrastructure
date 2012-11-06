using System;

namespace DowJones.API.Common.ExceptionHandling
{
    /// <summary>
    /// summary description for Service Exception.
    /// </summary>
    public class ServiceException : BaseException
    {
        public ServiceException()
        {
        }

        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(long code)
            : base(code)
        {
            Code = code;
        }

        public ServiceException(long code, string message)
            : base(code, message)
        {
            Code = code;
        }

        public ServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ServiceException(string message, string source)
            : base(message)
        {
            base.Source = source;
        }

        public ServiceException(long code, string message, string source)
            : base(message)
        {
            Code = code;
            base.Source = source;
        }
    }
}
