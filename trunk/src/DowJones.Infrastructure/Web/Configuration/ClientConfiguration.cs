using System.IO;
using Newtonsoft.Json;

namespace DowJones.Web.Configuration
{
    public class ClientConfiguration
    {
        [JsonProperty("credentials")]
        public ClientCredentials Credentials { get; set; }

        [JsonProperty("preferences")]
        public ClientPreferences Preferences { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("debug", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Debug { get; set; }



        // Convenience pass-through to Credentials.SetProxyCredentials
        public void SetProxyCredentials(string userId, string @namespace)
        {
            if (Credentials != null)
                Credentials.SetProxyCredentials(userId, @namespace);
        }


        public void WriteTo(TextWriter writer, bool includeScriptTags = false)
        {
            if (includeScriptTags)
                writer.WriteLine("<script type=\"text/javascript\">");

            writer.WriteLine("if(!window['DJ']) { DJ = window['DJ'] = {} };");

            writer.Write("$.extend(true, DJ, {'config':");
            writer.Write(   JsonConvert.SerializeObject(this));
            writer.WriteLine("});");

            if (includeScriptTags)
                writer.WriteLine("</script>");
        }

    }
}
