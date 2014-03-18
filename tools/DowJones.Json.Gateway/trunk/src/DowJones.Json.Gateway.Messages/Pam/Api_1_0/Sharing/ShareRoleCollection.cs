using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Sharing
{
    [CollectionDataContract(Name = "ShareRoleCollection", Namespace = "", ItemName = "shareRole")]
    public class ShareRoleCollection : List<ShareRole>
    {
        public ShareRoleCollection(IEnumerable<ShareRole> collection) : base(collection)
        {
        }
        
        public ShareRoleCollection()
        {
        }
    }
}