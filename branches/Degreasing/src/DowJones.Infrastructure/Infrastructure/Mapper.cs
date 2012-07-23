using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Exceptions;
using DowJones.Infrastructure;
using DowJones.Mapping;
using log4net;

namespace DowJones
{
    public class Mapper : IMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Mapper));

        public static IMapper Instance
        {
            get { return instance = instance ?? new Mapper(); }
            set { instance = value; }
        }
        private volatile static IMapper instance;

        internal TypeMapperCollection TypeMappers
        {
            get { return _typeMappers; }
        }
        private readonly TypeMapperCollection _typeMappers;


        public Mapper()
            : this(new TypeMapperCollection())
        {
        }

        public Mapper(IEnumerable<TypeMapperDefinition> typeMappers)
            : this(new TypeMapperCollection(typeMappers))
        {
        }

        public Mapper(TypeMapperCollection typeMappers)
        {
            _typeMappers = typeMappers;
        }


        public static TTarget Map<TTarget>(object source)
        {
            return Instance.Map<TTarget>(source);
        }

        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            return Instance.Map<TSource, TTarget>(source);
        }

        [Obsolete("Use source.Select(Mapper.Map<TTarget>) instead")]
        public static IEnumerable<TTarget> Map<TTarget>(IEnumerable<object> sources)
        {
            if (sources == null)
                return null;

            return sources.Select(Map<TTarget>).ToArray();
        }

        public static object Map(object source, Type targetType, Type sourceType = null)
        {
            return Instance.Map(source, targetType, sourceType);
        }


        public void Register(TypeMapperDefinition definition)
        {
            Guard.IsNotNull(definition, "definition");

            if (definition.SourceType == typeof(Object))
            {
                Log.InfoFormat("Ignoring mapping from System.Object to {0} - that doesn't make sense!", definition.TargetType);
                return;
            }

            TypeMappers.Add(definition);
        }

        object IMapper.Map(object source, Type targetType, Type sourceType)
        {
            object returnValue;

            if (sourceType == null && source != null)
            {
                sourceType = source.GetType();
            }

            // If we have a registered mapper, execute it
            if (TryRegisteredTypeMapper(source, sourceType, targetType, out returnValue))
                return returnValue;

            Type enumType = targetType;
            var isNullable = targetType.Name == "Nullable`1";

            if (!enumType.IsEnum && isNullable)
                enumType = targetType.GetGenericArguments().First();

            if (enumType.IsEnum)
            {
                if(TryMapToEnum(source, sourceType, enumType, out returnValue))
                    return returnValue;

                return isNullable ? null : Enum.GetValues(enumType).GetValue(0);
            }

            throw new TypeMappingNotFoundException(sourceType);
        }

        private bool TryRegisteredTypeMapper(object source, Type sourceType, Type targetType, out object returnValue)
        {
            var factories = TypeMappers.FindBySourceType(sourceType);

            var hasRegisteredMapperForType = factories.Any();
            if (!hasRegisteredMapperForType)
            {
                // Try to use a mapper for the source's base type (if we have one)
                if (sourceType != null && sourceType.BaseType != typeof(object))
                    return TryRegisteredTypeMapper(source, sourceType.BaseType, targetType, out returnValue);

                returnValue = null;
                return false;
            }

            var mappers = TypeMappers.FindBySourceTypeAndTargetType(sourceType, targetType).ToArray();

            if (!mappers.Any())
            {
                mappers = TypeMappers.FindBySourceType(sourceType).ToArray();

                if (!mappers.Any())
                {
                    returnValue = null;
                    return false;
                }
            }

            if(mappers.Count() > 1)
                throw new DowJonesUtilitiesException(string.Format("Found multiple mappers for {0} => {1} mapping", sourceType, targetType));

            var mapper = mappers.Single().Mapper;

            Log.Debug(string.Format("Found registered type mapper {0} for {1} => {2} mapping", mapper.GetType(), sourceType, targetType));
            
            var result = mapper.Map(source);

            // Yes, there is a possibility that the mapped result
            // is not a T... That's fine - just let the framework throw
            // an invalid cast exception!
            returnValue = result;

            return true;
        }

        private static bool TryMapToEnum(object source, Type sourceType, Type targetType, out object returnValue)
        {
            string sourceString = null;

            if (sourceType != null && sourceType.IsEnum && source != null)
                sourceString = source.ToString();
                
            else if(source is string)
                sourceString = (string) source;

            if (!string.IsNullOrEmpty(sourceString))
            {
                Log.Debug("Trying to map to enum " + targetType);

                try
                {
                    var enumValue = Enum.Parse(targetType, sourceString, true);
                    returnValue = enumValue;
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Debug(string.Format("Error trying to parse value '{0}' to enum {1}", source, targetType), ex);
                }
            }

            returnValue = null;
            return false;
        }
    }
}