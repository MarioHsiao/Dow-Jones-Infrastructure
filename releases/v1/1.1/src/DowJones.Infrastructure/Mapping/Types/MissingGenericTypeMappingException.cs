using System;

namespace DowJones.Mapping
{
    public class MissingGenericTypeMappingException : GenericTypeMappingException
    {
        public override string Message
        {
            get
            {
                return string.Format("Missing mapping for type {0}", DataType.FullName);
            }
        }

        public MissingGenericTypeMappingException(Type dataType) : base(dataType)
        {
        }
    }
}