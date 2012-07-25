using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "event")]
    public class Event : DocumentEntity
    {
        public Event(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }
    }
}