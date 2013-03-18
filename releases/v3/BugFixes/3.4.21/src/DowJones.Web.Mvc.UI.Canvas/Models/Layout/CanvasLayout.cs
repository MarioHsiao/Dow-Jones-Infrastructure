using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public abstract class CanvasLayout
    {
        [JsonProperty("name")]
        public virtual string Name
        {
            get { return _name ?? GetType().Name.Replace("Layout", "").ToLower(); }
            set { _name = value; }
        }
        private string _name;
    }
}