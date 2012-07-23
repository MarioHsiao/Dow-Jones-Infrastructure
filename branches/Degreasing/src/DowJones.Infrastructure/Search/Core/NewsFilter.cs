using DowJones.Converters;
using DowJones.Search.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace DowJones.Utilities.Search.Core
{
    public interface IFilterItem
    {
        
    }

    public class NewsFilterCollection
    {
        [JsonProperty("filterName")]
        public string FilterName { get; set; }

        [JsonProperty("filterKey")]
        public string FilterKey { get; set; }

        [JsonProperty("filter")]
        public List<FilterItem> Filter { get; set; }
    }

    public class FilterItem : IFilterItem
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class SourceFilterItem : List<FilterItem>, IFilterItem
    {
        
    }

    public class FilterList<T>
    {
        [JsonProperty("include")]
        public IEnumerable<T> Include { get; set; }
        [JsonProperty("exclude")]
        public IEnumerable<T> Exclude { get; set; }
        [JsonProperty("list")]
        public FilterItem List { get; set; }
        [JsonProperty("operator")]
        public SearchOperator Operator { get; set; }
    }

    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum FilterType
    {
        Company,
        Author,
        Executive,
        Subject,
        Industry,
        Region,
        Source,
        Language
    }

    public class SearchNewsFilters
    {
        [JsonProperty("author")]
        public IEnumerable<FilterItem> Author { get; set; }

        [JsonProperty("executive")]
        public IEnumerable<FilterItem> Executive { get; set; }

        [JsonProperty("industry")]
        public IEnumerable<FilterItem> Industry { get; set; }

        [JsonProperty("subject")]
        public IEnumerable<FilterItem> Subject { get; set; }

        [JsonProperty("region")]
        public IEnumerable<FilterItem> Region { get; set; }

        [JsonProperty("company")]
        public IEnumerable<FilterItem> Company { get; set; }

        [JsonProperty("source")]
        public IEnumerable<FilterItem> Source { get; set; }

        [JsonProperty("keyword")]
        public IEnumerable<string> Keyword { get; set; }

        [JsonProperty("dateRange")]
        public FilterItem DateRange { get; set; }
    }

    public  class SearchChannelFilters
    {
        [JsonProperty("author")]
        public FilterList<FilterItem> Author { get; set; }

        [JsonProperty("executive")]
        public FilterList<FilterItem> Executive { get; set; }

        [JsonProperty("industry")]
        public FilterList<FilterItem> Industry { get; set; }

        [JsonProperty("subject")]
        public FilterList<FilterItem> Subject { get; set; }

        [JsonProperty("region")]
        public FilterList<FilterItem> Region { get; set; }

        [JsonProperty("company")]
        public FilterList<FilterItem> Company { get; set; }

        [JsonProperty("source")]
        public FilterList<SourceFilterItem> Source { get; set; }   
    }

    
}
