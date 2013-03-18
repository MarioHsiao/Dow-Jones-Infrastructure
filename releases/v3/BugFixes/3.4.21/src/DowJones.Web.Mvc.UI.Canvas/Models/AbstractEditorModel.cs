using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DowJones.Web.Mvc.UI.Canvas.BaseComponents;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public abstract class AbstractEditorModel : ViewComponentModel
    {
        [ClientProperty("webServiceUrl")]
        public string WebServiceUrl { get; set; }

        [ClientProperty("method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HttpVerbs Method { get; set; }

        [ClientProperty("payloadFormat")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PayloadFormat PayloadFormat { get; set; }


        protected AbstractEditorModel()
        {
            Method = HttpVerbs.Put;
            PayloadFormat = PayloadFormat.Json;
        }
    }
}
