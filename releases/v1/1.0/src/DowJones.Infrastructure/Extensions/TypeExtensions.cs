// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Extension methods for the reflection meta data type "Type"
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Extension methods for the reflection meta data type "Type"
/// </summary>
public static class TypeExtensions
{
    internal static readonly Type[] PredefinedTypes = 
        {
            typeof(object),
            typeof(bool),
            typeof(char),
            typeof(string),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };

    /// <summary>
    /// Creates and returns an instance of the desired type
    /// </summary>
    /// <param name="type">The type to be instanciated.</param>
    /// <param name="constructorParameters">Optional constructor parameters</param>
    /// <returns>The instanciated object</returns>
    /// <example>
    /// <code>
    /// var type = Type.GetType(".NET full qualified class Type")
    /// var instance = type.CreateInstance();
    /// </code>
    /// </example>
    public static object CreateInstance(this Type type, params object[] constructorParameters)
    {
        return CreateInstance<object>(type, constructorParameters);
    }

    /// <summary>
    /// Creates and returns an instance of the desired type casted to the generic parameter type T
    /// </summary>
    /// <typeparam name="T">The data type the instance is casted to.</typeparam>
    /// <param name="type">The type to be instanciated.</param>
    /// <param name="constructorParameters">Optional constructor parameters</param>
    /// <returns>The instanciated object</returns>
    /// <example>
    /// <code>
    /// var type = Type.GetType(".NET full qualified class Type")
    /// var instance = type.CreateInstance&lt;IDataType&gt;();
    /// </code>
    /// </example>
    public static T CreateInstance<T>(this Type type, params object[] constructorParameters)
    {
        var instance = Activator.CreateInstance(type, constructorParameters);
        return (T) instance;
    }

    internal static bool IsPredefinedType(this Type type)
    {
        return PredefinedTypes.Any(t => t == type);
    }

    internal static string FirstSortableProperty(this Type type)
    {
        var firstSortableProperty = type.GetProperties().Where(property => property.PropertyType.IsPredefinedType()).FirstOrDefault();

        if (firstSortableProperty == null)
        {
            throw new SortPropertyNotFoundException();
        }

        return firstSortableProperty.Name;
    }

    internal static bool IsNullableType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    internal static Type GetNonNullableType(this Type type)
    {
        return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
    }

    internal static string GetTypeName(this Type type)
    {
        var baseType = GetNonNullableType(type);
        var s = baseType.Name;
        if (type != baseType)
        {
            s += '?';
        }

        return s;
    }

    internal static bool IsNumericType(this Type type)
    {
        return GetNumericTypeKind(type) != 0;
    }

    internal static bool IsSignedIntegralType(this Type type)
    {
        return GetNumericTypeKind(type) == 2;
    }

    internal static bool IsUnsignedIntegralType(this Type type)
    {
        return GetNumericTypeKind(type) == 3;
    }

    internal static int GetNumericTypeKind(this Type type)
    {
        if (type == null)
        {
            return 0;
        }

        type = GetNonNullableType(type);

        if (type.IsEnum)
        {
            return 0;
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Char:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return 1;
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return 2;
            case TypeCode.Byte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return 3;
            default:
                return 0;
        }
    }

    internal static PropertyInfo GetIndexerPropertyInfo(this Type type, params Type[] indexerArguments)
    {
        return
            (from p in type.GetProperties()
             where AreArgumentsApplicable(indexerArguments, p.GetIndexParameters())
             select p).FirstOrDefault();
    }

