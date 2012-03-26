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

            //1. First match both type
            //2. If not then check see if assignable 

//            List<TypeMapperDefinition> list = new List<TypeMapperDefinition>();
//            for (int index = 0; index < this.Count; index++)
//            {
//                TypeMapperDefinition x = this[index];
//                if (x.SourceType == sourceType)
//                {
//                    if (x.TargetType == targetType)
//                    {
//                        list.Add(x);
//                    }
//                    else if (targetType.IsAssignableFrom(x.TargetType))
//                    {
//                        list.Add(x);
//                    }
//                }
//            })


//            return list;
            return this.Where(x => x.SourceType == sourceType && x.TargetType == targetType);
        }
    }
}