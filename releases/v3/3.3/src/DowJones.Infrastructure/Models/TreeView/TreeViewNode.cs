using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Models.TreeView
{
    public class TreeViewNode
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("children")]
        public List<TreeViewNode> Children { get; set; }

        [JsonProperty("isChecked")]
        public bool IsChecked { get; set; }

        [JsonProperty("isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }
    }
}
