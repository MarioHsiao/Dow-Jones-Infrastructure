using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Multimedia
{
    public class VideoRequest : IRequest
    {
        public string AccessionNo { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(AccessionNo);
        }
    }
}
