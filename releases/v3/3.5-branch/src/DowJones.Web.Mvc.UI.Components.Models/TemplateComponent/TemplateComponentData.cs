using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.TemplateComponent
{
    public class TemplateComponentData
    {
        [JsonProperty("textOne")]
        public string TextOne;

        [JsonProperty("textTwo")]
        public string TextTwo;
    }
}
