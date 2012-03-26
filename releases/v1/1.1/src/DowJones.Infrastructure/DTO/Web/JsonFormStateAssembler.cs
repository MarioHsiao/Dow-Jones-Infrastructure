using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.DTO.Web.JSON;

namespace DowJones.Utilities.DTO.Web
{
	public class JsonFormStateAssembler
	{
		public static JSONObject GetJsonObject(object source)
		{
			return (JSONObject) WriteJsonObject(source);
		}

		public static JSONArray GetJsonArray(object source)
		{
			return (JSONArray) WriteJsonObject(source);
		}

		public static object WriteJsonObject(object source)
		{
			if (source == null)
				return null;
			Type sourceType = source.GetType();

			if (sourceType == typeof (bool))
				return (((bool) source)) ? 1 : 0;
		    if (sourceType.IsPrimitive || sourceType == typeof (string) || sourceType == typeof (Decimal))
		        return source;
		    if (sourceType == typeof (XmlDocument))
		        return ((XmlDocument) source).OuterXml;
		    if (sourceType.IsEnum)
		        return (int) Enum.Parse(sourceType, source.ToString(), true);
		    if (sourceType.IsArray)
		        return WriteJsonArray((Array) source);
		    if (sourceType == typeof (DateTime))
		        return ((DateTime) source).Ticks;
		    if (sourceType.IsClass)
		    {
		        JSONObject ht = new JSONObject();
		        FieldInfo[] sourceFields = sourceType.GetFields();
		        for (int i = 0; i < sourceFields.Length; i++)
		        {
		            // Changed from using integers to using the parameter name D.D 3/15/2007
		            ParameterName parameterName = (ParameterName)Attribute.GetCustomAttribute(sourceFields[i], typeof(ParameterName));
		            FieldInfo sourceField = sourceFields[i];
		            object sourceFieldValue = sourceField.GetValue(source);
		            if (sourceFieldValue != null && !string.IsNullOrEmpty(parameterName.Value) )
		                ht.put(parameterName.Value, WriteJsonObject(sourceFieldValue));
		        }
		        return ht;
		    }
		    throw new ArgumentException("Not supported type");
		}

		public static JSONArray WriteJsonArray(Array source)
		{
			if (source == null)
				return null;
			JSONArray list = new JSONArray();
			for (int i = 0; i < source.Length; i++)
				list.put(WriteJsonObject(source.GetValue(i)));
			return list;
		}

		public static object WriteDomainObject(string jsonString, Type type)
		{
            if (type == typeof (string))
				return jsonString;
		    if (type == typeof (short))
		    {
		        return short.Parse(jsonString);
		    }
		    if (type == typeof (ushort))
		    {
		        return ushort.Parse(jsonString);
		    }
		    if (type == typeof (int))
		    {
		        //return int.Parse(jsonString);
		        int i;
		        Int32.TryParse(jsonString, out i);
		        return i;
		    }
		    if (type == typeof (uint))
		    {
		        return uint.Parse(jsonString);
		    }
		    if (type == typeof (long))
		    {
		        return long.Parse(jsonString);
		    }
		    if (type == typeof (ulong))
		    {
		        return ulong.Parse(jsonString);
		    }
		    if (type == typeof (Double))
		    {
		        Double temp;
		        Double.TryParse(jsonString, out temp);
		        return temp;
		    }
		    if (type == typeof (float))
		    {
		        float temp;
		        float.TryParse(jsonString,out temp);
		        return temp;
		    }
		    if (type == typeof (Single))
		    {
		        Single temp;
		        Single.TryParse(jsonString, out temp);
		        return temp;
		    }
		    if (type == typeof (Decimal))
		    {
		        Decimal temp;
		        Decimal.TryParse(jsonString, out temp);
		        return temp;
		    }
		    if (type == typeof (bool))
		    {
		        string sBool = jsonString.Trim().ToLower();
		        return (sBool == "1" || sBool == "on" || sBool == "true");
		    }
		    if (type == typeof (char))
		        return char.Parse(jsonString);
		    if (type.IsEnum)
		        return Enum.Parse(type, jsonString);
		    if (type == typeof (XmlDocument))
		    {
		        XmlDocument doc = new XmlDocument();
		        doc.LoadXml(jsonString);
		        return doc;
		    }
		    if (type.IsArray)
		    {
		        if (ValidateJsonArrayString(jsonString))
		            return WriteDomainObject(new JSONArray(jsonString), type);
		        return !string.IsNullOrEmpty(jsonString) ? WriteDomainObject(new JSONArray(string.Concat("[",jsonString,"]")), type) : null;
		    }
		    if (type == typeof (DateTime))
		        return new DateTime(Convert.ToInt64(jsonString));

