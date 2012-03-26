// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionalMapPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Tools.Ajax.PortalHeadlineList;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Models.Common;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    /// <summary>
    /// The regional map package.
    /// </summary>
    [DataContract(Name = "AbstractRegionalMapPackage", Namespace = "")]
    [KnownType(typeof(RegionalMapNewsVolumePackage)), KnownType(typeof(RegionalMapHeadlinesPackage))]
    public abstract class AbstractRegionalMapPackage : IPackage
    {
    }

    /// <summary>
    /// The regional map headlines package.
    /// </summary>
    [DataContract(Name = "RegionalMapHeadlinesPackage", Namespace = "")]
    public class RegionalMapHeadlinesPackage : AbstractRegionalMapPackage
    {
        /// <summary>
        /// Gets or sets the newsstand section headline list.
        /// </summary>
        /// <value>The newsstand section headline list.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionalMapRegionHeadlines")]
        [XmlElement(Type = typeof(List<RegionalMapHeadlines>), ElementName = "regionalMapRegionHeadlines", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("regionalMapRegionHeadlines")]
        public PortalHeadlineListDataResult RegionalMapRegionHeadlineList { get; set; }
    }

    /// <summary>
    /// The regional map news volume package.
    /// </summary>
    [DataContract(Name = "RegionalMapNewsVolumePackage", Namespace = "")]
    public class RegionalMapNewsVolumePackage : AbstractRegionalMapPackage
    {
        /// <summary>
        /// Gets or sets the region news volume.
        /// </summary>
        /// <value>The region news volume.</value>
        /// <remarks></remarks>
        [DataMember(Name = "regionNewsVolume")]
        [XmlElement(Type = typeof(List<NewsEntityNewsVolumeVariation>), ElementName = "regionNewsVolume", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("regionNewsVolume")]
        public List<NewsEntityNewsVolumeVariation> RegionNewsVolume { get; set; }
    }

    /// <summary>
    /// The regional map headlines.
    /// </summary>
    [DataContract(Name = "RegionalMapHeadlines", Namespace = "")]
    public class RegionalMapHeadlines : IPortalHeadlines
    {
        /// <summary>
        /// Gets or sets the feed URI.
        /// </summary>
        /// <value>The feed URI.</value>
        /// <remarks></remarks>
        [DataMember(Name = "code")]
        [XmlElement(Type = typeof(string), ElementName = "code", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("code")]
        public string Code { get; set; }

        #region IPortalHeadlines Members

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        /// <remarks></remarks>
        [DataMember(Name = "result")]
        [XmlElement(Type = typeof(PortalHeadlineListDataResult), ElementName = "result", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = ""), JsonProperty("result")]
        public PortalHeadlineListDataResult Result { get; set; }

        #endregion
    }
}