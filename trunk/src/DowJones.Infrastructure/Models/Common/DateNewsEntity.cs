using System;
using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [DataContract(Name = "dateNewsEntity", Namespace = "")]
    public class DateNewsEntity : NewsEntity
    {
        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }
        
        [DataMember(Name = "startDateFormattedString")]
        public String StartDateFormattedString { get; set; }

        [DataMember(Name = "endDate")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "endDateFormattedString")]
        public String EndDateFormattedString { get; set; }
    }
}
