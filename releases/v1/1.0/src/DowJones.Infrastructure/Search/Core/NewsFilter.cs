// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Newtonsoft.Json;
using System.Collections.Generic;
namespace DowJones.Utilities.Search.Core
{
    public class NewsFilterCollection
    {
        [JsonProperty("filterName")]
        public string FilterName { get; set; }

        [JsonProperty("filterKey")]
        public string FilterKey { get; set; }

        [JsonProperty("filter")]
        public List<NewsFilter> Filter { get; set; }
    }

    public class NewsFilter
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