    internal static bool IsEnumType(this Type type)
    {
        return GetNonNullableType(type).IsEnum;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
    internal static bool IsCompatibleWith(this Type source, Type target)
    {
        if (source == target)
        {
            return true;
        }

        if (!target.IsValueType)
        {
            return target.IsAssignableFrom(source);
        }

        var st = source.GetNonNullableType();
        var tt = target.GetNonNullableType();

        if (st != source && tt == target)
        {
            return false;
        }

        var sc = st.IsEnum ? TypeCode.Object : Type.GetTypeCode(st);
        var tc = tt.IsEnum ? TypeCode.Object : Type.GetTypeCode(tt);
        switch (sc)
        {
            case TypeCode.SByte:
                switch (tc)
                {
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }

                break;
            case TypeCode.Byte:
                switch (tc)
                {
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                
                break;
            case TypeCode.Int16:
                switch (tc)
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                
                break;
            case TypeCode.UInt16:
                switch (tc)
                {
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                
                break;
            case TypeCode.Int32:
                switch (tc)
                {
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                
                break;
            case TypeCode.UInt32:
                switch (tc)
                {
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                
                break;
            case TypeCode.Int64:
                switch (tc)
                {
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                break;
            case TypeCode.UInt64:
                switch (tc)
                {
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                }
                break;
            case TypeCode.Single:
                switch (tc)
                {
                    case TypeCode.Single:
                    case TypeCode.Double:
                        return true;
                }
                break;
            default:
                if (st == tt) return true;
                break;
        }
        return false;
    }

    internal static Type FindGenericType(this Type type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return type;
            }

            if (genericType.IsInterface)
            {
                foreach (var found in type.GetInterfaces().Select(intfType => intfType.FindGenericType(genericType)).Where(found => found != null))
                {
                    return found;
                }
            }

            type = type.BaseType;
        }

        return null;
    }

    public static IEnumerable<Type> WithTypeName(this IEnumerable<Type> types, string typeName, StringComparison? comparisionKind = null)
    {
        var typeNameExpression = new Regex(string.Format(@".*\.{0}", typeName), RegexOptions.Compiled);
        
        var typesWithName = types.Where(type => typeNameExpression.IsMatch(type.FullName ?? string.Empty));
        
        return typesWithName.ToArray();
    }

    internal static string GetName(this Type type)
    {
        return type.FullName.Replace(type.Namespace + ".", string.Empty);
    }

    internal static object DefaultValue(this Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    internal static MemberInfo FindPropertyOrField(this Type type, string memberName)
    {
        return type.FindPropertyOrField(memberName, false) ?? type.FindPropertyOrField(memberName, true);
    }

    internal static MemberInfo FindPropertyOrField(this Type type, string memberName, bool staticAccess)
    {
        var flags = BindingFlags.Public | BindingFlags.DeclaredOnly | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
        return (from t in type.SelfAndBaseTypes() select t.FindMembers(MemberTypes.Property | MemberTypes.Field, flags, Type.FilterNameIgnoreCase, memberName) into members where members.Length != 0 select members[0]).FirstOrDefault();
    }
       
    internal static IEnumerable<Type> SelfAndBaseTypes(this Type type)
    {
        if (type.IsInterface)
        {
            var types = new List<Type>();
            AddInterface(types, type);
            return types;
        }

        return SelfAndBaseClasses(type);
    }

    internal static IEnumerable<Type> SelfAndBaseClasses(this Type type)
    {
        while (type != null)
        {
            yield return type;
            type = type.BaseType;
        }
    }

    internal static bool IsDateTime(this Type type)
    {
        return type == typeof(DateTime) || type == typeof(DateTime?);
    }

    internal static string ToJavaScriptType(this Type type)
    {
        if (type == null)
        {
            return "Object";
        }

        if (IsNumericType(type))
        {
            return "Number";
        }

        if (type == typeof(DateTime) || type == typeof(DateTime?))
        {
            return "Date";
        }

        if (type == typeof(string))
        {
            return "String";
        }

        if (type == typeof(bool) || type == typeof(bool?))
        {
            return "Boolean";
        }

        return type.IsEnum ? "Enum" : "Object";
    }

    private static void AddInterface(List<Type> types, Type type)
    {
        if (types.Contains(type))
        {
            return;
        }

        types.Add(type);
        foreach (var t in type.GetInterfaces())
        {
            AddInterface(types, t);
        }
    }

    private static bool AreArgumentsApplicable(IEnumerable<Type> arguments, IEnumerable<ParameterInfo> parameters)
    {
        var argumentList = arguments.ToList();
        var parameterList = parameters.ToList();

        if (argumentList.Count != parameterList.Count)
        {
            return false;
        }

        return !argumentList.Where((t, i) => parameterList[i].ParameterType != t).Any();
    }

    internal class SortPropertyNotFoundException : Exception
    {
    }

}