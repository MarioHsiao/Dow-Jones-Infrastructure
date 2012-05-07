using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    [DataContract(Name = "clip", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "clip")]
    public class Clip
    {
        [DataMember(Name = "url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public Medium Medium { get; set; }

        [DataMember(Name = "medium")]
        [JsonProperty("medium")]
        public string MediumDescriptor
        {
            get { return Medium.ToString().ToLowerInvariant(); } 

            set
            {
                Medium temp;
                if (Enum.TryParse(value, true, out temp))
                {
                    Medium = temp;
                }
            }
        }

        [DataMember(Name = "duration")]
        [JsonProperty("duration")]
        public string Duration { get; set; }

        [DataMember(Name = "thumbNail")]
        [JsonProperty("thumbNail")]
        public string ThumbNail { get; set; }

        [DataMember(Name = "title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [DataMember(Name = "streamer")]
        [JsonProperty("streamer")]
        public string Streamer { get; set; }

        [DataMember(Name = "file")]
        [JsonProperty("file")]
        public string File { get; set; }

        [DataMember(Name = "type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [DataMember(Name = "bitRate")]
        [JsonProperty("bitRate")]
        public string BitRate { get; set; }

        [DataMember(Name = "frameRate")]
        [JsonProperty("frameRate")]
        public string FrameRate { get; set; }

        [DataMember(Name = "width")]
        [JsonProperty("width")]
        public string Width { get; set; }

        [DataMember(Name = "height")]
        [JsonProperty("height")]
        public string Height { get; set; }

        public string Language { get; set; }
    }
}