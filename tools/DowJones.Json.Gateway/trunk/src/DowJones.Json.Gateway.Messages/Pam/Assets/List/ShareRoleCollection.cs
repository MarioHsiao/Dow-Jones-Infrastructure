using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ShareRoleCollection", Namespace = "", ItemName = "shareRole")]
    public class ShareRoleCollection : List<ShareRole>
    {
    }
}