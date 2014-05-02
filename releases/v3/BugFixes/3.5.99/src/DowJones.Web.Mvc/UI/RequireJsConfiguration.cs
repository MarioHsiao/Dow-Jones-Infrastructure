using System.IO;
using Newtonsoft.Json;

namespace DowJones.Web
{
	public class RequireJsConfiguration
	{
		[JsonProperty("baseUrl")]
		public string BaseUrl { get; set; }

		[JsonProperty("waitSeconds")]
		public int WaitSeconds { get; set; }

		public RequireJsConfiguration()
		{
			WaitSeconds = 20;
		}

		public void WriteTo(TextWriter writer, bool includeScriptTags = false)
		{
			if (includeScriptTags)
				writer.WriteLine("<script type=\"text/javascript\">");

			writer.Write("require.config(");
			writer.Write(JsonConvert.SerializeObject(this));
			writer.WriteLine(");");

			if (includeScriptTags)
				writer.WriteLine("</script>");
		}

		public void WriteVariableTo(TextWriter writer, bool includeScriptTags = false)
		{
			writer.Write("var require = ");
			writer.Write(JsonConvert.SerializeObject(this));
			writer.WriteLine(";\n");
		}
	}
}