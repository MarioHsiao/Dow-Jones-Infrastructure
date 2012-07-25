using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace VSDocPreprocessor
{
    [DataContract(Namespace="", Name = "constructor")]
    public class Constructor : ParamaterizedDocumentEntity
    {
        private static readonly Regex NameRegex =
            new Regex(@"#ctor(?<ctor>.*)");

        public Constructor(IEnumerable<DocumentEntity> children = null)
            : base(children)
        {
        }

        protected override string GetName()
        {
            var formatted = base.GetName();

            if (Parent == null)
                return formatted;

            var parentName = Parent.Name;

            if (string.IsNullOrWhiteSpace(formatted))
                return formatted;

            var match = NameRegex.Match(formatted);
            
            if (match.Success)
            {
                formatted = string.Format("{0}{1}",
                                     parentName,
                                     match.Groups["ctor"]);
            }

            return formatted;
        }
    }
}