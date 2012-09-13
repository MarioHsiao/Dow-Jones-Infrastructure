using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Attributes;
using DowJones.DependencyInjection;
using DowJones.Token;

namespace DowJones
{
    public static class EnumHelper
    {
        internal static Func<ITokenRegistry> TokenRegistryThunk = ServiceLocator.Resolve<ITokenRegistry>;

        public static IEnumerable<KeyValuePair<TEnum, string>> ToKeyValuePairs<TEnum>(IEnumerable<TEnum> exclusions = null)
            where TEnum : struct 
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new NotSupportedException("Only Enum types are supported");

            var tokenRegistry = TokenRegistryThunk();

            exclusions = exclusions ?? Enumerable.Empty<TEnum>();

            var keyValuePairs =
                from enumValue in Enum.GetValues(enumType).Cast<TEnum>()
                where !exclusions.Contains(enumValue)
                let key = enumValue
                let value = tokenRegistry.Get(enumValue as Enum)
                select new KeyValuePair<TEnum, string>(key, value);

            return keyValuePairs.ToArray();
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairsWithTranslatedValue<TEnum>(IEnumerable<TEnum> exclusions = null)
            where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new NotSupportedException("Only Enum types are supported");

            var tokenRegistry = TokenRegistryThunk();

            exclusions = exclusions ?? Enumerable.Empty<TEnum>();

            var keyValuePairs =
                from enumValue in Enum.GetValues(enumType).Cast<TEnum>()
                where !exclusions.Contains(enumValue)
                let key = enumValue
                let token = ((AssignedToken)Attribute.GetCustomAttribute(enumType.GetField(key.ToString()),typeof(AssignedToken))).Token
                let value = tokenRegistry.Get(token)
                select new KeyValuePair<string, string>(key.ToString(), value);
            return keyValuePairs.ToArray();
        }
    }
}
