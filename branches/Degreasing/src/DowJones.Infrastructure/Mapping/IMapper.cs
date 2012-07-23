using System;
using System.Collections.Generic;

namespace DowJones.Mapping
{
    public static class IMapperExtensions
    {
        public static TTarget Map<TTarget>(this IMapper mapper, object source)
        {
            return (TTarget)MapInternal(mapper, source, typeof(TTarget), null);
        }

        public static TTarget Map<TSource, TTarget>(this IMapper mapper, TSource source)
        {
            return (TTarget)MapInternal(mapper, source, typeof(TTarget), typeof(TSource));
        }

        private static object MapInternal(IMapper mapper, object source, Type targetType, Type sourceType)
        {
            var mapped = mapper.Map(source, targetType, sourceType);

            var mappedInstanceIsNotTargetType =
                mapped != null && !targetType.IsAssignableFrom(mapped.GetType());

            if (mappedInstanceIsNotTargetType)
            {
                const string message = "Source mapped to {0}, which doesn't match target type {1}";
                throw new TypeMappingException(string.Format(message, mapped.GetType(), targetType));
            }

            return mapped;
        }

        public static void Register(this IMapper mapper, IEnumerable<TypeMapperDefinition> mapperDefinitions)
        {
            foreach (var definition in mapperDefinitions)
                mapper.Register(definition);
        }
    }

    public interface IMapper
    {
        object Map(object source, Type targetType, Type sourceType);
        void Register(TypeMapperDefinition definition);
    }
}