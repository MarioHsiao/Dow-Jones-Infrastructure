using System;

namespace DowJones.Utilities.Attributes
{
    /// <summary>
    /// Summary description for ReadSpeakerSupportedLanguage.
    /// </summary>
    
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReadSpeakerSupportedLanguage : Attribute
	{
        /// <summary>
        /// Gets or sets a value indicating whether this instance is editors choice language.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is editors choice language; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadSpeakerSupportedLanguage { get; set; }

        /// <summary>
		/// Initializes a new instance of the <see cref="EditorsChoiceLanguage"/> class.
		/// </summary>
        public ReadSpeakerSupportedLanguage()
		{
            IsReadSpeakerSupportedLanguage = true;
		}
    }
}
