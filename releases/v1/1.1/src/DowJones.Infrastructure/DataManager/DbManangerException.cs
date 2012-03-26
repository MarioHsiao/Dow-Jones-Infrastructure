using System;

namespace DowJones.Utilities.DataManager
{
    public class DbManangerException : ApplicationException 
    {
        #region Class Members
        private long _errorCode = Int64.MinValue;

        public long ErrorCode
        {
            get
            {
                return _errorCode;
            }

            private set
            {
                _errorCode = value;
            }
        }
        #endregion

        #region Constructors

        public DbManangerException(long errorCode)
        {
            ErrorCode = errorCode;
        }

        public DbManangerException(long errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public DbManangerException(long errorCode, Exception innerException)
            : base(string.Empty, innerException)
        {
            ErrorCode = errorCode;
        }


        public DbManangerException(long errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
        #endregion
    }
}
