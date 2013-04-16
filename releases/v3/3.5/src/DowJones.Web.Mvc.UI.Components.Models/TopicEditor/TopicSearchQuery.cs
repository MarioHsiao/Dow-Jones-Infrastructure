using System;
using DowJones.Attributes;
using DowJones.Topic;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Components.Common;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;
using System.Collections.Generic;
using Track = Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Globalization;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.UI.Components.TopicEditor
{
    public class TopicSearchQuery
    {
        [JsonProperty("freeText")]
        public string FreeText { get; set; }

        [JsonProperty("filters")]
        public SearchChannelFilters Filters { get; set; }

        [JsonProperty("newsFilters")]
        public SearchNewsFilters NewsFilters { get; set; }
    }
}
