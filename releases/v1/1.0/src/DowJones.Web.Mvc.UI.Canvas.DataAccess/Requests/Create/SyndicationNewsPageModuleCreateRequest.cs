using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create
{
    [DataContract(Name = "syndicationNewsPageModuleCreateRequest", Namespace = "")]
    public class SyndicationNewsPageModuleCreateRequest : IModuleCreateRequest
    {
        private SyndicationIdCollection syndicationIdCollection = new SyndicationIdCollection();

        #region Implementation of IModuleRequest
        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "syndicationIds")]
        public SyndicationIdCollection SyndicationIdCollection
        {
            get { return syndicationIdCollection ?? (syndicationIdCollection = new SyndicationIdCollection()); }
            set { syndicationIdCollection = value; }
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public bool IsValid()
        {
            var originalCount = SyndicationIdCollection.Count;
            if (PageId.IsNullOrEmpty() || originalCount <= 0)
                return false;

            return originalCount == SyndicationIdCollection.GetUniques().Count;
        }
        #endregion
    }
}
