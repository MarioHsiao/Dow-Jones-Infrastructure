using System;
using System.Collections.Generic;

namespace DowJones.Extensions
{
    internal class CaseInsensitiveComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return String.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}