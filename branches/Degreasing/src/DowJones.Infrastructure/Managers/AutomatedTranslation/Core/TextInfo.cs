using System;
using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Core
{
    public class TextInfo  : ICloneable
    {
        private string m_Body;
        private TextFormat m_Format;
        private ContentLanguage m_Language;

        public TextInfo(string body, TextFormat format, ContentLanguage language)
        {
            m_Body = body;
            m_Format = format;
            m_Language = language;
        }

        public string Body
        {
            get
            {
                return m_Body;
            }
            set
            {
                m_Body = value;
            }
        }

        public TextFormat Format
        {
            get
            {
                return m_Format;
            }
            set
            {
                m_Format = value;
            }
        }

        public ContentLanguage Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                m_Language = value;
            }
        }

        public object Clone()
        {
            return new TextInfo(m_Body, m_Format, m_Language);
        }
    }
}
