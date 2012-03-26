using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create
{
    [DataContract(Name = "companyOverviewNewsPageModuleCreateRequest", Namespace = "")]
    public class CompanyOverviewNewsPageModuleCreateRequest : IModuleCreateRequest
    {
        private FCodeCollection fcodeCollection = new FCodeCollection();

        #region Implementation of IModuleCreateRequest

        /// <summary>
        /// Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        [DataMember(Name = "pageId")]
        public string PageId { get; set; }

        [DataMember(Name = "fcodes")]
        public FCodeCollection FCodeCollection
        {
            get { return fcodeCollection ?? (fcodeCollection = new FCodeCollection()); }
            set { fcodeCollection = value; }
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public bool IsValid()
        {
            return PageId.IsNotEmpty() && FCodeCollection.Count > 0;
        }

        #endregion
    }
}
