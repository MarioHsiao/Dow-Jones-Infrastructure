using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Models.Multimedia
{
    [DataContract(Name="mediaContent", Namespace="")]
    public class MediaContent
    {
        [DataMember(Name="type")]
        public string Type;

        [DataMember(Name="medium")]
        public string Medium;

        [DataMember(Name="bitRate")]
        public string BitRate;

        [DataMember(Name="frameRate")]
        public string FrameRate;

        [DataMember(Name="duration")]
        public string Duration;

        [DataMember(Name="width")]
        public string Width;

        [DataMember(Name="height")]
        public string Height;

        [DataMember(Name = "streamer")]
        public string Streamer;

        [DataMember(Name = "file")]
        public string File;

        [DataMember(Name="language")]
        public string Language;

        [DataMember(Name="url")]
        public string Url;
    }
    [DataContract(Name = "mustPlayFromSource", Namespace = "")]
    public class MustPlayFromSource
    {

        [DataMember(Name = "status")]
        public bool Status;

        [DataMember(Name = "url")]
        public string Url;
    }

    [CollectionDataContract(Name = "mediaContents", ItemName = "mediaContent", Namespace = "")]
    public class MediaContents : List<MediaContent> { }
}
//type="video/x-flv" medium="video" bitrate="500" framerate="29.97" duration="414" width="480" height="360" lang="en" url="rtmp://cp49988.edgefcs.net/ondemand/74940/video/20110308/030811opinionjournal/030811opinionjournal.flv