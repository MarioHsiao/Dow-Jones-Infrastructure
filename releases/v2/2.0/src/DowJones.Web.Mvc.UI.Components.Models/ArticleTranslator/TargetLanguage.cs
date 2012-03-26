using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Globalization;

namespace DowJones.Web.Mvc.UI.Components.ArticleTranslator
{
    public class TargetLanguage
    {
        public static IEnumerable<TargetLanguage> All
        {
            get
            {
                if (_all == null)
                    _all = 
                        Enum.GetValues(typeof(ContentLanguage))
                            .Cast<ContentLanguage>()
                            .Select(x => new TargetLanguage(x));

                return _all;
            }
        }
        private static IEnumerable<TargetLanguage> _all;

        /// <summary>
        /// ContentLanguages available in EMG.Utility
        /// </summary>
        public ContentLanguage contentLanguage { get; private set; }

        /// <summary>
        /// Lang text to override.
        /// </summary>
        public string LangText { get; private set; }

        public TargetLanguage(ContentLanguage contentLanguage)
        {
            this.contentLanguage = contentLanguage;
            LangText = ResourceTextManager.Instance.GetAssignedToken(contentLanguage);
        }
    }
}
