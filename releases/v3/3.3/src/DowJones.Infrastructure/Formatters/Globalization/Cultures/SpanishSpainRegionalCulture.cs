using DowJones.Formatters.Globalization.Core;

namespace DowJones.Formatters.Globalization.Cultures
{
    public class SpanishSpainRegionalCulture : BaseLatinRegionalCulture
    {
        private const string REGION_CODE = "ES";
        private const string TWO_LETTER_ISO_LANGUAGE_NAME = "es";


        /// <summary>
        /// Gets the long date pattern.
        /// </summary>
        /// <value>The long date pattern.</value>
        protected override string longDatePattern
        {
            get { return "dd 'de' MMMM 'de' yyyy"; }
        }

        /// <summary>
        /// Gets the standard date pattern.
        /// </summary>
        /// <value>The standard date pattern.</value>
        protected override string standardDatePattern
        {
            get { return "d 'de' MMMM 'de' yyyy"; }
        }


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