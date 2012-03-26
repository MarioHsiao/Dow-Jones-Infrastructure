using System;
using System.Collections.Generic;

namespace DowJones.Mapping
{
    public class MultiplePreferredGenericTypeMappingsException : GenericTypeMappingException
    {
        private readonly IEnumerable<GenericTypeMapping> _mappings;

        public override string Message
        {
            get
            {
                return string.Format(
                    "Multiple preferred mappings found for type {0}: {1}",
                    DataType.Name, string.Join(", ", _mappings));
            }
        }

        public MultiplePreferredGenericTypeMappingsException(Type dataType, IEnumerable<GenericTypeMapping> mappings)
            : base(dataType)
        {
            _mappings = mappings;
        }
    }
}