using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "SegmentFilter")]
    [Serializable]
    [XmlType(TypeName = "SegmentFilter", Namespace = Declarations.SchemaVersion)]
    public class SegmentFilter : Filter
    {
        private List<Segment> _segments;

        [JsonProperty(PropertyName = "segment")]
        [XmlElement("segment")]
        [DataMember(Name = "segment")]
        public List<Segment> Segments
        {
            get
            {
                if (_segments == null) _segments = new List<Segment>();
                return _segments;
            }
            set { _segments = value; }
        }
    }
}
