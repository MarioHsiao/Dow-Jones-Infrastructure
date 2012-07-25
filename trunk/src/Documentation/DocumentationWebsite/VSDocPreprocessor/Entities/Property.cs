using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "property")]
    public class Property : DocumentEntity
    {
        public Property(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }
    }
}