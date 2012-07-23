// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslationTask.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the TranslationTask type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    /// <summary>
    /// Translation Task Class
    /// </summary>
    public class TranslationTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationTask"/> class.
        /// </summary>
        /// <param name="uri">The Univeral Resource Identifier.</param>
        /// <param name="format">The translation task format.</param>
        public TranslationTask(string uri, TextFormat format)
        {
            Uri = uri;
            Format = format;
        }

        /// <summary>
        /// Gets the translation task format.
        /// </summary>
        /// <value>The translation task format.</value>
        public TextFormat Format { get; private set; }

        /// <summary>
        /// Gets the Univeral Resource Identifier.
        /// </summary>
        /// <value>The Univeral Resource Identifier.</value>
        public string Uri { get; private set; } 
    }
}
