using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Validation
{
    public class ValidateSyndicationFeedRequest : IRequest
    {
        #region Implementation of IModuleRequest

        public string FeedUri { get; set; }

        public bool IsValid()
        {
            return FeedUri.IsNotEmpty();
        }
        #endregion
    }
}
