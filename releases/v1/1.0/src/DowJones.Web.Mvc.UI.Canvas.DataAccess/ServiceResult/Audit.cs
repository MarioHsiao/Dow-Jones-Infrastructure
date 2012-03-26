// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Audit.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
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

    [DataContract(Name = "transaction", Namespace = "")]
    public class Transaction
    {
        public Transaction(string name) : this(name, null)
        {
        }

        public Transaction(string name, string detail)
        {
            Name = name;
            Detail = detail;
            CreationDateTime = DateTime.Now;
            ManagedThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            ApartmentState = Thread.CurrentThread.GetApartmentState();
        }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "controlData")]
        public ControlData ControlData { get; set; }

        [DataMember(Name = "creationDateTime")]
        public DateTime CreationDateTime { get; private set; }

        [DataMember(Name = "rawResponse")]
        public string RawResponse { get; set; }

        [DataMember(Name = "elapsedTime")]
        public long ElapsedTime { get; set; }

        [DataMember(Name = "returnCode")]
        public long ReturnCode { get; set; }

        [DataMember(Name = "detail")]
        public string Detail { get; set; }

        public string ManagedThreadId { get; private set; }

        public ApartmentState ApartmentState { get; private set; }
    }

    [CollectionDataContract(Name = "transactions", ItemName = "transaction", Namespace = "")]
    public class TransactionCollection : List<Transaction>
    {
        public TransactionCollection()
        {
        }

        public TransactionCollection(IEnumerable<Transaction> transactions) : base(transactions)
        {
        }
    }
}
