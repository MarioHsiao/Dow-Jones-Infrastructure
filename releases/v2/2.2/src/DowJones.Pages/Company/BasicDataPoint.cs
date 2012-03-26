using System;
using System.Runtime.Serialization;
using DowJones.Formatters;

namespace DowJones.Pages.Company
{
    [DataContract(Name = "basicDataPoint", Namespace = "")]
    public class BasicDataPoint : IDataPoint
    {
        [DataMember(Name = "date")]
        public DateTime? Date { get; set; }

        [DataMember(Name = "dateDisplay")]
        public string DateDisplay { get; set; }

        [DataMember(Name = "dataPoint")]
        public Number DataPoint { get; set; }
    }
}