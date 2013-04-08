using System;

namespace EMG.widgets.ui.attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class MimeType : Attribute
    {
        private readonly string m_MimeType = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeType"/> class.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        public MimeType(string mimeType)
        {
            m_MimeType = mimeType;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return m_MimeType; }
        }
    }
}