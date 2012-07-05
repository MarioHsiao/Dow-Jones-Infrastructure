In project `DowJones.Web.Mvc.UI.Components.Models`

* Create a new folder - `SampleComponent`

* Under `SampleComponent`, add class `SampleComponentData`:

<pre><code>
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SampleComponent
{
    [DataContract(Name = "sampleComponentdata", Namespace = "")]
    public class SampleComponentData
    {
        [DataMember(Name = "textOne")]
        [JsonProperty("textOne")]
        public string TextOne;

        [DataMember(Name = "textTwo")]
        [JsonProperty("textTwo")]
        public string TextTwo;
    }
}
</code></pre>