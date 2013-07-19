namespace DowJones.Formatters.Globalization.Cultures
{
    public class PortuguesePortugalRegionalCulture : SpanishSpainRegionalCulture
    {
        private const string REGION_CODE = "PT";
        private const string TWO_LETTER_ISO_LANGUAGE_NAME = "pt";

        /// <summary>
        /// Gets the region code.
        /// </summary>
        /// <value>The region code.</value>
        public override string RegionCode
        {
            get { return REGION_CODE; }
        }

        /// <summary>
        /// Gets the name of the two letter ISO language.
        /// </summary>
        /// <value>The name of the two letter ISO language.</value>
        public override string TwoLetterISOLanguageName
        {
            get { return TWO_LETTER_ISO_LANGUAGE_NAME; }
        }

        /// <summary>
        /// Gets the internal language code.
        /// </summary>
        /// <value>The internal language code.</value>
        public override string InternalLanguageCode
        {
            get { return TWO_LETTER_ISO_LANGUAGE_NAME; }
        }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        public override string LanguageName
        {
            get { return TWO_LETTER_ISO_LANGUAGE_NAME; }
        }
    }
}