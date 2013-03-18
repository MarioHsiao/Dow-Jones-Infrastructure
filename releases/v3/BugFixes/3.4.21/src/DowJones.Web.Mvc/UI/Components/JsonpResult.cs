using System.Web.Mvc;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components
{
    public class JsonpResult : ContentResult
    {
        public string Callback { get; set; }
        public object Model { get; set; }

        public JsonpResult()
        {
            ContentType = KnownMimeTypes.JavaScript;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string jsonifiedModel = JsonConvert.SerializeObject(Model);
            Content = string.Format("{0}({1});", Callback, jsonifiedModel);

            base.ExecuteResult(context);
        }
    }
}