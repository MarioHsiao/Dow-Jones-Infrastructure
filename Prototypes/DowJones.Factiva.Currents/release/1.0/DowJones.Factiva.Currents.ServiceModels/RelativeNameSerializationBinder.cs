using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace DowJones.Factiva.Currents.ServiceModels
{
	public class TypeNameSerializationBinder  : SerializationBinder
	{
		private readonly IEnumerable<Assembly> _searchAssemblies;

		public TypeNameSerializationBinder(IEnumerable<Assembly> assembly)
		{
			_searchAssemblies = assembly;
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			return TypeNameResolver.ResolveNameToType(_searchAssemblies, typeName);
		}
	}
}
