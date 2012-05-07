using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    [DataContract(Name = "medium", Namespace = "")]
    public enum Medium
    {
        Audio,
        Video,
    }

    [DataContract(Name = "clipCollection", Namespace = "")]
    [JsonObject(MemberSerialization.OptIn, Id = "clipCollection")]
    public class ClipCollection : List<Clip>
    {
        
    }

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

        [DataMember(Name = "thumbnail")]
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [DataMember(Name = "title")]
        [JsonProperty("title")]
        public string Title { get; set; }
    }


    public class VideoPlayerModel : ViewComponentModel
    {
        #region ..:: Client Properties ::..
        
        [ClientProperty("width")]
        public int Width { get; set; }

        [ClientProperty("height")]
        public int Height { get; set; }

        [ClientProperty("autoPlay")]
        public bool AutoPlay { get; set; }

        [ClientProperty("playerPath")]
        public string PlayerPath { get; set; }

        [ClientProperty("rtmpPluginPath")]
        public string RTMPPluginPath { get; set; }

        [ClientProperty("splashImagePath")]
        public string SplashImagePath { get; set; }

        [ClientProperty("playerKey")]
        public string PlayerKey { get; set; }
        #endregion

        #region ..:: Client Data ::..

        [ClientData]
        public ClipCollection PlayList { get; set; }

        #endregion

        #region ..:: Client Event Handlers ::..
        
        
        #endregion

    }
}
