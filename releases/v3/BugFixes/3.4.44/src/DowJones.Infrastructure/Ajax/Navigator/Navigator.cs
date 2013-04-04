using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Ajax.Navigator
{
    /// <summary>
    /// TODO: NN - revisit to change name and/or location of this class
    /// TODO: This is a 1-to-1 copy of Factiva.Gateway.Messages.Search.V2_0.Navigator class,
    /// TODO: used in TrendingPortalPackage.
    /// TODO: Needed to apply serialization attributes.
    /// </summary>
    [DataContract(Name = "navigator", Namespace = "")]
    public class Navigator
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "buckets")]
        public List<Bucket> BucketCollection { get; set; }

        [DataMember(Name = "countSpecified")]
        public bool CountSpecified { get; set; }
    }

    [DataContract(Name = "bucket", Namespace = "")]
    public class Bucket
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "hitCount")]
        public int HitCount { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "countSpecified")]
        public bool CountSpecified { get; set; }
    }
}
