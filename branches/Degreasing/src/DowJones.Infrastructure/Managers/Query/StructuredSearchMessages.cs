using System;
using System.Text;
using System.Collections.Generic;
using DowJones.Attributes;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.QueryUtility
{
    public class SearchStringContext
    {
        public string Scope;
        public StringBuilder SearchStringText = new StringBuilder();
        public List<SearchString> AdditionalSearchStrings = new List<SearchString>();
    }

    public class ContentSearchStringContext : SearchStringContext
    {
        public StringBuilder TraditionalSearchStringText = new StringBuilder();
    }

    public enum DateDtoType
    {
        DateRange,
        DateSinceDays,
        None
    }

    public class DateDTO
    {
        public DateFormat dateFormat = DateFormat.MMDDCCYY;
        public SearchDateRange dateRange = SearchDateRange._Unspecified;
        public YearMonthDay fromDate;
        public YearMonthDay toDate;
        public string SinceDays;
        public DateDtoType dateDtoType = DateDtoType.None;
    }

    public enum DateFormat
    {

        [AssignedToken("dateFormatMDY")]
        MMDDCCYY,

        [AssignedToken("dateFormatDMY")]
        DDMMCCYY,

        [AssignedToken("dateFormatYMD")] 
        CCYYMMDD,
    }

    public enum SearchDateRange
    {

        /// <remarks/>
        _Unspecified,

        /// <remarks/>
        LastDay,

        /// <remarks/>
        LastWeek,

        /// <remarks/>
        LastMonth,

        /// <remarks/>
        Last3Months,

        /// <remarks/>
        Last6Months,

        /// <remarks/>
        LastYear,

        /// <remarks/>
        Last2Years,

        /// <remarks/>
        SinceDays,

        /// <remarks/>
        Custom,
    }

    public class YearMonthDay
    {

        /// <remarks/>

        public int year;


        public bool yearSpecified;

        /// <remarks/>

        public int month;

        /// <remarks/>

        public bool monthSpecified;

        /// <remarks/>

        public int day;

        /// <remarks/>

        public bool daySpecified;


        /// <remarks/>

        public bool IsValidDate
        {
            get { return (yearSpecified && monthSpecified && daySpecified); }
        }

        /// <remarks/>

        public DateTime GetDate
        {
            get { return new DateTime(year, month, day); }
        }
    }
}