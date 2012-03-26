using System;
using DowJones.Utilities.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation
{
    class TranslateText : TranslateItem, ITranslateText
    {
        private readonly TextInfo m_Text;

        public TranslateText(TextInfo text, ContentLanguage targetLanguage)
            : base(targetLanguage)
        {
            m_Text = text;
        }

        public TextInfo Text
        {
            get
            {
                return m_Text;
            }
        }

        public override ContentLanguage GetLanguage()
        {
            return m_Text.Language;
        }

        public override TextFormat GetFormat()
        {
            return m_Text.Format;
        }

        public override string[] GetFragments()
        {
            return new string[] { m_Text.Body };
        }

        public override void SetFragments(string[] fragments)
        {
            if (fragments == null)
            {
                throw new ArgumentNullException("fragments");
            }

            if (fragments.Length > 0)
                m_Text.Body = fragments[0];
        }

        public override object Clone()
        {
            TextInfo text = (TextInfo)m_Text.Clone();
            
            return new TranslateText(text, IntoLanguage);
        }

      
    }
}
