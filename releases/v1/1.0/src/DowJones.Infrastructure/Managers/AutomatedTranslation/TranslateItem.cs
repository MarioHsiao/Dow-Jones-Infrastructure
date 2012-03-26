using DowJones.Utilities.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation
{
    abstract class TranslateItem : ITranslateItem
    {
        private readonly ContentLanguage m_TargetLanguage;

        protected TranslateItem(ContentLanguage targetLanguage)
        {
            m_TargetLanguage = targetLanguage;
        }

        public ContentLanguage IntoLanguage
        {
            get
            {
                return m_TargetLanguage;   
            }
        }

        public abstract ContentLanguage GetLanguage();
        public abstract TextFormat GetFormat();
        public abstract string[] GetFragments();
        public abstract void SetFragments(string[] fragments);


        public abstract object Clone();
    }
}
