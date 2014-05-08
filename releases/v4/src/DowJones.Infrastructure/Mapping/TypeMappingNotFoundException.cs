using System;
using DowJones.Exceptions;

namespace DowJones.Mapping
{
    public class TypeMappingException : DowJonesUtilitiesException
    {
        public TypeMappingException()
        {
        }

        public TypeMappingException(string message, Exception exception = null)
            : base(message, exception)
        {
        }
    }

    public class TypeMappingNotFoundException : TypeMappingException
    {
        public override string Message
        {
            get
            {
                return (SourceType == null)
                           ? base.Message
                           : "Source Type: " + SourceType.FullName;
            }
        }

        public Type SourceType { get; set; }

        public TypeMappingNotFoundException(Type sourceType)
        {
            SourceType = sourceType;
        }
    }
}