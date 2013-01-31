using System;
using Factiva.BusinessLayerLogic.Exceptions;
using log4net;

namespace EMG.widgets.ui.exception
{
    /// <summary>
    /// Summary description for FactivaUIException.
    /// </summary>
    public class EmgWidgetsUIException : FactivaBusinessLogicException
    {
        private const int BASELINE_ERROR = 520400;
        /// <summary>
        /// unknown error
        /// </summary>
        public const int UNKNOWN_ERROR = BASELINE_ERROR;

       
        /// <summary>
        /// invalid create widget dto
        /// </summary>
        public const int INVALID_DIRECT_URL_WIDGET_DTO = BASELINE_ERROR + 4;
        
        
        /// <summary>
        /// Invalid folder id error.
        /// </summary>
        public const int INVALID_FOLDER_ID = BASELINE_ERROR + 1;

        /// <summary>
        /// Invalid input for ajax delegate error.
        /// </summary>
        public const int INVALID_INPUT_AJAX_DELEGATE = BASELINE_ERROR + 3;

        private static readonly ILog Log = LogManager.GetLogger(typeof (EmgWidgetsUIException));

        /// <summary>
        /// invalid alert widget
        /// </summary>
        public const int RETURN_URL_IS_NOT_SPECIFIED = BASELINE_ERROR + 6;

        /// <summary>
        /// Unable to retrieve content headlines resultset error.
        /// </summary>
        public const int UNABLE_TO_RETRIEVE_CONTENT_HEADLINES_RESULTSET = BASELINE_ERROR + 2;

        /// <summary>
        /// Unable to retrieve content headlines resultset error.
        /// </summary>
        public const int NO_VALID_FOLDERS = BASELINE_ERROR + 10;

        /// <summary>
        /// Unable to retrieve content headlines resultset error.
        /// </summary>
        public const int SHARE_PROPERTIES_ON_FOLDER_SET_TO_DENY = BASELINE_ERROR + 11;

        /// <summary>
        /// 
        /// </summary>
        public const int WORKSPACE_HAS_BEEN_DISSEMINATED = BASELINE_ERROR + 12;
        
        /// <summary>
        /// Unable to retrieve content headlines resultset error.
        /// </summary>
        public const int WIDGET_ARE_FEEDS_ACTIVE_SET_TO_OFF = BASELINE_ERROR + 13;

        
        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        public EmgWidgetsUIException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public EmgWidgetsUIException(long returnCodeFromFactivaService) : base(returnCodeFromFactivaService)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public EmgWidgetsUIException(string message, long returnCodeFromFactivaService) : base(message, returnCodeFromFactivaService)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EmgWidgetsUIException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EmgWidgetsUIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmgWidgetsUIException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="returnCodeFromFactivaService">The return code from factiva service.</param>
        public EmgWidgetsUIException(Exception innerException, long returnCodeFromFactivaService)
            : base(innerException, returnCodeFromFactivaService)
        {
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public override ILog Logger
        {
            get { return Log; }
        }
    }
}