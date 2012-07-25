using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "method")]
    public class Method : ParamaterizedDocumentEntity
    {
        public Method(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }
    }
}