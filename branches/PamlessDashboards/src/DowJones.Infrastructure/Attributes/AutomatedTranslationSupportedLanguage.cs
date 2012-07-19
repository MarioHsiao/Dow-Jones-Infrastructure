using System;

namespace DowJones.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AutomatedTranslationSupportedLanguage : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is supported by the automated translation feature.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is supported by the automated translation feature.; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupported { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomatedTranslationSupportedLanguage"/> class.
		/// </summary>
        public AutomatedTranslationSupportedLanguage()
		{
            IsSupported = true;
		}
    }
}
