using System.Collections.Generic;
using System.Linq;

namespace DowJones.Search.Navigation
{
    public class Navigator
    {
        public static class Codes
        {
            public const string Author = "AU";
            public const string Company = "CO";
            public const string Executive = "PE";
            public const string Industry = "IN";
            public const string Region = "RE";
            public const string Source = "SC";
            public const string Subject = "NS";
        }

        public string Code { get; set; }

        public IEnumerable<NavigatorItem> Items { get; set; }

        public int ItemCount
        {
            get
            {
                return (Items ?? Enumerable.Empty<NavigatorItem>()).Count();
            }
        }
    }
}