using System;
using DowJones.Exceptions;
using DowJones.Mapping.Types;

namespace DowJones.Mapping
{
    public class TypeMapperDefinition
    {
        public Type SourceType { get; set; }

        public Type TargetType { get; set; }

        public Func<ITypeMapper> Factory { get; set; }

        public ITypeMapper Mapper
        {
            get
            {
                if(Factory == null)
                    throw new DowJonesUtilitiesException(string.Format("Type Mapping Factory not defined for {0} => {1} mapping", SourceType, TargetType));

                return Factory();
            }
        }


        public TypeMapperDefinition()
        {
        }

        public TypeMapperDefinition(ITypeMapper mapper)
        {
            Factory = () => mapper;
        }

        public TypeMapperDefinition(GenericTypeMapping typeMapping)
        {
            SourceType = typeMapping.GenericType;
            TargetType = typeMapping.DeclaringType;
        }


        public override string ToString()
        {
            return string.Format("{0} => {1}", SourceType, TargetType);
        }

        public static TypeMapperDefinition Create<TSource,TTarget>(ITypeMapper<TSource,TTarget> mapper)
        {
            return new TypeMapperDefinition(mapper)
                       {
                           SourceType = typeof(TSource), 
                           TargetType = typeof(TTarget),
                       };
        }
    }
}