using System;
using System.Collections.Generic;
using System.Text;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.DTO.Web.LOB;

namespace DowJones.Utilities.DTO.Web.Request
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
