using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DowJones.Extensions
{
    public static class CustomAttributeProviderExtensions
    {
        public static string GetDisplayName(this ICustomAttributeProvider member)
        {
            Guard.IsNotNull(member, "member");

            return member.GetAttributesOf<DisplayNameAttribute>()
                         .Select(attribute => attribute.DisplayName)
                         .LastOrDefault();
        }

        public static string GetDescription(this ICustomAttributeProvider member)
        {
            Guard.IsNotNull(member, "member");

            return member.GetAttributesOf<DescriptionAttribute>()
                         .Select(attribute => attribute.Description)
                         .LastOrDefault();
        }

        public static string GetFormat(this ICustomAttributeProvider member)
        {
            Guard.IsNotNull(member, "member");

            return member.GetAttributesOf<DisplayFormatAttribute>()
                         .Select(attribute => attribute.DataFormatString)
                         .LastOrDefault();
        }

        public static T GetDefaultValue<T>(this ICustomAttributeProvider member)
        {
            Guard.IsNotNull(member, "member");

            var defaultValue = 
                member
                    .GetAttributesOf<DefaultValueAttribute>()
                    .Select(attribute => attribute.Value)
                    .LastOrDefault();
            
            return (T) defaultValue;
        }

        public static IEnumerable<T> GetAttributesOf<T>(this ICustomAttributeProvider member, bool inherit = true)
        {
            Guard.IsNotNull(member, "member");

            IEnumerable<object > customAttributes =
                member.GetCustomAttributes(inherit);

            IEnumerable<T> customAttributesOfRequestedType = 
                customAttributes
                    .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                    .Cast<T>();
            
            return customAttributesOfRequestedType;
        }
    }
}