using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create
{
    [DataContract(Name = "alertsNewsPageModuleCreateRequest", Namespace = "")]
    public class AlertsNewsPageModuleCreateRequest : IModuleCreateRequest
    {
        private AlertCollection alertIdCollection = new AlertCollection();

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "alerts")]
        public AlertCollection AlertCollection
        {
            get { return alertIdCollection ?? (alertIdCollection = new AlertCollection()); }
            set { alertIdCollection = value; }
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public bool IsValid()
        {
            var originalCount = AlertCollection.Count;
            if (PageId.IsNullOrEmpty() || originalCount <= 0)
                return false;

            return originalCount == AlertCollection.GetUniques().Count;
        }
    }
}
