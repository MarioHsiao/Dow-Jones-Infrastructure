using System;

namespace DowJones.Attributes
{
    /// <summary>
    /// Summary description for AssignedToken.
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssignedToken : Attribute
    {
        private readonly string _token = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedToken"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public AssignedToken(string token)
        {
            _token = token;
        }
        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token
        {
            get { return _token; }
        }
    }
}
