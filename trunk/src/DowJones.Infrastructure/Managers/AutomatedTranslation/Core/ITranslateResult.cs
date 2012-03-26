namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslateResult
    {
        TranslateStatus Status { get; }
        ITranslateItem TranslatedItem { get; }
    }
}
