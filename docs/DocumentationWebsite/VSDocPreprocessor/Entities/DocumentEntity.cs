using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace VSDocPreprocessor
{
    [DebuggerDisplay("{Name}")]
    [DataContract(Namespace="")]
    [KnownType("GetKnownTypes")]
    public abstract class DocumentEntity
    {
        [DataMember(Name = "example", EmitDefaultValue = false)]
        public virtual string Example { get; set; }

        [DataMember(Name = "name", EmitDefaultValue = false)]
        public virtual string Name { get; set; }

        [IgnoreDataMember]
        public virtual DocumentEntity Parent { get; protected set; }

        [DataMember(Name = "remarks", EmitDefaultValue = false)]
        public virtual string Remarks { get; set; }

        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public virtual string Summary { get; set; }

        [IgnoreDataMember]
        public IEnumerable<DocumentEntity> Children { get; private set; }

        protected DocumentEntity(IEnumerable<DocumentEntity> children)
        {
            Children = new List<DocumentEntity>((children ?? Enumerable.Empty<DocumentEntity>()).Where(x => x != null));

            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        public static IEnumerable<System.Type> GetKnownTypes()
        {
            var documentEntityType = typeof (DocumentEntity);
            return documentEntityType.Assembly.GetExportedTypes().Where(documentEntityType.IsAssignableFrom);
        }
    }
}