		    return ValidateJsonObjectString(jsonString) ? WriteDomainObject(new JSONObject(jsonString), type) : null;
		}
		
		public static object WriteDomainObject(JSONObject source, Type type)
		{
			if (type == typeof (object))
			{
				return null;
			}
			object target = Construct(type);
			IDictionary mapping = GetFieldMapping(type);
			IEnumerator ienum = source.keys();
			while (ienum.MoveNext())
			{
				string key = ienum.Current.ToString();
				string fieldName = GetFieldName(mapping, key);
				FieldInfo fieldInfo = target.GetType().GetField(fieldName);
				if (fieldInfo == null)
					continue;
				Type fieldType = fieldInfo.FieldType;

				object keyValue = source.getValue(key);
				object fieldValue;
				if (fieldType == typeof (bool))
					fieldValue = Convert.ToBoolean(keyValue);
				else if (fieldType == typeof (XmlNode))
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(keyValue.ToString());
					fieldValue = doc;
				}
				else if (fieldType == typeof (DateTime))
					fieldValue = new DateTime(Convert.ToInt64(keyValue));
				else if (fieldType.IsArray && keyValue.GetType() == typeof (JSONArray))
					fieldValue = WriteDomainObject((JSONArray) keyValue, fieldType);
				else if (fieldType.IsEnum)
					fieldValue = Enum.Parse(fieldType, keyValue.ToString(), true);
				else if (fieldType.IsClass && keyValue.GetType() == typeof (JSONObject))
					fieldValue = WriteDomainObject((JSONObject) keyValue, fieldType);
				else
					fieldValue = keyValue;
				fieldInfo.SetValue(target, fieldValue);
			}
			return target;

		}

		public static object WriteDomainObject(JSONArray source, Type fieldType)
		{
			Type fieldElementType = fieldType.GetElementType();
			int length = source.Count;
			Array target = Array.CreateInstance(fieldElementType, length);
			for (int i = 0; i < target.Length; i++)
			{
				object val = source.getValue(i);
				if (val.GetType() == typeof (JSONObject))
					val = WriteDomainObject((JSONObject) val, fieldElementType);
				else if (val.GetType() == typeof (JSONArray))
					val = WriteDomainObject((JSONArray) val, fieldElementType);
				else
					val = WriteDomainObject(val.ToString(), fieldElementType);
				target.SetValue(val, i);
			}
			return target;
		}

		public static object Construct(Type type)
		{
			if (type == null)
				throw new TypeLoadException(type + ". Are you sure you are not missing an assembly reference?");
			ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
			if (constructorInfo == null)
				throw new InvalidOperationException("There is no default constructor for type " + type);
			return constructorInfo.Invoke(null);
		}

		public static bool ValidateJsonObjectString(string s)
		{
			s = s.Trim();
			if ((s.Length > 1) && (s[0] == '{') && (s[s.Length - 1] == '}'))
				return true;
			return false;
		}

		public static bool ValidateJsonArrayString(string s)
		{
			s = s.Trim();
			if ((s.Length > 1) && (s[0] == '[') && (s[s.Length - 1] == ']'))
				return true;
			return false;
		}

		public static IDictionary GetFieldMapping(Type type)
		{
			Hashtable table = new Hashtable(13);
			FieldInfo[] fieldInfo = type.GetFields();
			for (int i = 0; i < fieldInfo.Length; i++)
				table.Add(i, fieldInfo[i].Name);
			return table;
		}

		public static string GetFieldName(IDictionary list, string fieldId)
		{
			IDictionaryEnumerator enumerator = list.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Key.ToString().Equals(fieldId))
					return enumerator.Value.ToString();
			}
			throw new NotImplementedException(fieldId);
		}
	}
}