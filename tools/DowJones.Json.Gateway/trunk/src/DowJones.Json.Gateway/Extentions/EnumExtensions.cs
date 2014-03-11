using System;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.Json.Gateway.Attributes;

namespace DowJones.Json.Gateway.Extentions
{
    public static class EnumExtensions
    {
        public static T ConvertTo<T>(this object value) where T : struct, IConvertible
        {
            var sourceType = value.GetType();
            if (!sourceType.IsEnum)
            {
                throw new ArgumentException("Source type is not enum");
            }

            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("Destination type is not enum");
            }

            return (T) Enum.Parse(typeof (T), value.ToString());
        }

        public static string GetServicePath<T>(this T sourceClass)
        {
            return ((ServicePathAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(ServicePathAttribute))).Path;
        }

        public static DataMemberAttribute GetDataMember<T>(this T sourceField)
        {
            return Attribute.GetCustomAttributes(typeof(T).GetField(sourceField.ToString()), typeof(DataMemberAttribute)).FirstOrDefault() as DataMemberAttribute;
        }

        public static DataContractAttribute GetDataContract<T>(this T sourceClass)
        {
            return ((DataContractAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(DataContractAttribute)));
        }
    }
}