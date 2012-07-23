using DowJones.Topic;
using DowJones.Utilities.Search.Core;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.Search;
using Factiva.Gateway.Messages.Assets.Queries.V1_0;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Components.TopicEditor
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TopicData
    {
        #region ..:: Private Members ::..
        private List<CodeDesc> _topicCategories;
        #endregion

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

    public class TopicSearchQuery
    {
        [JsonProperty("freeText")]
        public string FreeText { get; set; }

        [JsonProperty("filters")]
        public SearchChannelFilters Filters { get; set; }

        [JsonProperty("newsFilters")]
        public SearchNewsFilters NewsFilters { get; set; }
    }

    public class TopicEditorModel : ViewComponentModel
    {
        #region ..:: Private Members ::..
        private TopicData _topicData = new TopicData();
        #endregion

        #region ..:: Public Members ::..

        [ClientProperty("searchCriteriaEditable")]
        public bool SearchCriteriaEditable { get; set; }

        /// <summary>
        /// Gets or Sets the Topic Data.
        /// </summary>
        [ClientData]
        [JsonProperty("data")]
        public TopicData TopicData
        {
            get {
                return _topicData;
            }
            set { _topicData = value; }
        }
        #endregion
        
        #region ..:: Constructor ::..

        public TopicEditorModel()
        {
            
        }

        #endregion
    }
}
