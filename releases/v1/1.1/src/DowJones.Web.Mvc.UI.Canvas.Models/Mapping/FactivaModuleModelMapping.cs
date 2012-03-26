using System;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMapping
    {
        public GenericTypeMapping GenericTypeMapping { get; private set; }

        public Type ModelType
        {
            get { return GenericTypeMapping.DeclaringType; }
        }

        public Type FactivaModuleType
        {
            get { return GenericTypeMapping.GenericType; }
        }

        public FactivaModuleModelMapping(GenericTypeMapping genericTypeMapping)
        {
            Guard.IsNotNull(genericTypeMapping, "genericTypeMapping");

            GenericTypeMapping = genericTypeMapping;
        }
    }

}
