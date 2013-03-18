using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Pages.Multimedia
{
    [CollectionDataContract(Name = "mediaContents", ItemName = "mediaContent", Namespace = "")]
    public class MediaContents : List<MediaContent> { }
}
//type="video/x-flv" medium="video" bitrate="500" framerate="29.97" duration="414" width="480" height="360" lang="en" url="rtmp://cp49988.edgefcs.net/ondemand/74940/video/20110308/030811opinionjournal/030811opinionjournal.flv