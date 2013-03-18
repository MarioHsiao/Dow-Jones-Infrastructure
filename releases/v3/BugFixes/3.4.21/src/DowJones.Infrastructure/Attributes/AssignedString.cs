using System;
using DowJones.Search.Attributes;

namespace DowJones.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssignedString : Attribute
    {
        private readonly string _token = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssignedRST"/> class.
		/// </summary>
		/// <param name="token">The token.</param>
        public AssignedString(string token)
		{
            _token = token;
		}
		/// <summary>
		/// Gets the RST code.
		/// </summary>
		/// <value>The RST code.</value>
		public string Value
		{
			get { return _token; }
		}
    }
}
