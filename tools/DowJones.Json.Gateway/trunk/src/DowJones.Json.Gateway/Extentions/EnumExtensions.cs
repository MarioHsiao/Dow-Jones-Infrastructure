using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Extentions
{
    public static class EnumExtensions
    {
        public static T ConvertTo<T>(this object value) where T : struct,IConvertible
        {
            var sourceType = value.GetType();
            if (!sourceType.IsEnum)
                throw new ArgumentException("Source type is not enum");
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Destination type is not enum");
            return (T)Enum.Parse(typeof(T), value.ToString());
        }
    }


    public static class ControlDataExtensions
    {
        public static bool IsValid(this IControlData controlData)
        {
            return controlData != null;
        }
    }
}
