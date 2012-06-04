using System.IO;
using System.Text.RegularExpressions;

namespace DowJones.Documentation.Website.Models
{
    public class Name
    {
        public string Value { get; private set; }
        public string DisplayName { get; private set; }

        public Name(string value)
        {
            Value = Path.GetFileNameWithoutExtension(value ?? string.Empty);
            DisplayName = ToDisplayName(Value);
        }

        private static string ToDisplayName(string value)
        {
            var withSpaces = Regex.Replace(value, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            var lowerCasePrepositions =
                Regex.Replace(withSpaces, " (And|Or|At|The|Of|To|From) ",
                              m => string.Format(" {0} ", m.Value).ToLower());
            return lowerCasePrepositions;
        }


        public static implicit operator Name(string name)
        {
            return new Name(name);
        }
    }
}