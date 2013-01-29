using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Models.TreeView
{
    [DataContract(Name = "treeViewNode", Namespace = "")]
    public class TreeViewNode
    {
        [JsonProperty("text")]
        [DataMember(Name = "text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [JsonProperty("children")]
        [DataMember(Name = "children")]
        public List<TreeViewNode> Children { get; set; }

        [JsonProperty("isChecked")]
        [DataMember(Name = "isChecked")]
        public bool IsChecked { get; set; }

        [JsonProperty("isOpen")]
        [DataMember(Name = "isOpen")]
        public bool IsOpen { get; set; }

        [JsonProperty("metadata")]
        [DataMember(Name = "metadata")]
        public object Metadata { get; set; }
    }
}
