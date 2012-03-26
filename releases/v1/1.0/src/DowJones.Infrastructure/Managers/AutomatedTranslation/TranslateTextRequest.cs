using System;
using DowJones.Utilities.Core;
using DowJones.Utilities.Managers.AutomatedTranslation.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation
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
