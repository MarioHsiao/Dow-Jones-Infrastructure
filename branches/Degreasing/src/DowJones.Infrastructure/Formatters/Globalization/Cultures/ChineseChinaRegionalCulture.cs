// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChineseChinaRegionalCulture.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Formatters.Globalization.Core;

namespace DowJones.Formatters.Globalization.Cultures
{
    public class ChineseChinaRegionalCulture : BaseAsianRegionalCulture
    {
        private const string INTERNAL_LANGUAGE_CODE = "zhcn";
        private const string LANGUAGE_NAME = "zh-CHS";
        private const string REGION_CODE = "CN";
        private const string TwoLetterIsoLanguageName = "zh";

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
            get { return TwoLetterIsoLanguageName; }
        }

        /// <summary>
        /// Gets the internal language code.
        /// </summary>
        /// <value>The internal language code.</value>
        public override string InternalLanguageCode
        {
            get { return INTERNAL_LANGUAGE_CODE; }
        }

        /// <summary>
        /// Gets the name of the language.
        /// </summary>
        /// <value>The name of the language.</value>
        public override string LanguageName
        {
            get { return LANGUAGE_NAME; }
        }

    }
}