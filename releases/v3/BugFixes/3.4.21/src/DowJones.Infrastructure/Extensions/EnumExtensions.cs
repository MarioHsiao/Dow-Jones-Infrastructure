using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Attributes;

namespace DowJones.Extensions
{
	public static class EnumExtensions
	{
		public static string GetAssignedToken<T>(this T sourceEnum)
		{
			return ((AssignedToken)Attribute.GetCustomAttribute(typeof(T).GetField(sourceEnum.ToString()), typeof(AssignedToken))).Token;
		}
	}
}
