using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DowJones.Factiva.Currents.ServiceModels
{
	public class TypeNameResolver
	{
		public static Type ResolveNameToType(IEnumerable<Assembly> assemblies, string typeName)
		{
			return assemblies
					.SelectMany(t => t.GetTypes())
					.FirstOrDefault(t => t.FullName != null
						&& t.FullName.EndsWith(typeName, StringComparison.OrdinalIgnoreCase));
		}
	}
}
