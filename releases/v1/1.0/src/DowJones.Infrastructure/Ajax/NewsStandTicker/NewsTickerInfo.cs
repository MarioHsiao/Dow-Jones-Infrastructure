using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Tools.Ajax.HeadlineList;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Tools.Ajax.NewsStandTicker
{
    [DataContract(Name = "NewsTickerHeadline", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "NewsTickerHeadline")]
    public class NewsTickerInfo
    {
        private Reference reference;

        [DataMember(Name = "sourceDescriptor")]
        [JsonProperty("sourceDescriptor")]
        public string Source { get; set; }

        [DataMember(Name = "title")]
        [JsonProperty("title")]
        public string Title;

        [DataMember(Name = "toolTip")]
        [JsonProperty("toolTip")]
        public string ToolTip { get; set; }

        [DataMember(Name = "headlineUrl")]
        [JsonProperty("headlineUrl")]
        public string HeadlineUrl { get; set; }

        [DataMember(Name = "publicationDateTimeDescriptor")]
        [JsonProperty("publicationDateTimeDescriptor")]
        public string PublicationDateTimeDescriptor { get; set; }

        [DataMember(Name = "author")]
        [JsonProperty("author")]
        public List<string> Author { get; set; }

        [DataMember(Name = "reference")]
        [JsonProperty("reference")]
        public Reference Reference
        {
            get { return reference ?? (reference = new Reference()); }
            set { reference = value; }
        }

        [DataMember(Name = "snippet")]
        [JsonProperty("snippet")]
        public List<string> Snippet { get; set; }

        [DataMember(Name = "time")]
        [JsonProperty("time")]
        public string Time;

        [DataMember(Name = "thumbnailImage")]
        [JsonProperty("thumbnailImage")]
        public ThumbnailImage thumbnailImage;       
    }
}
