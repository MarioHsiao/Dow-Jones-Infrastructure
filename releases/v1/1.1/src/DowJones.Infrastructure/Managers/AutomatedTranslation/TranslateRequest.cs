using DowJones.Utilities.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation
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
