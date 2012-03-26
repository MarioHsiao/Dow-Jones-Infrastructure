// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDateTimeConverter.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   A converter for parsing multiple Twitter API date formats.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Newtonsoft.Json;

namespace DowJones.Converters
{
    public class GeneralJsonEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override void WriteJson(JsonWriter writer, object
        value, JsonSerializer serializer)
        {
            writer.WriteValue(((int)value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type
        objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType.IsEnum)
            {
                int intValue = 0;
                if (int.TryParse(reader.Value.ToString(), out intValue))
                {
                    return Enum.ToObject(objectType, intValue);
                }
                return Enum.Parse(objectType, reader.Value.ToString(), true);
            }
            else
            {
                return existingValue;
            }
        }
    }
}