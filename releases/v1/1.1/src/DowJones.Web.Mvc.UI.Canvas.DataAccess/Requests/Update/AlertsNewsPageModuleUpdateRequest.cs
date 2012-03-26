// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertsNewsPageModuleUpdateRequest.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update
{
    [DataContract(Name = "alertsNewsPageModuleUpdateRequest", Namespace = "")]
    public class AlertsNewsPageModuleUpdateRequest : IModuleUpdateRequest
    {
        private AlertCollection alertCollection = new AlertCollection(); 

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

        [DataMember(Name = "alerts")]
        public AlertCollection AlertCollection
        {
            get { return alertCollection ?? (alertCollection = new AlertCollection()); }
            set { alertCollection = value; }
        }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        public bool IsValid()
        {
            var originalCount = AlertCollection.Count;
            if (PageId.IsNullOrEmpty() || ModuleId.IsNullOrEmpty() || originalCount <= 0)
                return false;

            return originalCount == AlertCollection.GetUniques().Count;
        }
        #endregion
    }
}
