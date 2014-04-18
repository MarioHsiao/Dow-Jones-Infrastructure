using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Converters
{
    internal class TypeNameSerializationBinder : SerializationBinder
    {
        protected List<string> AssemblyNames { get; private set; }
        private static List<Assembly> LoadedAssemblies { get; set; } 

        public TypeNameSerializationBinder()
        {
            AssemblyNames = new List<string>(new []
                                             {
                                                 "DowJones.Json.Gateway", 
                                                 "DowJones.Json.Gateway.Messages"
                                             });
            LoadedAssemblies = new List<Assembly>();

            foreach (var assembly in AssemblyNames)
            {
                LoadedAssemblies.Add(Assembly.Load(assembly));
            }
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return LoadedAssemblies.Select(a => a.GetTypes()).SelectMany(assemblyTypes => assemblyTypes.Where(t => t.Name == typeName)).FirstOrDefault();
        }
    }
}