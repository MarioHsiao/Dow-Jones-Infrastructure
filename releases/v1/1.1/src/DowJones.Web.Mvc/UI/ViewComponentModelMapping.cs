using System;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentModelMapping
    {
        public GenericTypeMapping GenericTypeMapping { get; private set; }

        public Type ViewComponentType
        {
            get { return GenericTypeMapping.DeclaringType; }
        }

        public Type ViewComponentModelType 
        {
            get { return GenericTypeMapping.GenericType; }
        }


        public ViewComponentModelMapping(Type viewComponentType, Type viewComponentModelType, bool? preferred = null)
            : this(new GenericTypeMapping() { DeclaringType = viewComponentType, GenericType = viewComponentModelType, Preferred = preferred})
        {

        }

        public ViewComponentModelMapping(GenericTypeMapping genericTypeMapping)
        {
            Guard.IsNotNull(genericTypeMapping, "genericTypeMapping");

            GenericTypeMapping = genericTypeMapping;
        }
    }
}