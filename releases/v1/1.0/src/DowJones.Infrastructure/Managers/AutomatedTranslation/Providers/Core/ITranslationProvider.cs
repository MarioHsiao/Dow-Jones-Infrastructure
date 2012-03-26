using DowJones.Utilities.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Providers.Core
{
    interface ITranslationProvider
    {
        object Translate(string[] fragments, TextFormat format, ContentLanguage sourcelanguage, ContentLanguage targetLanguage);
        object BeginTranslateTask(string[] fragments, TextFormat format, ContentLanguage sourcelanguage, ContentLanguage targetLanguage);
        object QueryTranslateTask(object task);
        object EndTranslateTask(object task);
        string[] GetTranslatedFragments(object translateResult);
        bool IsTranslating(object translateResult);
        bool IsDone(object translateResult);
        bool IsFailed(object translateResult);
    }
}
