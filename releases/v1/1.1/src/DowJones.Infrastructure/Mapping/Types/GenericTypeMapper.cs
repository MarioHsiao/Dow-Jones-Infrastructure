using System;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Mapping
{
    public class GenericTypeMapper
    {
        private readonly IEnumerable<GenericTypeMapping> _mappings;


        public GenericTypeMapper(IEnumerable<GenericTypeMapping> mappings)
        {
            _mappings = mappings;
        }

        public Type GetGenericTypeByDeclaringType<TDeclaring>()
        {
            return GetGenericTypeByDeclaringType(typeof(TDeclaring));
        }

        public Type GetGenericTypeByDeclaringType(Type declaringType)
        {
            Guard.IsNotNull(declaringType, "declaringType");

            GenericTypeMapping mapping =
                GetMapping(declaringType, x => x.DeclaringType == declaringType);

            if (mapping == null)
                throw new MissingGenericTypeMappingException(declaringType);

            return mapping.DeclaringType;
        }


        public Type GetDeclaringTypeByGenericType<TGeneric>()
        {
            return GetDeclaringTypeByGenericType(typeof(TGeneric));
        }

        public Type GetDeclaringTypeByGenericType(Type genericType)
        {
            Guard.IsNotNull(genericType, "genericType");

            GenericTypeMapping mapping = 
                GetMapping(genericType, x => x.GenericType == genericType);

            if (mapping == null)
                throw new MissingGenericTypeMappingException(genericType);

            return mapping.DeclaringType;
        }

        protected internal virtual GenericTypeMapping GetMapping(Type type, Func<GenericTypeMapping,bool> filter)
        {
            GenericTypeMapping mapping;

            var genericTypeMappings = _mappings.Where(filter);

            if (genericTypeMappings.Count() > 1)
            {
                var preferredMapping = GetPreferredMapping(type, genericTypeMappings);
                
                if(preferredMapping == null)
                    throw new AmbiguousGenericTypeMappingException(type, genericTypeMappings);

                mapping = preferredMapping;
            }
            else
            {
                mapping = genericTypeMappings.Single();
            }

            if (mapping == null)
                throw new MissingGenericTypeMappingException(type);

            return mapping;
        }


        private static GenericTypeMapping GetPreferredMapping(Type dataType, IEnumerable<GenericTypeMapping> genericTypeMappings)
        {
            var preferredMappings = genericTypeMappings.Where(x => x.Preferred.GetValueOrDefault());

            if (preferredMappings.Count() > 1)
                throw new MultiplePreferredGenericTypeMappingsException(dataType, genericTypeMappings);

            return preferredMappings.SingleOrDefault();
        }
    }
}
