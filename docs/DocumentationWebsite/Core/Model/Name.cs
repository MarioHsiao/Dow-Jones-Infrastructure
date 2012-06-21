using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DowJones.Documentation
{
    public class Name : IEquatable<Name>
    {
        public string Value { get; private set; }
        public string Prefix { get; private set; }
        public string Key { get; private set; }
        public string DisplayName { get; private set; }

        public Name(string value)
        {
            Value = Path.GetFileNameWithoutExtension(value ?? string.Empty);
            Key = Value.ToLower();

            var split = Value.Split('_');

            if(split.Length == 1)
            {
                Prefix = string.Empty;
                DisplayName = ToDisplayName(split[0]);
            }
            else
            {
                Prefix = split[0];
                DisplayName = ToDisplayName(split[1]);
            }
        }

        private static string ToDisplayName(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return value ?? string.Empty;

            var withSpaces = Regex.Replace(value, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            var lowerCasePrepositions =
                Regex.Replace(withSpaces, " (And|Or|At|The|Of|To|From) ",
                              m => string.Format(" {0} ", m.Value).ToLower());
            return lowerCasePrepositions;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name) obj);
        }

        public bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }


        public static bool operator ==(Name left, Name right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !Equals(left, right);
        }


        public static implicit operator Name(string name)
        {
            return new Name(name);
        }
    }
}