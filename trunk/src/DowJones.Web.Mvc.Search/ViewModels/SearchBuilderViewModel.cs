using DowJones.AlertEditor;
using DowJones.Topic;
using DowJones.Web.Mvc.UI.Components.SearchBuilder;
using DowJones.Web.Mvc.UI.Components.TopicEditor;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.Search.ViewModels
{
    public class SearchBuilderViewModel
    {
        public string AlertId { get; set; }

        public Mvc.UI.Components.AlertEditor.AlertEditorModel AlertEditor { get; set; }

        public AlertProperties AlertProperties { get; set; }

        public string AlertPropertiesJson
        {
            get
            {
                if (AlertProperties == null)
                    return "null";

                return JsonConvert.SerializeObject(AlertProperties);
            }
        }


        public SearchBuilderModel SearchBuilder { get; set; }


        public TopicEditorModel TopicEditor { get; set; }

        public TopicProperties TopicProperties { get; set; }

        public string TopicPropertiesJson
        {
            get
            {
                if (TopicProperties == null)
                    return "null";

                return JsonConvert.SerializeObject(TopicProperties);
            }
        }


        public SearchBuilderViewModel()
        {
            AlertEditor = new Mvc.UI.Components.AlertEditor.AlertEditorModel();
            AlertProperties = new AlertProperties();

            TopicEditor = new TopicEditorModel();
            TopicProperties = new TopicProperties();
        }
    }
}