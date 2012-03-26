using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Common
{
    public class SourceGroupItem : CodeDesc
    {
        [JsonProperty("collection")]
        public List<SourceGroupItem> SourceGroupCollection { get; set; }
    }
}