using System;
using System.Globalization;
using System.Text;

namespace DowJones.DTO.Web.JSON
{
	/// <summary>
	///  Public Domain 2002 JSON.org
	///  @author JSON.org
	///  @version 0.1
	///  Ported to C# by Are Bjolseth, teleplan.no
	/// </summary>
	public sealed class JSONUtils
	{
		/// <summary>
		/// Produce a string in double quotes with backslash sequences in all the right places.
		/// </summary>
		/// <param name="s">A String</param>
		/// <returns>A String correctly formatted for insertion in a JSON message.</returns>
		public static string Enquote(string s)
		{
			if (s == null || s.Length == 0)
			{
				return "\"\"";
			}
			char c;
			int i;
			int len = s.Length;
			StringBuilder sb = new StringBuilder(len + 4);
			string t;

			sb.Append('"');
			for (i = 0; i < len; i += 1)
			{
				c = s[i];
				if ((c == '\\') || (c == '"') || (c == '>'))
				{
					sb.Append('\\');
					sb.Append(c);
				}
				else if (c == '\b')
					sb.Append("\\b");
				else if (c == '\t')
					sb.Append("\\t");
				else if (c == '\n')
					sb.Append("\\n");
				else if (c == '\f')
					sb.Append("\\f");
				else if (c == '\r')
					sb.Append("\\r");
				else
				{
					if (c < ' ')
					{
						//t = "000" + Integer.toHexString(c);
						string tmp = new string(c, 1);
						t = "000" + int.Parse(tmp, NumberStyles.HexNumber);
						sb.Append("\\u" + t.Substring(t.Length - 4));
					}
					else
					{
						sb.Append(c);
					}
				}
			}
			sb.Append('"');
			return sb.ToString();
		}

		public static Array StringToEnumArray(Type type, string names)
		{
			return StringToEnumArray(type, names, ConvertEnumAs.String);
		}

		public static Array StringToEnumArray(Type type, string names, ConvertEnumAs convertAs)
		{
			if (names == null || names.Length < 1)
			{
				return null;
			}

			string[] namesAry = names.Split(',');
			object[] objAry = new object[namesAry.Length];
			Array types = Array.CreateInstance(type, namesAry.Length);

			for (int i = 0; i < namesAry.Length; i++)
			{
				objAry[i] = ((convertAs == ConvertEnumAs.Integer) ? Enum.ToObject(type, Int32.Parse(namesAry[i])) : Enum.Parse(type, namesAry[i], false));
			}

			objAry.CopyTo(types, 0);

			return types;
		}

		public static string EnumArrayToString(Type type, Array o)
		{
			return EnumArrayToString(type, o, ConvertEnumAs.Integer);
		}

		public static string EnumArrayToString(Type type, Array o, ConvertEnumAs convertAs)
		{
			if (o == null || o.Length < 1)
			{
				return "";
			}

			Array types = Array.CreateInstance(type, o.Length);
			o.CopyTo(types, 0);

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < o.Length; i++)
			{
				sb.Append((convertAs == ConvertEnumAs.Integer) ? ((Enum) types.GetValue(i)).ToString("d") : Enum.GetName(type, types.GetValue(i)));
				sb.Append(',');
			}

			return sb.ToString().TrimEnd(',');
		}

		public static string FromBase64String(string base64)
		{
			UTF8Encoding enc = new UTF8Encoding();
			byte[] bytes = Convert.FromBase64String(base64);
			return enc.GetString(bytes);
		}

		public static string ToBase64String(string input)
		{
			UTF8Encoding enc = new UTF8Encoding();
			return Convert.ToBase64String(enc.GetBytes(input));
		}
	}

	public enum ConvertEnumAs
	{
		Integer = 0,
		String
	}
}