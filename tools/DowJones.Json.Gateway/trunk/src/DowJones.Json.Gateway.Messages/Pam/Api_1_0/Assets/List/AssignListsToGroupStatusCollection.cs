using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.List
{
    [CollectionDataContract(Name = "AssignListsToGroupStatusCollection", Namespace = "", ItemName = "assignListsToGroupStatus")]
    public class AssignListsToGroupStatusCollection : List<AssignListsToGroupStatus>
    {
        
        public AssignListsToGroupStatusCollection()
        {
        }

        public AssignListsToGroupStatusCollection(IEnumerable<AssignListsToGroupStatus> collection) : base(collection)
        {
        }
    }
}