// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleUpdateRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update
{
    [DataContract(Name = "companyOverviewNewsPageModuleUpdateRequest", Namespace = "")]
    public class CompanyOverviewNewsPageModuleUpdateRequest : IModuleUpdateRequest
    {
        private FCodeCollection fcodeCollection = new FCodeCollection(); 

        #region Implementation of IModuleRequest

        /// <summary>
        /// Gets or sets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        /// <remarks></remarks>
        [DataMember(Name = "moduleId")]
        public string ModuleId { get; set; }
        
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
            return PageId.IsNotEmpty() && ModuleId.IsNotEmpty() && FCodeCollection.Count > 0;
        }
        #endregion
    }
}
