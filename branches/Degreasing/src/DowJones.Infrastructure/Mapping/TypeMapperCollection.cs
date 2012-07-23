using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Mapping
{
    public class TypeMapperCollection : List<TypeMapperDefinition>
    {
        public TypeMapperCollection()
        {
        }

        public TypeMapperCollection(IEnumerable<TypeMapperDefinition> definitions)
        {
            AddRange(definitions);
        }

        public IEnumerable<TypeMapperDefinition> FindBySourceType(Type sourceType)
        {
            return this.Where(x => x.SourceType == sourceType);
        }

        public IEnumerable<TypeMapperDefinition> FindByTargetType(Type targetType)
        {
            return this.Where(x => x.TargetType == targetType);
        }

        public IEnumerable<TypeMapperDefinition> FindBySourceTypeAndTargetType(Type sourceType, Type targetType)
        {
            return this.Where(x => x.SourceType == sourceType && x.TargetType == targetType);
        }
    }
}