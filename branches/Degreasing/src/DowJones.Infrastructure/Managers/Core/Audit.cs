using System;
using System.Runtime.Serialization;

namespace DowJones.Managers.Core
{
    [DataContract(Name = "audit", Namespace = "")]
    public class Audit : IAudit
    {
        private TransactionCollection transactionCollection;

        public Audit()
        {
            CreationDateTime = DateTime.Now;
        }

        [DataMember(Name = "transactions")]
        public TransactionCollection TransactionCollection
        {
            get { return transactionCollection ?? (transactionCollection = new TransactionCollection()); }
            set { transactionCollection = value; }
        }

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