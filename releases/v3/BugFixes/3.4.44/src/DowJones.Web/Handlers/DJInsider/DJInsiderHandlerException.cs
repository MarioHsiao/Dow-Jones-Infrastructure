using System;
using DowJones.Exceptions;

namespace DowJones.Web.Handlers.DJInsider
{

    #region DJInsiderHandlerException


    public class DJInsiderHandlerException : ApplicationException
    {
        private long _errorCode = DowJonesUtilitiesException.DjindexHandlerError;

        public long ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }

        }

        public DJInsiderHandlerException(string message)
            : base(message)
        {

        }
        public DJInsiderHandlerException(long errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }
        public DJInsiderHandlerException(long errorCode, string message, Exception inner)
            : base(message, inner)
        {
            _errorCode = errorCode;
        }

    }

    #endregion
}