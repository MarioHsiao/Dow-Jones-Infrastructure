using Factiva.Gateway.Messages.Archive.V1_0;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Core
{
    public interface ITranslateArticle : ITranslateItem
    {
        Article Article { get; }
    }
}
