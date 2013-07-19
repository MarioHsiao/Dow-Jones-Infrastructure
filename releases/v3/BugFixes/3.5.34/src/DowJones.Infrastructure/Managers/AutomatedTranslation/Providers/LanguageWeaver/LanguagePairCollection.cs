using System.Collections.Generic;

namespace DowJones.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    public class LanguagePairCollection : List<LanguagePair>
    {
        public LanguagePair Get(string fromLanguage, string intoLanguage)
        {
            LanguagePair pair = null;

            foreach (LanguagePair item in this)
            {
                if (!(item.FromLanguage == fromLanguage && item.IntoLanguage == intoLanguage))
                    continue;

                pair = item;
                break;
            }

            return pair;
        }
    }
}
