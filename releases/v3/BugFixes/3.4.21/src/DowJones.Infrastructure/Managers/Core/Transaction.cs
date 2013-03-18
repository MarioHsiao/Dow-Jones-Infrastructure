using System;
using System.Runtime.Serialization;
using System.Threading;

namespace DowJones.Managers.Core
{
    [DataContract(Name = "transaction", Namespace = "")]
    public class Transaction
    {
        // Need this for Serialization, DO NOT REMOVE
        public Transaction() : this(null, null) { }

        public Transaction(string name) : this(name, null) { }

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
}