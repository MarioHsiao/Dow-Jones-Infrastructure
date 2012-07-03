using Newtonsoft.Json;

namespace DowJones.Search.Navigation
{
    public class Keyword
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
        public float Weight { get; set; }

        
        public Keyword()
        {
        }

        public Keyword(string name, float weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}