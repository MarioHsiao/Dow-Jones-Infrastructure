using System;
using System.Runtime.Serialization;

namespace DowJones.Exceptions
{
    /// <summary>
    /// Generic exception for combining several other exceptions
    /// </summary>
    [DataContract(Name = "combinedException", Namespace = "")]
    [Serializable]
    public class CombinedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CombinedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The inner exceptions.</param>
        public CombinedException(string message, Exception[] innerExceptions)
            : base(message)
        {
            InnerExceptions = innerExceptions;
        }

        /// <summary>
        /// Gets the inner exceptions.
        /// </summary>
        /// <value>The inner exceptions.</value>
        public Exception[] InnerExceptions { get; protected set; }
    }
}