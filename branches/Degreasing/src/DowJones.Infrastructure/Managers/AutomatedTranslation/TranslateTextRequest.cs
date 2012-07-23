using System;
using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Core;

namespace DowJones.Managers.AutomatedTranslation
{
    public class TranslateTextRequest : TranslateRequest
    {
        private readonly TextInfo m_Text;

        public TranslateTextRequest(TextInfo text, ContentLanguage targetLanguage)
            :base(targetLanguage)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }

            m_Text = text;
        }


        public override ITranslateItem GetTranslateItem()
        {
            return new TranslateText(m_Text, TargetLanguage);
        }
    }
}
