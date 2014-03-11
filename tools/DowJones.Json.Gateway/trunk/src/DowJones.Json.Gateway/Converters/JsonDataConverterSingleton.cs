namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDataConverterSingleton
    {
        private static JsonDotNetJsonConverter _instance;

        private JsonDataConverterSingleton() { }

        internal static JsonDotNetJsonConverter Instance
        {
            get { return _instance ?? (_instance = new JsonDotNetJsonConverter()); }
        }
    }
}