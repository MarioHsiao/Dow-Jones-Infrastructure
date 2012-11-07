using System;
using System.Runtime.Serialization;
using DowJones.Managers.Core;
using IAudit = DowJones.Pages.IAudit;

namespace DowJones.Factiva.Currents.Website.Models.PageService
{
    [DataContract(Name = "audit", Namespace = "")]
    public class Audit : IAudit
    {

        [DataMember(Name = "transactions")]
		public TransactionCollection TransactionCollection { get; set; }

        #region Implementation of IAudit

        [DataMember(Name = "elapsedTime")]
        public long ElapsedTime { get; set; }

        [DataMember(Name = "returnCode")]
        public long ReturnCode { get; set; }

        #endregion

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "creationDateTime")]
        public DateTime CreationDateTime { get; private set; }

        [DataMember(Name = "additionalInfo")]
        public string AdditionalInfo { get; set; }
    }
}
