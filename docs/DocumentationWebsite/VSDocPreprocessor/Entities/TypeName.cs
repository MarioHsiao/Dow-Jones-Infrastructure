using System.Collections.Generic;

namespace VSDocPreprocessor.Entities
{
    public class TypeName
    {
        private static readonly int SystemNamespace = "System".Length;

        private static readonly IDictionary<string, string> PrimitiveTypes = 
            new Dictionary<string,string> {
                    {"SByte", "sbyte"},
                    {"Byte", "byte"},
                    {"Int16", "short"},
                    {"UInt16", "ushort"},
                    {"Int32", "int"},
                    {"UInt32", "uint"},
                    {"Int64", "long"},
                    {"UInt64", "ulong"},
                    {"Char", "char"},
                    {"Single", "float"},
                    {"Double", "double"},
                    {"Boolean", "bool"},
                    {"Decimal", "decimal"},
                    {"Object", "object"},
                    {"String", "string"},
                };

        public string Namespace { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }

        public TypeName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return;

            if (fullName.StartsWith("T:"))
                fullName = fullName.Substring(2);

            fullName = fullName.Trim();

            FullName = fullName;

            var lastDot = fullName.LastIndexOf('.');

            if(lastDot > 0)
            {
                Namespace = fullName.Substring(0, lastDot);
                Name = fullName.Substring(lastDot + 1);

                string name;
                if(lastDot == SystemNamespace && PrimitiveTypes.TryGetValue(Name, out name))
                {
                    Name = name;
                    Namespace = null;
                }
            }
            else
            {
                Name = fullName;
            }
        }

        public static implicit operator TypeName(string fullName)
        {
            return new TypeName(fullName);
        }
    }
}
