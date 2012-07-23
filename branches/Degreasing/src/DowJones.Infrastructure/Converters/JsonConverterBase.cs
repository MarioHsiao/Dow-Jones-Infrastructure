// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonConverterBase.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The twitter converter base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Infrastructure.Converters
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// The twitter converter base.
    /// </summary>
    public abstract class JsonConverterBase : JsonConverter
    {
        #region Public Methods
                                                          
        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullable(Type type)
        {
            return type != null && (!type.IsValueType || IsNullableType(type));
        }

        /// <summary>
        /// Determines whether [is nullable type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is nullable type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullableType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <typeparam name="T">Any matching type</typeparam>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanConvert<T>(Type objectType)
        {
            if (objectType == null)
            {
                return false;
            }
            var t = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
            return typeof(T).IsAssignableFrom(t);
        }

        #endregion
    }
}