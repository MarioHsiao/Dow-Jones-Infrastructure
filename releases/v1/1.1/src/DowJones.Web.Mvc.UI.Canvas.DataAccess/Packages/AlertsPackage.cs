// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertsPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
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
    }

    [DataContract(Name = "alert", Namespace = "")]
    public class AlertID
    {
        /// <summary>
        /// Gets or sets the source code FCode.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        [DataMember(Name = "alertId")]
        public string AlertId { get; set; }

        [DataMember(Name = "isPrivate")]
        public bool IsPrivate{ get; set; }

        [DataMember(Name = "makePublic")]
        public bool MakePublic { get; set; }
    }
}