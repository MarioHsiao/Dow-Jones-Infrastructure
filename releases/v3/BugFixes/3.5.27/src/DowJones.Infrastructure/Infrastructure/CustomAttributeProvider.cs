using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DowJones.Infrastructure
{
    public abstract class CustomAttributeProvider : ICustomAttributeProvider
    {
        private static readonly object CustomAttributesByTypeLock = new object();
        private static readonly IDictionary<Type, IEnumerable<Attribute>> CustomAttributesByType = new Dictionary<Type, IEnumerable<Attribute>>();

        private static IEnumerable<Attribute> GetCustomAttributes(Type targetType)
        {
            IEnumerable<Attribute> attributes;

            if (!CustomAttributesByType.TryGetValue(targetType, out attributes))
            {
                lock (CustomAttributesByTypeLock)
                {
                    if (!CustomAttributesByType.TryGetValue(targetType, out attributes))
                    {
                        attributes = targetType
                            .GetCustomAttributes(true)
                            .OfType<Attribute>();

                        CustomAttributesByType[targetType] = attributes;
                    }
                }
            }

            return attributes;
        }


        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return GetCustomAttributes(attributeType).ToArray();
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return GetCustomAttributes(GetType()).ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            return GetCustomAttributes(attributeType).Any();
        }

    }
}
