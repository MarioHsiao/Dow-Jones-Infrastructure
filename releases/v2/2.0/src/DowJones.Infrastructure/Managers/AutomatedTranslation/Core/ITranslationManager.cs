using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslationManager
    {
        bool IsIPAllowedForAutomatedTranslation(string ipCode);
        bool IsSourceAllowedForAutomatedTranslation(string sourceCode);
        ITranslateResult Translate(ITranslateRequest request);
        ITranslateTask BeginTranslateTask(ITranslateRequest request);
        ITranslateResult QueryTranslateTask(ITranslateTask task);
        ITranslateResult EndTranslateTask(ITranslateTask task);
        ContentLanguage[] GetSupportedTargetLanguages(ContentLanguage sourceLanguage);
    }
}
