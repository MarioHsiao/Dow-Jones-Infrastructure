using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "ChartDates")]
    public class ChartDates
    {
        public ChartDates()
        {
            DateRange = new DateRange();
        }

        [DataMember(Name = "datePeriod")]
        public DatePeriod? DatePeriod { get; set; }

        [DataMember(Name = "dateRange")]
        public DateRange DateRange { get; set; }

        [DataMember(Name = "dateInterval")]
        public DateInterval? DateInterval { get; set; }

        [DataMember(Name = "dateType")]
        public DateType? DateType { get; set; }

        [DataMember(Name = "startDay")]
        public StartDay? StartDay { get; set; }

        [DataMember(Name = "allDates")]
        public bool AllDates { get; set; }

        [DataMember(Name = "fromToday")]
        public bool? FromToday { get; set; }
        [DataMember(Name = "lastCompleted")]
        public bool? LastCompleted { get; set; }
    }
}