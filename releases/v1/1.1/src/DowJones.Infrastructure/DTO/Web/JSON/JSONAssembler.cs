using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DowJones.Utilities.DTO.Web.JSON
{
	/// <summary>
	/// Summary description for JSONAssembler.
	/// </summary>
	public class JSONAssembler
	{
		private JSONAssembler()
		{
		}

		public static object WriteDomainObject(string jsonString, Type type)
		{
			return WriteDomainObject(new JSONObject(jsonString), type);
		}

		public static string WriteJsonString(object domainObject)
		{
			return WriteJsonObject(domainObject).ToString();
		}

		public static string WriteJsonString(IList list)
		{
			StringBuilder sb = new StringBuilder();
			IEnumerator myEnumerator = list.GetEnumerator();
			while (myEnumerator.MoveNext())
			{
				if (myEnumerator.Current != null)
					sb.Append(WriteJsonString(myEnumerator.Current));
			}
			if (sb.Length > 0)
				return sb.ToString();
			return null;
		}

		private static object WriteDomainObject(JSONObject source, Type type)
		{
			object target = Construct(type);
			IEnumerator ienum = source.keys();
			while (ienum.MoveNext())
			{
				string key = ienum.Current.ToString();
				FieldInfo fieldInfo = target.GetType().GetField(key);
				if (fieldInfo == null)
				{
					continue;
				}
				Type fieldType = fieldInfo.FieldType;

				object keyValue = source.getValue(key);
				object fieldValue;
				if (fieldType == typeof (XmlNode))
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(keyValue.ToString());
					fieldValue = doc;
				}
				else if (fieldType == typeof (DateTime))
				{
					fieldValue = new DateTime(Convert.ToInt64(keyValue));
				}
				else if (fieldType.IsArray && keyValue.GetType() == typeof (JSONArray))
				{
					fieldValue = WriteDomainObject((JSONArray) keyValue, fieldType);
				}
				else if (fieldType.IsEnum)
				{
					fieldValue = Enum.Parse(fieldType, keyValue.ToString(), true);
				}
				else if (fieldType.IsClass && keyValue.GetType() == typeof (JSONObject))
				{
					fieldValue = WriteDomainObject((JSONObject) keyValue, fieldType);
				}
				else
				{
					fieldValue = keyValue;
				}
				fieldInfo.SetValue(target, fieldValue);
			}
			return target;
		}

		private static object WriteDomainObject(JSONArray source, Type fieldType)
		{
			Type fieldElementType = fieldType.GetElementType();
			int length = source.Count;
			Array target = Array.CreateInstance(fieldElementType, length);
			for (int i = 0; i < target.Length; i++)
			{
				object val = source.getValue(i);
				if (val.GetType() == typeof (JSONObject))
				{
					val = WriteDomainObject((JSONObject) val, fieldElementType);
				}
				if (val.GetType() == typeof (JSONArray))
				{
					val = WriteDomainObject((JSONArray) val, fieldElementType);
				}
				target.SetValue(val, i);
			}
			return target;
		}

		private static object WriteJsonObject(object source)
		{
			if (source == null)
			{
				return null;
			}
			Type sourceType = source.GetType();
			if (sourceType.IsPrimitive || sourceType == typeof (string) || sourceType == typeof (Single))
			{
				return source;
			}
			else if (sourceType == typeof (XmlDocument))
			{
				return (source as XmlDocument).OuterXml;
			}
			else if (sourceType.IsEnum)
			{
				return source;
			}
			else if (sourceType.IsArray)
			{
				return WriteJsonArray((object[]) source);
			}
			else if (sourceType == typeof (DateTime))
			{
				return ((DateTime) source).Ticks;
			}
			else if (sourceType.IsClass)
			{
				JSONObject ht = new JSONObject();
				FieldInfo[] sourceFields = sourceType.GetFields();
				foreach (FieldInfo sourceField in sourceFields)
				{
					object sourceFieldValue = sourceField.GetValue(source);
					if (sourceFieldValue != null)
					{
						ht.put(sourceField.Name, WriteJsonObject(sourceFieldValue));
					}
				}
				return ht;
			}
			throw new NotImplementedException("Not supported type");
		}

		private static JSONArray WriteJsonArray(object[] source)
		{
			if (source == null)
			{
				return null;
			}
			JSONArray list = new JSONArray();
			for (int i = 0; i < source.Length; i++)
			{
				list.put(WriteJsonObject(source[i]));
			}
			return list;
		}

		private static object Construct(Type type)
		{
			ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
			if (constructorInfo == null)
				throw new InvalidOperationException("There is no default constructor for type " + type);
			return constructorInfo.Invoke(null);
		}
	}
}