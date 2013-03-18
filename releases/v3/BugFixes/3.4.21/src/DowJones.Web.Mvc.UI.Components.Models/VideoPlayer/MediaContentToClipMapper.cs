using DowJones.Managers.Multimedia;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI.Components.VideoPlayer
{
    public class MediaContentToClipMapper : TypeMapper<MediaContent, Clip>
    {
        public override Clip Map(MediaContent source)
        {
            var temp = new Clip
                           {
                               BitRate = source.BitRate,
                               Duration = source.Duration,
                               File = source.File,
                               FrameRate = source.FrameRate,
                               Height = source.Height,
                               Language = source.Language,
                               MediumDescriptor = source.Medium,
                               Streamer = source.Streamer,
                               Type = source.Type,
                               Url = source.Url,
                               Width = source.Width,
                           };
            return temp;
        }
    }
}