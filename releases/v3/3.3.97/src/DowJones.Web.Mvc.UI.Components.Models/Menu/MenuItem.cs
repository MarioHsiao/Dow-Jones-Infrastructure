using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Menu
{
    [JsonObject("menuItem")]
    public class MenuItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }


        /// <summary>
        /// Gets or sets any addtional meta data that needs to be passed on tab click event on client side.
        /// </summary>
        /// <value>
        /// The meta data object. Must be serailizable as JSON.
        /// </value>
        [JsonProperty("metaData")]
        public object MetaData { get; set; }

    }
}
