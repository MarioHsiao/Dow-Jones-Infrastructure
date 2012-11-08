using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.ServiceModels.PageService
{
    [DataContract(Namespace = "")]
    public class AbstractServiceResult : IServiceResult
    {
        #region IServiceResult Members

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "returnCode")]
        //// [XmlElement(Type = typeof (int), ElementName = "returnCode", IsNullable = false, Form = XmlSchemaForm.Qualified,
        //    Namespace = "")]
        [XmlElement(ElementName = "returnCode")]
        public long ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the feed.
        /// </summary>
        /// <value>The type of the feed.</value>
        [DataMember(Name = "statusMessage")]
        [XmlElement(ElementName = "statusMessage")]
        //// [XmlElement(Type = typeof (string), ElementName = "statusMessage", IsNullable = false,
        //    Form = XmlSchemaForm.Qualified, Namespace = "")]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        [DataMember(Name = "elapsedTime")]
        public long ElapsedTime { get; set; }

        [DataMember(Name = "audit", EmitDefaultValue = false)]
        public Audit Audit { get; set; }

        #endregion
    }

    [DataContract(Namespace = "")]
    public class AbstractServiceResult<TServicePartResult, TPackage> : AbstractServiceResult, IServicePartResults<TServicePartResult, TPackage>
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {
        [DataMember(Name = "partResults")]
        public virtual IEnumerable<TServicePartResult> PartResults { get; set; }

        [DataMember(Name = "maxPartsAvailable")]
        public int MaxPartsAvailable { get; set; }

    }

	[DataContract(Namespace = "")]
	public class AbstractModuleServiceResult<TServicePartResult, TPackage, TModule> :
		AbstractServiceResult<TServicePartResult, TPackage>
		where TServicePartResult : IServicePartResult<TPackage>
		where TPackage : IPackage
		where TModule : Module
	{
		
	}

}
