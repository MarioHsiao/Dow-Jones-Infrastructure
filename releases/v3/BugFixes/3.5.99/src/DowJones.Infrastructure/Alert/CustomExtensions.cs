using System;
using System.Linq;

namespace DowJones.Infrastructure.Alert
{
    public static class CustomExtensions
    {
        /// <summary>
        /// custom extension to get an attribute value of an enum field
        /// </summary>
        /// <typeparam name="TIn">the attribute type</typeparam>
        /// <typeparam name="TOut">the expected return value</typeparam>
        /// <param name="enumeration">the enum value</param>
        /// <param name="expression">the lambda expression</param>
        /// <returns>the expected TOut type</returns>
        public static TOut GetAttributeValue<TIn, TOut>(this Enum enumValue, Func<TIn, TOut> expression)
        where TIn : Attribute
        {
            var memInfo = enumValue.GetType().GetMember(enumValue.ToString())[0];
            TIn attribute = memInfo.GetCustomAttributes(typeof(TIn), false).Cast<TIn>().SingleOrDefault();

            if (attribute == null)
                return default(TOut);

            return expression(attribute);
        }
    }
}
