using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Ajax.PortalHeadlineList;
using Newtonsoft.Json;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.RegionalMap.Packages
{
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