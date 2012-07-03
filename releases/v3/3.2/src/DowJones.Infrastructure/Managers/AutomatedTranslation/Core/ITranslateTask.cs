using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslateTask
    {
        object Identifier { get; }
        ContentLanguage TargetLanguage { get; }
        ITranslateItem SourceItem { get; }

    }
}
