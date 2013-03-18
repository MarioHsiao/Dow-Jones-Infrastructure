using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation
{
    class LanguagePair
    {
        private readonly ContentLanguage m_FromLanguage;
        private readonly ContentLanguage m_IntoLanguage;

        public LanguagePair(ContentLanguage fromLanguage, ContentLanguage intoLanguage)
        {
            m_FromLanguage = fromLanguage;
            m_IntoLanguage = intoLanguage;
        }

        public ContentLanguage FromLanguage
        {
            get { return m_FromLanguage; }
        }

        public ContentLanguage IntoLanguage
        {
            get { return m_IntoLanguage; }
        }
    }
}
