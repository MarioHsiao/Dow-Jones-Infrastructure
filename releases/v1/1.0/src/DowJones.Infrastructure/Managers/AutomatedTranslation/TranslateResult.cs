using DowJones.Utilities.Managers.AutomatedTranslation.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation
{
    class TranslateResult : ITranslateResult
    {
        private readonly TranslateStatus m_Status;
        private readonly ITranslateItem m_TranslatedItem;

        public TranslateResult(TranslateStatus status, ITranslateItem item)
        {
            m_Status = status;
            m_TranslatedItem = item;
        }

        public TranslateStatus Status
        {
            get
            {
                return m_Status;
            }
        }

        public ITranslateItem TranslatedItem
        {
            get
            {
                return m_TranslatedItem;
            }
        }
    }
}
