using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [CollectionDataContract(Name = "ShareScopeCollection", Namespace = "", ItemName = "shareScope")]
    public class ShareScopeCollection : List<ShareScope>
    {
        public ShareScopeCollection(IEnumerable<ShareScope> collection) : base(collection)
        {
        }

        public ShareScopeCollection()
        {
        }
    }
}