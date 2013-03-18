namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslateText : ITranslateItem
    {
        TextInfo Text { get; }
    }
}
