using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VSDocPreprocessor.Entities;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="")]
    public abstract class ParamaterizedDocumentEntity : DocumentEntity
    {
        private static readonly Regex TypesRegex = 
            new Regex(@"(?<Signature>.*)\((?<Types>[^)]*)\)");

        public override string Name
        {
            get
            {
                return GetName();
            }
            set
            {
                base.Name = value;
                MergeParameterNames();
            }
        }

        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public IEnumerable<Parameter> Parameters
        {
            get
            {
                return Children.OfType<Parameter>();
            }
        }

        protected ParamaterizedDocumentEntity(IEnumerable<DocumentEntity> children)
            : base(children)
        {
        }

        protected virtual string GetName()
        {
            var name = base.Name;

            if (!(name == null || name.EndsWith(")")))
                name += "()";

            return name;
        }

        protected void MergeParameterNames()
        {
            var name = GetName();
            var parameters = Children.OfType<Parameter>().ToArray();

            if (string.IsNullOrEmpty(name))
                return;

            var typesMatch = TypesRegex.Match(name);

            if(!typesMatch.Success)
                return;

            var signature = typesMatch.Groups["Signature"].Value;

            var types = 
                (
                    from type in typesMatch.Groups["Types"].Value.Split(',')
                    where !string.IsNullOrWhiteSpace(type) 
                    let typeName = new TypeName(type)
                    select new TypeName(type)
                ).ToArray();
            
            var typeCount = types.Count();

            if (typeCount != parameters.Count())
            {
                Trace.TraceWarning("Types list and parameter list for {0} have different counts -- skipping", name);
                return;
            }

            // This, of course, assumes that the Parameters are in the same order 
            // as they appear in the method signature.
            for (int i = 0; i < typeCount; i++)
            {
                var type = types[i];
                var parameter = parameters[i];
                parameter.Type = type.Name;
            }

            base.Name = string.Format("{0}({1})", 
                                      signature, 
                                      string.Join(", ", types.Select(x => x.Name)));
        }
    }
}