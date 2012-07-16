using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace DowJones.Documentation
{
    [DebuggerDisplay("{Key}")]
    public class Name : IEquatable<Name>
    {
        public string DisplayName { get; private set; }
        
        public string Key { get; private set; }
		public string DisplayKey { get; private set; }

        public string Value { get; private set; }


        public Name(string value)
        {
			Value = Path.GetFileNameWithoutExtension(value ?? string.Empty);
			Key = Value.ToLower();
			DisplayKey = Value.WithoutOrdinal();
			DisplayName = ToDisplayName(DisplayKey);
        }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }

        private static string ToDisplayName(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return value ?? string.Empty;

			var withSpaces = Regex.Replace(value, "([a-z](?=[A-Z0-9])|[A-Z0-9](?=[A-Z0-9][a-z]))", "$1 ").Replace("_", " ");
            var lowerCasePrepositions =
                Regex.Replace(withSpaces, " (A(nd|t)|From|Is|O(f|r)|T(he|o))",
                              m => string.Format("{0}", m.Value.ToLower()));
            return lowerCasePrepositions;
        }


        #region Overrides and Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Name) obj);
        }

        public bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase)
                || string.Equals(DisplayKey, other.DisplayKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(Value, other.DisplayKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(DisplayKey, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return Value;
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

        #endregion
    }

	public static class StringExtensions
	{
		public static string WithoutOrdinal(this string source)
		{
			return string.IsNullOrEmpty(source) ? source : source.TrimStart("0123456789".ToCharArray());
		}
	}
}