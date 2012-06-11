using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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

            var types = typesMatch.Groups["Types"].Value.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
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
                parameter.Type = type;
                types[i] = string.Format("{0} {1}", type, parameter.Name);
            }

            base.Name = string.Format("{0}({1})", typesMatch.Groups["Signature"].Value, string.Join(", ", types));
        }
    }
}