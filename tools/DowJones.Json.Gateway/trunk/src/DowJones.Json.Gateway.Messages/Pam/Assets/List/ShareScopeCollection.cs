using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.List
{
    [CollectionDataContract(Name = "ShareScopeCollection", Namespace = "", ItemName = "shareScope")]
    public class ShareScopeCollection : List<ShareScope>
    {
    }
}