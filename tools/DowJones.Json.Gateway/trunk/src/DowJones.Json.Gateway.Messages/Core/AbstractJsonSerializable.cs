using System;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public abstract class AbstractJsonSerializable:  IJsonSerializable
    {
        public string ToJson()
        {
            return JsonDataConverterSingleton.Instance.Serialize(this);
        }
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        public ControlData GetClone()
        {
            return (ControlData)MemberwiseClone();
        }
    }
}