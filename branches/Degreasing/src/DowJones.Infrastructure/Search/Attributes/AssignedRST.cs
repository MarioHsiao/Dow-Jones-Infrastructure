using System;

namespace DowJones.Search.Attributes
{
    /// <summary>
    /// Summary description for AssignedRST.
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class AssignedRST : Attribute
    {
        private readonly string _rstCode = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignedRST"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public AssignedRST(string code)
        {
            _rstCode = code;
        }
        /// <summary>
        /// Gets the RST code.
        /// </summary>
        /// <value>The RST code.</value>
        public string RSTCode
        {
            get { return _rstCode; }
        }
    }
}
