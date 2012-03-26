using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;

namespace DowJones.Extensions
{
    public static class GenericTypeMappingExtensions
    {
        public static IEnumerable<GenericTypeMapping> ToGenericTypeMappings(this IEnumerable<Type> types)
        {
            return from type in types
                   let genericType = type.GetGenericArguments().FirstOrDefault()
                   where genericType != null
                   select new GenericTypeMapping
                              {
                                  DeclaringType = type,
                                  GenericType = genericType
                              };
        }

        public static IEnumerable<GenericTypeMapping> ToGenericBaseTypeMappings(this IEnumerable<Type> types)
        {
            return from type in types
                   let baseType = type.BaseType
                   where baseType != null
                   let genericType = baseType.GetGenericArguments().FirstOrDefault()
                   where genericType != null
                   select new GenericTypeMapping
                              {
                                  DeclaringType = type,
                                  GenericType = genericType
                              };
        }
    }
}