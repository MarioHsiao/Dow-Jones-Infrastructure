using System;

namespace DowJones.Mapping.Types
{
    public class GenericTypeMapping
    {
        public Type DeclaringType { get; set; }
        public Type GenericType { get; set; }
        public bool? Preferred { get; set; }

        public override string ToString()
        {
            return string.Format("{0}<{1}>", DeclaringType, GenericType);
        }
    }
}
