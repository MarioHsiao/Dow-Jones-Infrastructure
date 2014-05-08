using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Core;

namespace DowJones.Managers.AutomatedTranslation
{
    class TranslateTask : ITranslateTask
    {
        private readonly object m_Identifier;
        private readonly ContentLanguage m_TargetLanguage;
        private readonly ITranslateItem m_TranslateItem;

        public TranslateTask(object identifier, ContentLanguage targetLanguage, ITranslateItem translateItem)
        {
            m_Identifier = identifier;
            m_TranslateItem = translateItem;
            m_TargetLanguage = targetLanguage;
        }

        public object Identifier
        {
            get
            {
                return m_Identifier;
            }
        }

        public ContentLanguage TargetLanguage
        {
            get
            {
                return m_TargetLanguage;
            }
        }

        public ITranslateItem SourceItem
        {
            get
            {
                return m_TranslateItem;
            }
        }
    }
}
