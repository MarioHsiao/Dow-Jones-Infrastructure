using System;
using DowJones.Exceptions;

namespace DowJones.Web.Handlers.Items
{

    #region FileUploadErrorManager
   

    public class ItemHandlerException : ApplicationException
    {
        private long _errorCode = DowJonesUtilitiesException.ItemHandlerError;

        public long ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }

        }

        public ItemHandlerException(string message)
            : base(message)
        {

        }
        public ItemHandlerException(long errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }
        public ItemHandlerException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }

    #endregion
}