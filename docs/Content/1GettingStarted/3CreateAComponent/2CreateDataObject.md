In project `DowJones.Web.Mvc.UI.Components.Models`

* Create a new folder - `SampleComponent`

* Under `SampleComponent`, add class `SampleComponentData`:

* Make sure that the class is made `public`

		using System.Runtime.Serialization;
		using Newtonsoft.Json;
	
		namespace DowJones.Web.Mvc.UI.Components.SampleComponent
		{
			public class SampleComponentData
			{
				[JsonProperty("textOne")]
				public string TextOne;
	
				[JsonProperty("textTwo")]
				public string TextTwo;
			}
		}