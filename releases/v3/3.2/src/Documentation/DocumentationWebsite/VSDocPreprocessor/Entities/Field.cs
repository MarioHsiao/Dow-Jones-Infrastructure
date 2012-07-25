using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "field")]
    public class Field : DocumentEntity
    {
        public Field(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }
    }
}