using DowJones.Utilities.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Core
{
    public interface ITranslateTask
    {
        object Identifier { get; }
        ContentLanguage TargetLanguage { get; }
        ITranslateItem SourceItem { get; }

    }
}
