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
    public class TopicEditorModel : ViewComponentModel
    {
        private TopicData _topicData = new TopicData();
        
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
       
    }
}
