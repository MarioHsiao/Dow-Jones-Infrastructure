using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "CreateItemIfNotExistsResponse")]
    public class CreateItemIfNotExistsResponse
    {
        public CreateItemIfNotExistsResponse()
        {
            SyndicationItemResult = new List<SyndicationItemResult>();
        }

        [DataMember(Name = "syndicationItemResult")]
        public List<SyndicationItemResult> SyndicationItemResult { get; set; }
    }
}