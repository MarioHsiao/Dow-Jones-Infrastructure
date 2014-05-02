using EMG.Utility.Core;

namespace EMG.Utility.Managers.AutomatedTranslation.Core
{
    interface ITranslationProvider
    {
        object Translate(string[] fragments, TextFormat format, ContentLanguage sourcelanguage, ContentLanguage targetLanguage);
        object BeginTranslateTask(string[] fragments, TextFormat format, ContentLanguage sourcelanguage, ContentLanguage targetLanguage);
        object QueryTranslateTask(object task);
        object EndTranslateTask(object task);
        ContentLanguage[] GetSupportedTargetLanguages(ContentLanguage language);
        string[] GetTranslatedFragments(object translateResult);
        bool IsRunning(object translateResult);
        bool IsDone(object translateResult);
        bool IsFailed(object translateResult);

    }
}
