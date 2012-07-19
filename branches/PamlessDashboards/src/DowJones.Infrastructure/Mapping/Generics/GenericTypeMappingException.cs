using System;

namespace DowJones.Mapping.Types
{
    public abstract class GenericTypeMappingException : Exception
    {
        public Type DataType { get; set; }

        protected GenericTypeMappingException(Type dataType)
        {
            DataType = dataType;
        }
    }
}