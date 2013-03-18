using DowJones.Topic;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;
using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.UI.Components.TopicEditor
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TopicData
    {
        private List<CodeDesc> _topicCategories;
        
        [JsonProperty("properties")]
        public TopicProperties Properties { get; set; }

        [JsonProperty("searchQuery")]
        public TopicSearchQuery SearchQuery { get; set; }

        /// <summary>
        /// Gets TopicCategories.
        /// </summary>
        [JsonProperty("topicCategories")]
        public List<CodeDesc> TopicCategories
        {
            get {
                if (_topicCategories == null)
                {
                    _topicCategories = new List<CodeDesc>();
                    var categories = new[]
                                         {
                                             MadeTopicCategory.Brand, MadeTopicCategory.Company,
                                             MadeTopicCategory.Custom, MadeTopicCategory.Message, MadeTopicCategory.Person,
                                             MadeTopicCategory.Subject
                                         };
                    foreach (var category in categories)
                    {
                        _topicCategories.Add(new CodeDesc { Code = ((int)category).ToString(), Desc = category.ToString() });
                    }
                }
                return _topicCategories;
            }
            //set { _topicCategories = value; }
        }

        //TODO: Add filter properties here
    }
}
