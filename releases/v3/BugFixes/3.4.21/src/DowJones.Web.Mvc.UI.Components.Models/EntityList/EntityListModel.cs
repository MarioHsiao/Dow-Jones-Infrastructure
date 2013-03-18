using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.EntityList
{
    public class EntityListModel : ViewComponentModel
    {
        [JsonProperty("groups")]
        public IEnumerable<EntityGroup> Groups { get; set; }

        public EntityListModel()
        {
            Groups = Enumerable.Empty<EntityGroup>();
        }
    }

    public class EntityGroup
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("entities")]
        public IEnumerable<Entity> Entities { get; set; }

        public EntityGroup()
        {
            Entities = Enumerable.Empty<Entity>();
        }
    }

    public class Entity
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}