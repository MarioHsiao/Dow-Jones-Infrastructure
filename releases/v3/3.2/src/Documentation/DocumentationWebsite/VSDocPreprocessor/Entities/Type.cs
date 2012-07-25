using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VSDocPreprocessor.Entities;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "type")]
    public class Type : DocumentEntity
    {
        [DataMember(Name = "assembly")]
        public string Assembly { get; set; }

        [DataMember(Name = "constructors")]
        public IEnumerable<Constructor> Constructors
        {
            get { return Children.OfType<Constructor>(); }
        }

        [DataMember(Name = "events")]
        public IEnumerable<Event> Events
        {
            get { return Children.OfType<Event>(); }
        }

        [DataMember(Name = "methods")]
        public IEnumerable<Method> Methods
        {
            get { return Children.OfType<Method>(); }
        }

        public override string Name
        {
            get { return base.Name; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    base.Name = value;
                    return;
                }

                var typeName = new TypeName(value);
                FullName = typeName.FullName;
                Namespace = typeName.Namespace;
                base.Name = typeName.Name;
            }
        }

        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        [DataMember(Name = "namespace")]
        public string Namespace { get; set; }

        [DataMember(Name = "properties")]
        public IEnumerable<Property> Properties
        {
            get { return Children.OfType<Property>(); }
        }

        public Type(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }
    }
}