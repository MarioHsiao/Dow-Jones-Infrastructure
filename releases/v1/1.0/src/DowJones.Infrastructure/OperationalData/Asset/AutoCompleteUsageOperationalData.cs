using System.Collections.Generic;
using System.Xml.Serialization;
using DowJones.Utilities.OperationalData.Asset;

namespace DowJones.Utilities.OperationalData.AutoCompleteUsage
{
    public class AutoCompleteUsageOperationalData : BaseAssetOperationalData
    {

        /// <summary>
        /// Gets or sets the Source browser category.
        /// </summary>
        /// <value>The Source browser category.</value>
        public SourceBrowserCategory SrcBrowserCategory
        {
            get
            {
                return MapStringToSourceBrowserCategory(Get(ODSConstants.FCS_OD_SOURCE_BROWSER_CATEGORY));
            }
            set
            {
                Add(ODSConstants.FCS_OD_SOURCE_BROWSER_CATEGORY, MapSourceBrowserCategoryToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the source page.
        /// </summary>
        /// <value>The source page.</value>
        public string SourcePage
        {
            get { return Get(ODSConstants.FCS_OD_SOURCE_PAGE); }
            set { Add(ODSConstants.FCS_OD_SOURCE_PAGE, value); }
        }

        /// <summary>
        /// Gets or sets the requestor IP.
        /// </summary>
        /// <value>The requestor IP.</value>
        public string RequestorIP
        {
            get { return Get(ODSConstants.FCS_OD_REQUESTOR_IP); }
            set { Add(ODSConstants.FCS_OD_REQUESTOR_IP, value); }
        }

        public AutoCompleteUsageOperationalData() { }

        protected AutoCompleteUsageOperationalData(IDictionary<string, string> list) : base(list) { }

        #region EnumRegion
        public enum SourceBrowserCategory
        {
            Company,
            Industry, 
            Source, 
            Region, 
            News_Subject, 
            Executive, 
            Author, 
            Keyword,
            NotApplicable
        }
        #endregion

        #region MapperRegion
        public static SourceBrowserCategory MapStringToSourceBrowserCategory(string type)
        {
            switch (type)
            {
                case "Company":
                    return SourceBrowserCategory.Company;
                case "Industry":
                    return SourceBrowserCategory.Industry;
                case "Source":
                    return SourceBrowserCategory.Source;
                case "Region":
                    return SourceBrowserCategory.Region;
                case "News Subject":
                    return SourceBrowserCategory.News_Subject;
                case "Executive":
                    return SourceBrowserCategory.Executive;
                case "Author":
                    return SourceBrowserCategory.Author;
                case "Keyword":
                    return SourceBrowserCategory.Keyword;
                default:
                    return SourceBrowserCategory.NotApplicable;

            }
        }
        public static string MapSourceBrowserCategoryToString(SourceBrowserCategory type)
        {
            switch (type)
            {
                case SourceBrowserCategory.Company:
                    return "Company";
                case SourceBrowserCategory.Industry:
                    return "Industry";
                case SourceBrowserCategory.Source:
                    return "Source";
                case SourceBrowserCategory.Region:
                    return "Region";
                case SourceBrowserCategory.News_Subject:
                    return "News Subject";
                case SourceBrowserCategory.Executive:
                    return "Executive";
                case SourceBrowserCategory.Author:
                    return "Author";
                case SourceBrowserCategory.Keyword:
                    return "Keyword";
                default:
                    return "";
            }
        }
        #endregion
    }
}
