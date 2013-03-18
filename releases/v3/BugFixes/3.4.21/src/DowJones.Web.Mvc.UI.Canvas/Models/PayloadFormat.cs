using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.BaseComponents
{
    public enum PayloadFormat
    {
        [JsonProperty("json")]
        Json,
        [JsonProperty("xml")]
        Xml
    }
}
