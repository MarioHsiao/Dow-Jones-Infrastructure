using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.EntityList
{
    public class EntityList : ViewComponentModel
    {
        [JsonProperty("groups")]
        public IEnumerable<EntityGroup> Groups { get; set; }
    }

    public class EntityGroup
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("entities")]
        public IEnumerable<Entity> Entities { get; set; }
    }

    public class Entity
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}