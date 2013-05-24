using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslateRequest
    {
        ContentLanguage TargetLanguage { get; }
        ITranslateItem GetTranslateItem();
    }
}
