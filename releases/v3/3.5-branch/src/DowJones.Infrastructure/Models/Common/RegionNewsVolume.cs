using System.Runtime.Serialization;
using System.Collections.Generic;

namespace DowJones.Models.Common
{
    [DataContract(Name = "regionNewsVolumeSet", Namespace = "")]
    public class RegionNewsVolumeResult
    {
        [DataMember(Name = "regionNewsVolume", EmitDefaultValue = false)]
        public IList<NewsEntityNewsVolumeVariation> RegionNewsVolume { get; set; }
    }
}