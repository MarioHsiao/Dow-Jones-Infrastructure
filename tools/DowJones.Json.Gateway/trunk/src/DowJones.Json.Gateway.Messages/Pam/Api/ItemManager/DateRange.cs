using System;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "DateRange")]
    public class DateRange
    {
        [DataMember(Name = "from")]
        public DateTime From { get; set; }

        [DataMember(Name = "to")]
        public DateTime To { get; set; }
    }
}