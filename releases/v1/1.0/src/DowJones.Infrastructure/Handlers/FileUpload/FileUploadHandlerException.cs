using System;


namespace EMG.Utility.Handlers.FileUpload
{

    #region FileUploadErrorManager
   

    public class FileUploadHandlerException : ApplicationException
    {
        private long _errorCode;

        public long ErrorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }

        }

        public FileUploadHandlerException(string message)
            : base(message)
        {

        }
        public FileUploadHandlerException(long errorCode, string message)
            : base(message)
        {
            _errorCode = errorCode;
        }
        public FileUploadHandlerException(string message, Exception inner)
            : base(message, inner)
        {

        }

    }

    #endregion
}