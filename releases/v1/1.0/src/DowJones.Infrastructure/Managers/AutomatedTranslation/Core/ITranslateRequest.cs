using DowJones.Utilities.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Core
{
    public interface ITranslateRequest
    {
        ContentLanguage TargetLanguage { get; }
        ITranslateItem GetTranslateItem();
    }
}
