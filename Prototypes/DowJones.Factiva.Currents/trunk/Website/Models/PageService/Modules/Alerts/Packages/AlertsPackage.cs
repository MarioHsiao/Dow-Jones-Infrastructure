using System.Runtime.Serialization;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Alerts.Packages
{
    [DataContract(Name = "alertsPackage", Namespace = "")]
    [KnownType(typeof(AlertsPackage))]
    public class AlertsPackage : AbstractHeadlinePackage, IViewAllSearchContextRef
    {
        /// <summary>
        /// Gets or sets the source code FCode.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        [DataMember(Name = "alertId")]
        public string AlertId { get; set; }

        /// <summary>
        /// Gets or sets the name of the source.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [DataMember(Name = "alertName")]
        public string AlertName { get; set; }

        #region Implementation of IViewAllSearchContextRef

        [DataMember(Name = "viewAllSearchContext")]
        public string ViewAllSearchContextRef { get; set; }

        #endregion

        [DataMember(Name = "alertDedupLevel")]
        public DeduplicationMode AlertDedupLevel { get; set; }
    }
}