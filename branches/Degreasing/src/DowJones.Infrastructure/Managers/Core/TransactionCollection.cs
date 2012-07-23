using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Managers.Core
{
    [CollectionDataContract(Name = "transactions", ItemName = "transaction", Namespace = "")]
    public class TransactionCollection : List<Transaction>
    {
        public TransactionCollection()
        {
        }

        public TransactionCollection(IEnumerable<Transaction> transactions)
            : base(transactions)
        {
        }
    }
}