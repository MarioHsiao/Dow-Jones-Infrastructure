using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Core;

namespace DowJones.Managers.AutomatedTranslation
{
    public abstract class TranslateRequest : ITranslateRequest
    {
        private readonly ContentLanguage m_TargetLanguage;

        protected TranslateRequest(ContentLanguage targetLanguage)
        {
            m_TargetLanguage = targetLanguage;
        }

        public ContentLanguage TargetLanguage
        {
            get
            {
                return m_TargetLanguage;
            }
        }

        public abstract ITranslateItem GetTranslateItem();
    }
}
