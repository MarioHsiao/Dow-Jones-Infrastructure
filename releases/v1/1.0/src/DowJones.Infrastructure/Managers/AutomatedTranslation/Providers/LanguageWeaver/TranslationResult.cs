using DowJones.Utilities.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Providers.LanguageWeaver
{
    class TranslationResult
    {
        private readonly string m_Status;
        private readonly string m_TranslatedText;
        private readonly TextFormat m_Format;


        public TranslationResult(string status, string translatedText, TextFormat format)
        {
            m_Status = status;
            m_TranslatedText = translatedText;
            m_Format = format;
        }

        public TextFormat Format
        {
            get { return m_Format; }
        }

        public string Status
        {
            get { return m_Status; }
        }

        public string TranslatedText
        {
            get { return m_TranslatedText; }
        }
    }
}
