﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace DowJones.Web.Mvc.UI.Components.AuthorList
{
	public static class EnumDescription
	{
		public static string StringValueOf(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Length > 0)
			{
				return attributes[0].Description;
			}
			else
			{
				return value.ToString();
			}
		}
	}
}