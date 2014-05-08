using EMG.Utility.Core;

namespace EMG.Utility.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    class TranslationJob
    {
        private readonly string m_Uri;
        private readonly TextFormat m_Format;

        public TranslationJob(string uri, TextFormat format)
        {
            m_Uri = uri;
            m_Format = format;
        }

        public TextFormat Format
        {
            get { return m_Format; }
        }

        public string Uri
        {
            get { return m_Uri; }
        }
    }
}
