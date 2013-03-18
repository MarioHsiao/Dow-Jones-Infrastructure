using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Search
{
    public class CodeDesc
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        public CodeDesc()
        {
        }

        public CodeDesc(string code, string desc)
        {
            Code = code;
            Desc = desc;
        }

        public CodeDesc(KeyValuePair<string,string> keyValuePair)
            : this(keyValuePair.Key, keyValuePair.Value)
        {
        }

        public CodeDesc(KeyValuePair<Enum, string> keyValuePair)
            : this(keyValuePair.Key.ToString(), keyValuePair.Value)
        {
        }
    }
}