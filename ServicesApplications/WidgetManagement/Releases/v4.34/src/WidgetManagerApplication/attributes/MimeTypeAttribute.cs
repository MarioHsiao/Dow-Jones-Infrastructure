using System;

namespace EMG.widgets.ui.attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class MimeType : Attribute
    {
        private readonly string _mimeType = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeType"/> class.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        public MimeType(string mimeType)
        {
            _mimeType = mimeType;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return _mimeType; }
        }
    }
}