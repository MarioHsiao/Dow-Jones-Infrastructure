using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
using System.Collections;

namespace DowJones.Managers.SocialMedia.Serializers
{
    /// <summary>
    /// The JSON convention resolver.
    /// </summary>
    internal class JsonConventionResolver : DefaultContractResolver
    {
        #region Methods

        /// <summary>
        /// The create properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="memberSerialization">The member serialization.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract"/>.
        /// </returns>
        /// ///
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            foreach (var property in properties)
            {
                property.PropertyName = PascalCaseToElement(property.PropertyName);
            }

            var result = properties;
            return result;
        }

        /// <summary>
        /// The Pascal case to element.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// The string representing the Pascal case to element.
        /// </returns>
        private static string PascalCaseToElement(string input)
        {
            if (input.Length > 0 && char.IsLower(input[0]))
            {
                return input;
            }

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var result = new StringBuilder();
            result.Append(char.ToLowerInvariant(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsLower(input[i]))
                {
                    result.Append(input[i]);
                }
                else
                {
                    result.Append("_");
                    result.Append(char.ToLowerInvariant(input[i]));
                }
            }

            return result.ToString();
        }

        #endregion

        /// <summary>
        /// The to string comparer.
        /// </summary>
        public class ToStringComparer : IComparer
        {
            #region Implemented Interfaces

            #region IComparer

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns>
            /// The int value.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception>
            public int Compare(object x, object y)
            {
                return x.ToString().CompareTo(y.ToString());
            }

            #endregion

            #endregion
        }
    }
}
