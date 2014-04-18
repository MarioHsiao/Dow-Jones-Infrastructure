namespace DowJones.Json.Gateway.Converters
{
    public interface ISerialize
    {
        string Serialize<T>(T obj, Formatting formatting);

        string Serialize<T>(T obj);

        T Deserialize<T>(string str);
    }
}