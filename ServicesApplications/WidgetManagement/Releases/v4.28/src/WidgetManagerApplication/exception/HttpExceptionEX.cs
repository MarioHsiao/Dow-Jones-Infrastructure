using System;
using System.Net;
using System.Web;

namespace EMG.widgets.ui.exception
{
    /// <summary>
    /// Extention to the HttpException object.
    /// </summary>
    public class HttpExceptionEx : HttpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionEx"/> class.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <param name="message">The message.</param>
        /// <param name="hr">The hr.</param>
        public HttpExceptionEx(HttpStatusCode httpCode, string message, int hr)
            : base((int) httpCode, message, hr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionEx"/> class.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <param name="message">The message.</param>
        public HttpExceptionEx(HttpStatusCode httpCode, string message)
            : base((int) httpCode, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionEx"/> class.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public HttpExceptionEx(HttpStatusCode httpCode, string message, Exception innerException)
            : base((int) httpCode, message, innerException)
        {
        }
    }
}