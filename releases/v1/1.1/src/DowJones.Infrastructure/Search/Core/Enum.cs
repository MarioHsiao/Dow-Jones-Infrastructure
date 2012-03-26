using System.Xml.Serialization;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.Search.Attributes;

namespace DowJones.Utilities.Search.Core
{
    //public enum ClusterMode
    //{
    //    LabelsOnly,
    //    On,
    //    Off,
    //}

    //public enum DateFormat
    //{
    //    MMDDCCYY,
    //    DDMMCCYY,
    //}

    public enum DateQualifier
	{
        /// <summary>
        /// Three Months. Corresponds to last 97 days.
        /// </summary>
        [TimeSlice(-97)]
        [AssignedToken("dateRangeLast3Months")]
        ThreeMonths = 0,
        /// <summary>
        /// Last Day. Corresponds to last 2 days.
        /// </summary>
        [TimeSlice(-2)]
        [AssignedToken("dateRangeLastDay")]
        LastDay,
        /// <summary>
        /// Last Week. Corresponds to last week.
        /// </summary>
        [TimeSlice(-9)]
        [AssignedToken("dateRangeLastWeek")]
        LastWeek,
        /// <summary>
        /// Last Month. Corresponds to last month.
        /// </summary>
        [TimeSlice(-33)]
        [AssignedToken("dateRangeLastMonth")]
        LastMonth,
        /// <summary>
        /// Last Six Year. Corresponds to last 183 days.
        /// </summary>
        [TimeSlice(-183)]
        [AssignedToken("dateRangeLast6Months")]
        LastSixMonths,
        /// <summary>
        /// Last Year. Corresponds to last 367 days.
        /// </summary>
        [TimeSlice(-367)] // done for leap year
        [AssignedToken("dateRangeLastYear")]
        LastYear,
        /// <summary>
        /// Last two Year. Corresponds to last 367 days.
        /// </summary>
        [TimeSlice(-734)] // done for leap year
        [AssignedToken("dateRangeLast2Years")]
		LastTwoYears,
        [AssignedToken("dateRangeAllDates")]
		All,
        AfterAndOrBefore,
		EqualsDate,
		CustomDateRange,
	}

    public enum DedupeStatus
	{
		ProcessedAsSpecified,
		RemovedDueToDateRange,
		RemovedDueToCJKCharacters,
	}

    //public enum DeduplicationMode
    //{
    //    None,
    //    Similar,
    //    NearExact,
    //}

    //public enum DescriptorControlMode
    //{
    //    None,
    //    Company,
    //    All,
    //}

    public enum DocumentNumberType
	{
		Unspecified,
		Celex,
		EuropeanUnion,
	}

    public enum FIIType
	{
		Company,
		NewsSubject,
		Region,
		Industry,
		Language,
		Keyword,
		Restrictor,
	}

    public enum Language
	{
		Unspecified,
        [XmlEnum("en")]
		English,
        [XmlEnum("es")]
		Spanish,
        [XmlEnum("de")]
		German,
        [XmlEnum("fr")]
		French,
        [XmlEnum("it")]
		Italian,
        [XmlEnum("ja")]
		Japanese,
        [XmlEnum("ru")]
		Russian,
        [XmlEnum("zhcn")]
		SimplifiedChinese,
        [XmlEnum("zhtw")]
		ComplexChinese,
	}

    //public enum LinguisticsMode
    //{
    //    Off,
    //    Suggest,
    //    Modify,
    //}

    public enum NavigatorControllerMode
	{
		All,
		Custom,
		FII,
	}

    //public enum SearchMode
    //{
    //    All,
    //    Any,
    //    None,
    //    Phrase,
    //    Simple,
    //    Advanced,
    //    Traditional,
    //}

	public enum SearchOperator
	{
        [XmlEnum("and")]
		And,
        [XmlEnum("or")]
		Or,
	}

	public enum SearchQualifier
	{
		Global,
		SearchTwo,
		SearchTwoMoreSearchOptions,
		NewsViewsOneDimension,
		NewsViewsTwoDimentions,
		NewsStandViewAllSearch,
		EditorsChoiceViewAllSearch,
	}

	public enum SearchServer
	{
		Fast,
		Index,
	}

	public enum SearchSubCategory
	{
		Unspecified,
		NewsPapers,
		Magazines,
		NewsWires,
		Blogs,
		Audio,
		Video,
	}

	public enum SearchTwoDateQualifier
	{
		ThreeMonths,
		LastDay,
		LastWeek,
		LastMonth,
		LastSixMonths,
		LastYear,
	}

    //public enum SnippetType
    //{
    //    None,
    //    Fixed,
    //    Contextual,
    //}

    //public enum SortOrder
    //{
    //    PublicationDateChronological,
    //    PublicationDateReverseChronological,
    //    Relevance,
    //    RelevanceMediumFreshness,
    //    RelevanceHighFreshness,
    //    ArrivalTime,
    //}

    //public enum TimeNavigatorMode
    //{
    //    None,
    //    PublicationMonth,
    //    PublicationWeek,
    //    PulbicationDate,
    //    All,
    //}
}
