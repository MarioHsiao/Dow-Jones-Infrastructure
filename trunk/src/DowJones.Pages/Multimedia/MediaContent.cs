using System.Runtime.Serialization;

namespace DowJones.Pages.Multimedia
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
}