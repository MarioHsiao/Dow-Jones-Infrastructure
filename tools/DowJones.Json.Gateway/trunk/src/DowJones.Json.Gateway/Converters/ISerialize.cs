namespace DowJones.Json.Gateway.Converters
{
    public interface ISerialize
    {
        string Serialize(object obj, Formatting formatting);

        string Serialize<T>(T obj);
    }
}