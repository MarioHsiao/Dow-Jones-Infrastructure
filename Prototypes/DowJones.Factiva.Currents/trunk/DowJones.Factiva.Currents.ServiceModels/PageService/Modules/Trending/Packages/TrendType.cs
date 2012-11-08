using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Modules.Trending.Packages
{
    [DataContract(Name = "trendType", Namespace = "")]
    public enum TrendType
    {
        [EnumMember]
        TopEntities,

        [EnumMember]
        TrendingUp,

        [EnumMember]
        TrendingDown
    }
}