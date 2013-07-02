using DowJones.Infrastructure;
using DowJones.Managers.Abstract;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Prod.X.Core.Interfaces
{
    public interface IHeadlineUtilityService : IService
    {
        string GenerateUrl(ContentHeadline contentHeadline, bool isDuplicate);

        string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem, bool isBlob = false);
    }
}
