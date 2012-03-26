using System;
using System.Collections.Generic;
using System.Text;

namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public enum DisseminationMethod
    {
        UnSpecified,
        Edition,
        Widget,
        Rss,
        PodCast,
    }

    #region SearchRegion
    public enum SearchPageType
    {
        NotApplicable,
        Other,
        SimpleSearch,
        SearchForm,
        SearchBuilder,
        iWorks,
        NewsstandSearch,
        FactivaMobile,
        DJCE_SimpleSearch,
        DJCE_AdvanceSearch,
    }

    public enum SearchIndicator
    {
        NotApplicable,
        Other,
        Initial,
        ContinuousNext,
        ContinuousPrev,
    }

    public enum DateRange
    {
        NotApplicable,
        AllDates,
        LastDay,
        LastWeek,
        LastMonth,
        LastThreeMonths,
        LastSixMonths,
        LastYear,
        LastTwoYears,
        Custom,
    }

    public enum DeduplicationType
    {
        NotApplicable,
        Off,
        VirtuallyIdentical,
        Similar,
    }

    public enum PublicationSortOrder
    {
        NotApplicable,
        ArrivalDate,
        Relevance,
        RecentPublicationDateFirst,
        OldestPublicationDateFirst
    }

    public enum SourceList
    {
        NotApplicable,
        None,
        ListOfIndividualSources,
        SavedSourceList,
    }

    public enum CompanyList
    {
        NotApplicable,
        None,
        ListOfIndividualCompanies,
        SavedCompanyList,
        SavedAndIndividualCompanyList,
    }

    public enum LeadSentenceDisplayOption
    {
        NotApplicable,
        NotAtAll,
        BelowHeadline,
        OnMouseover,
    }

    public enum SearchFreeTextArea
    {
        NotApplicable,
        FullArticle,
        FullArticlePlusIndexing,
        HeadlineAndLeadParagraph,
        Headline,
        Author,
        Custom,
    }

    public enum SavedSearches
    {
        NotApplicable,
        Personal,
        Group,
    }

    public enum ShareTypeOption
    {
        NotApplicable,
        None,
        Assigned,
    }
    #endregion

    #region ArticleRegion
    public enum OriginType
    {
        NotApplicable,
        Other,
        Search,
        Alert,
        Newsletter,
        Workspace,
        PHP,
        Newspage,
        PostProcessing,
        EntryPoint,
        Newsstand,
        EditorsChoice,
    }

    public enum PostProcessingType
    {
        NotApplicable,
        EMail,
        TextToSpeach,
        FormatForPrinting,
        FormatForSaving,
        RTF,
        PDF,
        XML,
        TranslateArticle_en,
        TranslateArticle_es,
        TranslateArticle_fr,
        TranslateArticle_it,
        TranslateArticle_de,
        TranslateArticle_ru,
        TranslateArticle_zhtw,
        TranslateArticle_zhcn,
        TranslateArticle_ja
    }

    public enum PostProcessingAdditional
    {
        NotApplicable,
        HeadlineFormat,
        ArticleFormat,
        HeadlineArticleAndTOC,
    }

    public enum DisplayFormatType
    {
        NotApplicable,
        HeadlineAndLeadParagraph,
        FullArticles,
        FullArticlePlusIndexing,
        KeywordInContext,
        Custom,
    }

    public enum ArticleCategoryType
    {
        NotApplicable,
        Publications,    
        WebNews,
        Pictures,
        Multimedia,
        Internal,
        Boards,
    }

    public enum ArticleCategorySubType
    {
        NotApplicable,      // default
        Newspapers,     // Publications
        Magazines,      // Publications
        Newswires,      // Publications 
        NewsSites,      // WebNews
        Blogs,          // WebNews
        Yes,            // Pictures
        No,             // Pictures
        Audio,          // Multimedia
        Video,          // Multimedia
    }

    public enum DestDetailsOption
    {
        NotApplicable,
        ODE,
        Scheduled,
        Continuous,
        Bundled,
    }

    public enum ViewDestination
    {
        NotApplicable,
        InProduct,
        NewsletterHTML,
    }

    public enum AutoCompletedTerm
    {
        False,
        True,
        NotApplicable
    }

    #endregion

    
    public class EnumMapper
    {

        #region SearchMapperRegion

        public static AutoCompletedTerm MapAutoCompletedTerm(string type)
        {
            switch (type)
            {
                case "0":
                    return AutoCompletedTerm.False;
                case "1":
                    return AutoCompletedTerm.True;
                default: // case "":
                    return AutoCompletedTerm.NotApplicable;
            }
        }

        public static string MapAutoCompletedTermToString(AutoCompletedTerm type)
        {
            switch (type)
            {
                case AutoCompletedTerm.False:
                    return "0";
                case AutoCompletedTerm.True:
                    return "1";
                default: //SearchPageType.NotApplicable
                    return "";
            }
        }

        public static SearchPageType MapStringToSearchPageType(string type)
        {
            switch (type)
            {
                case "SS":
                    return SearchPageType.SimpleSearch;
                case "SF":
                    return SearchPageType.SearchForm;
                case "SB":
                    return SearchPageType.SearchBuilder;
                case "IW":
                    return SearchPageType.iWorks;
                case "NS":
                    return SearchPageType.NewsstandSearch;
                case "FM":
                    return SearchPageType.FactivaMobile;
                case "DJCESS":
                    return SearchPageType.DJCE_SimpleSearch;
                case "DJCEAS":
                    return SearchPageType.DJCE_AdvanceSearch;
                case "OTH":
                    return SearchPageType.Other;
                default: // case "":
                    return SearchPageType.NotApplicable;
            }
        }

        public static string MapSearchPageTypeToString(SearchPageType type)
        {
            switch (type)
            {
                case SearchPageType.SimpleSearch:
                    return "SS";
                case SearchPageType.SearchForm:
                    return "SF";
                case SearchPageType.SearchBuilder:
                    return "SB";
                case SearchPageType.iWorks:
                    return "IW";
                case SearchPageType.NewsstandSearch:
                    return "NS";
                case SearchPageType.FactivaMobile:
                    return "FM";
                case SearchPageType.DJCE_SimpleSearch:
                    return "DJCESS";
                case SearchPageType.DJCE_AdvanceSearch:
                    return "DJCEAS";
                case SearchPageType.Other:
                    return "OTH";
                default: //SearchPageType.NotApplicable
                    return "";
            }
        }

        public static SearchIndicator MapStringToSearchIndicator(string type)
        {
            switch (type)
            {
                case "IN":
                    return SearchIndicator.Initial;
                case "CN":
                    return SearchIndicator.ContinuousNext;
                case "CP":
                    return SearchIndicator.ContinuousPrev;
                case "OTH":
                    return SearchIndicator.Other;
                default: // case "":
                    return SearchIndicator.NotApplicable;
            }
        }

        public static string MapSearchIndicatorToString(SearchIndicator type)
        {
            switch (type)
            {
                case SearchIndicator.Initial:
                    return "IN";
                case SearchIndicator.ContinuousNext:
                    return "CN";
                case SearchIndicator.ContinuousPrev:
                    return "CP";
                case SearchIndicator.Other:
                    return "OTH";
                default: //SearchIndicator.NotApplicable
                    return "";
            }
        }

        public static string MapDateRangeToString(DateRange type)
        {
            switch (type)
            {
                case DateRange.LastDay:
                    return "LD";
                case DateRange.LastWeek:
                    return "LW";
                case DateRange.LastMonth:
                    return "LM";
                case DateRange.LastThreeMonths:
                    return "L3M";
                case DateRange.LastSixMonths:
                    return "L6M";
                case DateRange.LastYear:
                    return "LY";
                case DateRange.LastTwoYears:
                    return "L2Y";
                case DateRange.Custom:
                    return "CUST";
                case DateRange.AllDates:
                    return "ALL";
                default: //DateRange.NotApplicable
                    return "";
            }
        }

        public static DateRange MapStringToDateRange(string type)
        {
            switch (type)
            {
                case "LD":
                    return DateRange.LastDay;
                case "LW":
                    return DateRange.LastWeek;
                case "LM":
                    return DateRange.LastMonth;
                case "L3M":
                    return DateRange.LastThreeMonths;
                case "L6M":
                    return DateRange.LastSixMonths;
                case "LY":
                    return DateRange.LastYear;
                case "L2Y":
                    return DateRange.LastTwoYears;
                case "CUST":
                    return DateRange.Custom;
                case "ALL":
                    return DateRange.AllDates;
                default: // case "":
                    return DateRange.NotApplicable;
            }
        }

        public static string MapDeduplicationTypeToString(DeduplicationType type)
        {
            switch (type)
            {
                case DeduplicationType.VirtuallyIdentical:
                    return "NEXACT";
                case DeduplicationType.Similar:
                    return "SIM";
                case DeduplicationType.Off:
                    return "OFF";
                default: //DeduplicationType.NotApplicable
                    return "";
            }
        }

        public static DeduplicationType MapStringToDeduplicationType(string type)
        {
            switch (type)
            {
                case "NEXACT":
                    return DeduplicationType.VirtuallyIdentical;
                case "SIM":
                    return DeduplicationType.Similar;
                case "OFF":
                    return DeduplicationType.Off;
                default: // case "":
                    return DeduplicationType.NotApplicable;
            }
        }

        public static string MapPublicationSortOrderToString(PublicationSortOrder type)
        {
            switch (type)
            {
                case PublicationSortOrder.Relevance:
                    return "REL";
                case PublicationSortOrder.ArrivalDate:
                    return "AD";
                case PublicationSortOrder.OldestPublicationDateFirst:
                    return "PDO";
                case PublicationSortOrder.RecentPublicationDateFirst:
                    return "PDM";
                default: //PublicationSortOrder.NotApplicable
                    return "";
            }
        }

        public static PublicationSortOrder MapStringToPublicationSortOrder(string type)
        {
            switch (type)
            {
                case "REL":
                    return PublicationSortOrder.Relevance;
                case "AD":
                    return PublicationSortOrder.ArrivalDate;
                case "PDO":
                    return PublicationSortOrder.OldestPublicationDateFirst;
                case "PDM":
                    return PublicationSortOrder.RecentPublicationDateFirst;
                default: // case "":
                    return PublicationSortOrder.NotApplicable;
            }
        }

        public static string MapSourceListToString(SourceList type)
        {
            switch (type)
            {
                case SourceList.ListOfIndividualSources:
                    return "LIS";
                case SourceList.SavedSourceList:
                    return "SSL";
                case SourceList.None:
                    return "NONE";
                default: //SourceList.NotApplicable
                    return "";
            }
        }

        public static SourceList MapStringToSourceList(string type)
        {
            switch (type)
            {
                case "LIS":
                    return SourceList.ListOfIndividualSources;
                case "SSL":
                    return SourceList.SavedSourceList;
                case "NONE":
                    return SourceList.None;
                default: // case "":
                    return SourceList.NotApplicable;
            }
        }

        public static string MapCompanyListToString(CompanyList type)
        {
            switch (type)
            {
                case CompanyList.ListOfIndividualCompanies:
                    return "LIC";
                case CompanyList.SavedCompanyList:
                    return "SCL";
                case CompanyList.SavedAndIndividualCompanyList:
                    return "CLIL";
                case CompanyList.None:
                    return "NONE";
                default: //CompanyList.NotApplicable
                    return "";
            }
        }

        public static CompanyList MapStringToCompanyList(string type)
        {
            switch (type)
            {
                case "LIC":
                    return CompanyList.ListOfIndividualCompanies;
                case "SCL":
                    return CompanyList.SavedCompanyList;
                case "CLIL":
                    return CompanyList.SavedAndIndividualCompanyList;
                case "NONE":
                    return CompanyList.None;
                default: // case "":
                    return CompanyList.NotApplicable;
            }
        }

        public static string MapLeadSentenceDisplayOptionToString(LeadSentenceDisplayOption type)
        {
            switch (type)
            {
                case LeadSentenceDisplayOption.BelowHeadline:
                    return "BH";
                case LeadSentenceDisplayOption.OnMouseover:
                    return "OM";
                case LeadSentenceDisplayOption.NotAtAll:
                    return "NONE";
                default: //LeadSentenceDisplayOption.NotApplicable
                    return "";
            }
        }

        public static LeadSentenceDisplayOption MapStringToLeadSentenceDisplayOption(string type)
        {
            switch (type)
            {
                case "BH":
                    return LeadSentenceDisplayOption.BelowHeadline;
                case "OM":
                    return LeadSentenceDisplayOption.OnMouseover;
                case "NONE":
                    return LeadSentenceDisplayOption.NotAtAll;
                default: // case "":
                    return LeadSentenceDisplayOption.NotApplicable;
            }
        }

        public static string MapSearchFreeTextToString(SearchFreeTextArea type)
        {
            switch (type)
            {
                case SearchFreeTextArea.Author:
                    return "BY";
                case SearchFreeTextArea.Custom:
                    return "CUST";
                case SearchFreeTextArea.Headline:
                    return "HL";
                case SearchFreeTextArea.HeadlineAndLeadParagraph:
                    return "HLP";
                case SearchFreeTextArea.FullArticlePlusIndexing:
                    return "FULR";
                case SearchFreeTextArea.FullArticle:
                    return "FULL";
                default: //SearchFreeTextArea.NotApplicable
                    return "";
            }
        }

        public static SearchFreeTextArea MapStringToSearchFreeText(string type)
        {
            switch (type)
            {
                case "BY":
                    return SearchFreeTextArea.Author;
                case "CUST":
                    return SearchFreeTextArea.Custom;
                case "HL":
                    return SearchFreeTextArea.Headline;
                case "HLP":
                    return SearchFreeTextArea.HeadlineAndLeadParagraph;
                case "FULR":
                    return SearchFreeTextArea.FullArticlePlusIndexing;
                case "FULL":
                    return SearchFreeTextArea.FullArticle;
                default: // case "":
                    return SearchFreeTextArea.NotApplicable;
            }
        }

        public static string MapSavedSearchesToString(SavedSearches type)
        {
            switch (type)
            {
                case SavedSearches.Group:
                    return "G";
                case SavedSearches.Personal:
                    return "P";
                default: //SavedSearches.NotApplicable
                    return "";
            }
        }

        public static SavedSearches MapStringToSavedSearches(string type)
        {
            switch (type)
            {
                case "G":
                    return SavedSearches.Group;
                case "P":
                    return SavedSearches.Personal;
                default: // case "":
                    return SavedSearches.NotApplicable;
            }
        }

        public static string MapShareTypeOptionToString(ShareTypeOption type)
        {
            switch (type)
            {
                case ShareTypeOption.Assigned:
                    return "ASSGN";
                case ShareTypeOption.None:
                    return "NONE";
                default: //ShareTypeOption.NotApplicable
                    return "";
            }
        }

        public static ShareTypeOption MapStringToShareTypeOption(string type)
        {
            switch (type)
            {
                case "ASSGN":
                    return ShareTypeOption.Assigned;
                case "NONE":
                    return ShareTypeOption.None;
                default: // case "":
                    return ShareTypeOption.NotApplicable;
            }
        }

        #endregion

        #region ArticleMapperRegion
        public static OriginType MapStringToOriginType(string type)
        {
            switch (type)
            {
                case "SR":
                    return OriginType.Search;
                case "AL":
                    return OriginType.Alert;
                case "NL":
                    return OriginType.Newsletter;
                case "WS":
                    return OriginType.Workspace;
                case "PH":
                    return OriginType.PHP;
                case "NP":
                    return OriginType.Newspage;
                case "PP":
                    return OriginType.PostProcessing;
                case "EP":
                    return OriginType.EntryPoint;
                case "EC":
                    return OriginType.EditorsChoice;
                case "OTH":
                    return OriginType.Other;
                default: // case "":
                    return OriginType.NotApplicable;
            }
        }

        public static string MapOriginTypeToString(OriginType type)
        {
            switch (type)
            {
                case OriginType.Search:
                    return "SR";
                case OriginType.Alert:
                    return "AL";
                case OriginType.Newsletter:
                    return "NL";
                case OriginType.Workspace:
                    return "WS";
                case OriginType.PHP:
                    return "PH";
                case OriginType.Newspage:
                    return "NP";
                case OriginType.PostProcessing:
                    return "PP";
                case OriginType.EntryPoint:
                    return "EP";
                case OriginType.Newsstand:
                    return "NS";
                case OriginType.EditorsChoice:
                    return "EC";
                case OriginType.Other:
                    return "OTH";
                default: //OriginType.NotApplicable
                    return "";
            }
        }

        public static PostProcessingType MapStringToPostProcessingType(string type)
        {
            switch (type)
            {
                case "EML":
                    return PostProcessingType.EMail;
                case "TTS":
                    return PostProcessingType.TextToSpeach;
                case "FFP":
                    return PostProcessingType.FormatForPrinting;
                case "FFS":
                    return PostProcessingType.FormatForSaving;
                case "RTF":
                    return PostProcessingType.RTF;
                case "PDF":
                    return PostProcessingType.PDF;
                case "XML":
                    return PostProcessingType.XML;
                case "AAT:de":
                    return PostProcessingType.TranslateArticle_de;
                case "AAT:en":
                    return PostProcessingType.TranslateArticle_en;
                case "AAT:es":
                    return PostProcessingType.TranslateArticle_es;
                case "AAT:fr":
                    return PostProcessingType.TranslateArticle_fr;
                case "AAT:it":
                    return PostProcessingType.TranslateArticle_it;
                case "AAT:ja":
                    return PostProcessingType.TranslateArticle_ja;
                case "AAT:ru":
                    return PostProcessingType.TranslateArticle_ru;
                case "AAT:zhcn":
                    return PostProcessingType.TranslateArticle_zhcn;
                case "AAT:zhtw":
                    return PostProcessingType.TranslateArticle_zhtw;
                default: // case "":
                    return PostProcessingType.NotApplicable;
            }
        }

        public static string MapPostProcessingTypeToString(PostProcessingType type)
        {
            switch (type)
            {
                case PostProcessingType.EMail:
                    return "EML";
                case PostProcessingType.TextToSpeach:
                    return "TTS";
                case PostProcessingType.FormatForPrinting:
                    return "FFP";
                case PostProcessingType.FormatForSaving:
                    return "FFS";
                case PostProcessingType.RTF:
                    return "RTF";
                case PostProcessingType.PDF:
                    return "PDF";
                case PostProcessingType.XML:
                    return "XML";
                case PostProcessingType.TranslateArticle_de:
                    return "AAT:de";
                case PostProcessingType.TranslateArticle_en:
                    return "AAT:en";
                case PostProcessingType.TranslateArticle_es:
                    return "AAT:es";
                case PostProcessingType.TranslateArticle_fr:
                    return "AAT:fr";
                case PostProcessingType.TranslateArticle_it:
                    return "AAT:it";
                case PostProcessingType.TranslateArticle_ja:
                    return "AAT:ja";
                case PostProcessingType.TranslateArticle_ru:
                    return "AAT:ru";
                case PostProcessingType.TranslateArticle_zhcn:
                    return "AAT:zhcn";
                case PostProcessingType.TranslateArticle_zhtw:
                    return "AAT:zhtw";
                default: //PostProcessingType.NotApplicable
                    return "";
            }
        }

        public static PostProcessingAdditional MapStringToPostProcessingAdditional(string type)
        {
            switch (type)
            {
                case "ART":
                    return PostProcessingAdditional.ArticleFormat;
                case "HAT":
                    return PostProcessingAdditional.HeadlineArticleAndTOC;
                case "HDL":
                    return PostProcessingAdditional.HeadlineFormat;
                default: // case "":
                    return PostProcessingAdditional.NotApplicable;
            }
        }

        public static string MapPostProcessingAdditionalToString(PostProcessingAdditional type)
        {
            switch (type)
            {
                case PostProcessingAdditional.ArticleFormat:
                    return "ART";
                case PostProcessingAdditional.HeadlineArticleAndTOC:
                    return "HAT";
                case PostProcessingAdditional.HeadlineFormat:
                    return "HDL";
                default: //PostProcessingAdditional.NotApplicable
                    return "";
            }
        }

        public static DisplayFormatType MapStringToDisplayFormatType(string type)
        {
            switch (type)
            {
                case "CUST":
                    return DisplayFormatType.Custom;
                case "FULR":
                    return DisplayFormatType.FullArticlePlusIndexing;
                case "FULL":
                    return DisplayFormatType.FullArticles;
                case "HLPI":
                    return DisplayFormatType.HeadlineAndLeadParagraph;
                case "KWIC":
                    return DisplayFormatType.KeywordInContext;
                default: // case "":
                    return DisplayFormatType.NotApplicable;
            }
        }

        public static string MapDisplayFormatTypeToString(DisplayFormatType type)
        {
            switch (type)
            {
                case DisplayFormatType.Custom:
                    return "CUST";
                case DisplayFormatType.FullArticlePlusIndexing:
                    return "FULR";
                case DisplayFormatType.FullArticles:
                    return "FULL";
                case DisplayFormatType.HeadlineAndLeadParagraph:
                    return "HLPI";
                case DisplayFormatType.KeywordInContext:
                    return "KWIC";
                default: //DisplayFormatType.NotApplicable
                    return "";
            }
        }

        public static ViewDestination MapStringToViewDestination(string type)
        {
            switch (type)
            {
                case "NLHT":
                    return ViewDestination.NewsletterHTML;
                case "IP":
                    return ViewDestination.InProduct;
                default: // case "":
                    return ViewDestination.NotApplicable;
            }
        }

        public static string MapViewDestinationToString(ViewDestination type)
        {
            switch (type)
            {
                case ViewDestination.NewsletterHTML:
                    return "NLHT";
                case ViewDestination.InProduct:
                    return "IP";
                default: //ViewDestination.NotApplicable
                    return "";
            }
        }
        
        #endregion

    }

}
