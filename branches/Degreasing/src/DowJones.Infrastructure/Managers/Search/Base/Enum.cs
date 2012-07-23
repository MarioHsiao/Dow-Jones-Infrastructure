using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using DowJones.Attributes;
using DowJones.Search.Attributes;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.Base
{
    public enum SearchOperator
    {
        [XmlEnum("and")]
        And,
        [XmlEnum("or")]
        Or,
    }

    public enum FIIType
    {
        [XmlEnum("co")]
        Company,
        [XmlEnum("co:occur")]
        CompanyOccurance,
        [XmlEnum("ns")]
        NewsSubject,
        [XmlEnum("re")]
        Region,
        [XmlEnum("in")]
        Industry,
        [XmlEnum("au")]
        Author
    }


    /// <summary>
    /// The time frame.
    /// </summary>
    [DataContract(Name = "timeFrame", Namespace = "")]
    public enum TimeFrame
    {
        /// <summary>
        /// Three Months. Corresponds to last 97 days.
        /// </summary>
        [EnumMember]
        [TimeSlice(-90)] // might have to be 91 -- this is good for the three month date chart on discovery
        [AssignedToken("dateRangeLast3Months")]
        ThreeMonths = -90,

        /// <summary>
        /// Last Day. Corresponds to last 2 days.
        /// </summary>
        [EnumMember]
        [Obsolete]
        [TimeSlice(-1)]
        [AssignedToken("dateRangeLastDay")]
        LastDay = -1,

        /// <summary>
        /// Last Week. Corresponds to last week.
        /// </summary> 
        [EnumMember]
        [TimeSlice(-7)]
        [AssignedToken("dateRangeLastWeek")]
        LastWeek = -7,

        /// <summary>
        /// Last Month. Corresponds to last month.
        /// </summary>
        [EnumMember]
        [TimeSlice(-30)]
        [AssignedToken("dateRangeLastMonth")]
        LastMonth = -30,

        /// <summary>
        /// Last Six Year. Corresponds to last 183 days.
        /// </summary>
        [EnumMember]
        [TimeSlice(-180)]
        [AssignedToken("dateRangeLast6Months")]
        LastSixMonths = -180,

        /// <summary>
        /// Last Year. Corresponds to last 367 days.
        /// </summary>
        [EnumMember]
        [TimeSlice(-732)] // done for leap year
        [AssignedToken("dateRangeLastTwoYear")]
        LastTwoYear = -732,

        /// <summary>
        /// Last Year. Corresponds to last 367 days.
        /// </summary>
        [EnumMember]
        [TimeSlice(-367)] // done for leap year
        [AssignedToken("dateRangeLastYear")]
        LastYear = -367,
    }

    public static class TimeFrameUtility
    {
        public static Dates ConvertToCurrentDateRange(this TimeFrame timeFrame)
        {
            return new Dates
            {
                After = ((int)timeFrame).ToString()
            };
        }

        public static Dates ConvertToPreviousDateRange(this TimeFrame timeFrame)
        {
            var timeFrameInt = (int)timeFrame;
            return new Dates
            {
                After = (timeFrameInt * 2).ToString(),
                Before = (timeFrameInt + 1).ToString()
            };
        }
    }
}
