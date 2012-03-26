using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Mapping
{
    public class AmbiguousGenericTypeMappingException : GenericTypeMappingException
    {
        private readonly IEnumerable<GenericTypeMapping> _mappings;

        public override string Message
        {
            get
            {
                return string.Format(
                    "Multiple mappings found for type {0}: one should be set as Preferred.  Mappings: {1}",
                    DataType.Name, string.Join(", ", _mappings));
            }
        }

        public AmbiguousGenericTypeMappingException(Type dataType, IEnumerable<GenericTypeMapping> mappings)
            : base(dataType)
        {
            _mappings = mappings ?? Enumerable.Empty<GenericTypeMapping>();
        }
    }
}