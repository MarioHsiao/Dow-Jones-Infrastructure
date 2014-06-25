// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the root data type object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json;

namespace DowJones.Extensions
{
    /// <summary>
    /// Extension methods for the root data type object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="obj">The object to be compared.</param>
        /// <param name="values">The values to compare with the object.</param>
        /// <returns>A boolean value</returns>
        public static bool EqualsAny<T>(this T obj, params T[] values)
        {
            return Array.IndexOf(values, obj) != -1;
        }

        /// <summary>
        /// Determines whether the object is equal to none of the provided values.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="obj">The object to be compared.</param>
        /// <param name="values">The values to compare with the object.</param>
        /// <returns>A boolean value</returns>
        public static bool EqualsNone<T>(this T obj, params T[] values)
        {
            return obj.EqualsAny(values) == false;
        }

        /// <summary>
        /// Converts an object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">A generic type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The target type.</returns>
        public static T ConvertTo<T>(this object value)
        {
            return value.ConvertTo(default(T));
        }

        /// <summary>
        /// Converts an object to the specified target type or returns the default value.
        /// </summary>
        /// <typeparam name="T">A generic type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }

            var targetType = typeof(T);

            var converter = TypeDescriptor.GetConverter(value);
            if (converter.CanConvertTo(targetType))
            {
                return (T)converter.ConvertTo(value, targetType);
            }

            converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(value.GetType()))
            {
                return (T)converter.ConvertFrom(value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Converts an object to the specified target type or returns the default value. Any exceptions are optionally ignored.
        /// </summary>
        /// <typeparam name="T">A generic type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="ignoreException">if set to <c>true</c> ignore any exception.</param>
        /// <returns>The target type</returns>
        public static T ConvertTo<T>(this object value, T defaultValue, bool ignoreException)
        {
            if (!ignoreException)
            {
                return value.ConvertTo<T>();
            }
            try
            {
                return value.ConvertTo<T>();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Determines whether the value can (in theory) be converted to the specified target type.
        /// </summary>
        /// <typeparam name="T">A generic type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if this instance can be convert to the specified target type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanConvertTo<T>(this object value)
        {
            if (value == null)
            {
                return false;
            }

            var targetType = typeof(T);

            var converter = TypeDescriptor.GetConverter(value);
            if (converter.CanConvertTo(targetType))
            {
                return true;
            }

            converter = TypeDescriptor.GetConverter(targetType);
            return converter.CanConvertFrom(value.GetType());
        }

        /// <summary>
        /// Converts the specified value to a different type.
        /// </summary>
        /// <typeparam name="T">Any generic type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>An universal converter supplying additional target conversion methods</returns>
        /// <example><code>
        /// var value = "123";
        /// var numeric = value.Convert().ToInt32();
        /// </code></example>
        public static IConverter<T> Convert<T>(this T value)
        {
            return new Converter<T>(value);
        }

        /// <summary>
        /// Dynamically invokes a method using reflection
        /// </summary>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// <code>
        /// var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// var file = type.CreateInstance(@"c:\autoexec.bat");
        /// if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        ///  var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        ///  Console.WriteLine(reader.ReadToEnd());
        ///  reader.Close();
        /// }
        /// </code>
        /// </example>
        public static object InvokeMethod(this object obj, string methodName, params object[] parameters)
        {
            return InvokeMethod<object>(obj, methodName, parameters);
        }

        /// <summary>
        /// Dynamically invokes a method using reflection and returns its value in a typed manner
        /// </summary>
        /// <typeparam name="T">The expected return data types</typeparam>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">The parameters passed to the method.</param>
        /// <returns>The return value</returns>
        /// <example>
        /// <code>
        /// var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// var file = type.CreateInstance(@"c:\autoexec.bat");
        /// if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        ///  var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        ///  Console.WriteLine(reader.ReadToEnd());
        ///  reader.Close();
        /// }
        /// </code>
        /// </example>
        public static T InvokeMethod<T>(this object obj, string methodName, params object[] parameters)
        {
            var type = obj.GetType();
            var method = type.GetMethod(methodName);

            if (method == null)
            {
                throw new ArgumentException(string.Format("Method '{0}' not found.", methodName), methodName);
            }

            var value = method.Invoke(obj, parameters);
            return value is T ? (T)value : default(T);
        }

        /// <summary>
        /// Dynamically retrieves a property value.
        /// </summary>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// <code>
        /// var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// var file = type.CreateInstance(@"c:\autoexec.bat");
        /// if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        ///  var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        ///  Console.WriteLine(reader.ReadToEnd());
        ///  reader.Close();
        /// }
        /// </code>
        /// </example>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return GetPropertyValue<object>(obj, propertyName, null);
        }

        /// <summary>
        /// Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name="T">The expected return data type</typeparam>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="propertyName">The Name of the property.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// <code>
        /// var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// var file = type.CreateInstance(@"c:\autoexec.bat");
        /// if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        ///  var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        ///  Console.WriteLine(reader.ReadToEnd());
        ///  reader.Close();
        /// }
        /// </code>
        /// </example>
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return GetPropertyValue(obj, propertyName, default(T));
        }

        /// <summary>
        /// Dynamically retrieves a property value.
        /// </summary>
        /// <typeparam name="T">The expected return data type</typeparam>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="propertyName">The Name of the property.</param>
        /// <param name="defaultValue">The default value to return.</param>
        /// <returns>The property value.</returns>
        /// <example>
        /// <code>
        /// var type = Type.GetType("System.IO.FileInfo, mscorlib");
        /// var file = type.CreateInstance(@"c:\autoexec.bat");
        /// if(file.GetPropertyValue&lt;bool&gt;("Exists")) {
        ///  var reader = file.InvokeMethod&lt;StreamReader&gt;("OpenText");
        ///  Console.WriteLine(reader.ReadToEnd());
        ///  reader.Close();
        /// }
        /// </code>
        /// </example>
        public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);
            }

            var value = property.GetValue(obj, null);
            return value is T ? (T)value : defaultValue;
        }

        /// <summary>
        /// Dynamically sets a property value.
        /// </summary>
        /// <param name="obj">The object to perform on.</param>
        /// <param name="propertyName">The Name of the property.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException(string.Format("Property '{0}' not found.", propertyName), propertyName);
            }

            property.SetValue(obj, value, null);
        }

        /// <summary>
        /// Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name="T">The attribute type tp look for.</typeparam>
        /// <param name="obj">The object to look on.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this object obj) where T : Attribute
        {
            return GetAttribute<T>(obj, true);
        }

        /// <summary>
        /// Gets the first matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name="T">The attribute type tp look for.</typeparam>
        /// <param name="obj">The object to look on.</param>
        /// <param name="includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attribute</returns>
        public static T GetAttribute<T>(this object obj, bool includeInherited) where T : Attribute
        {
            var type = obj as Type ?? obj.GetType();
            var attributes = type.GetCustomAttributes(typeof(T), includeInherited);
            if (attributes.Length > 0)
            {
                return attributes[0] as T;
            }

            return null;
        }

        /// <summary>
        /// Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name="T">The attribute type tp look for.</typeparam>
        /// <param name="obj">The object to look on.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj) where T : Attribute
        {
            return GetAttributes<T>(obj, true);
        }

        /// <summary>
        /// Gets all matching attribute defined on the data type.
        /// </summary>
        /// <typeparam name="T">The attribute type tp look for.</typeparam>
        /// <param name="obj">The object to look on.</param>
        /// <param name="includeInherited">if set to <c>true</c> includes inherited attributes.</param>
        /// <returns>The found attributes</returns>
        public static IEnumerable<T> GetAttributes<T>(this object obj, bool includeInherited) where T : Attribute
        {
            var type = obj as Type ?? obj.GetType();
            return type.GetCustomAttributes(typeof(T), includeInherited).OfType<T>();
        }

        /// <summary>
        /// Inits the defaults.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void InitDefaults(object obj)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(true).Length <= 0)
                {
                    continue;
                }

                var defaultValueAttribute = prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (defaultValueAttribute.Length == 0)
                {
                    continue;
                }

                var dva = defaultValueAttribute[0] as DefaultValueAttribute;
                if (dva == null)
                {
                    continue;
                }

                if (prop.CanWrite)
                {
                    prop.SetValue(obj, dva.Value, null);
                }
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="str">The string value.</param>
        /// <param name="type">The type value.</param>
        /// <returns>An object.</returns>
        public static object GetObject(string str, Type type)
        {
            if (type == typeof(string))
            {
                return str;
            }

            if (type == typeof(short))
            {
                return short.Parse(str);
            }

            if (type == typeof(ushort))
            {
                return ushort.Parse(str);
            }

            if (type == typeof(int))
            {
                int i;
                Int32.TryParse(str, out i);
                return i;
            }

            if (type == typeof(uint))
            {
                return uint.Parse(str);
            }

            if (type == typeof(long))
            {
                return long.Parse(str);
            }

            if (type == typeof(ulong))
            {
                return ulong.Parse(str);
            }

            if (type == typeof(double))
            {
                double temp;
                double.TryParse(str, out temp);
                return temp;
            }

            if (type == typeof(float))
            {
                float temp;
                float.TryParse(str, out temp);
                return temp;
            }

            if (type == typeof(float))
            {
                float temp;
                float.TryParse(str, out temp);
                return temp;
            }

            if (type == typeof(decimal))
            {
                decimal temp;
                decimal.TryParse(str, out temp);
                return temp;
            }

            if (type == typeof(bool))
            {
                var strBool = str.Trim().ToLower();
                return strBool == "1" || strBool == "on" || strBool == "true";
            }

            if (type == typeof(char))
            {
                return char.Parse(str);
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, str);
            }

            if (type == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                return doc;
            }

            if (type.IsArray)
            {
                return new NotSupportedException("Type IsArray and is not supported currently");
            }

            if (type == typeof(DateTime))
            {
                return new DateTime(System.Convert.ToInt64(str));
            }

            return null;
        }

        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
        };

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
            {
                var obj = propertyDescriptor.GetValue(anonymousObject);
                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }

        /// <summary>
        /// Serializes object to json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            var r = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, jsonSerializerSettings);
            if (r == "[]" || r == "{}")
            {
                return String.Empty;
            }
            return r;
        }

        public static bool IsNumeric(this object value)
        {
            if(value == null)
                return false;

            var type = value.GetType();

            return type.IsNumericType();
        }
    }
}