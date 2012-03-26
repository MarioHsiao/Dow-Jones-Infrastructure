using DowJones.Attributes;
using DowJones.DTO.Web.LOB;

namespace DowJones.DTO.Web.Request
{
    public class ArchiveFileRequestDTO : AbstractRequestDTO
    {
        [ParameterName("accessno")]
        public string AccessionNumber;
        [ParameterName("reference")]
        public string Reference;
        [ParameterName("mimetype")]
        public string MimeType;
        [ParameterName("imagetype")]
        public string ImageType;


    }
}
