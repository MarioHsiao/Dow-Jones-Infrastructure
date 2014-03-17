namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDotNetDataConverterSingleton
    {
        private static JsonDotNetJsonConverter _instance;

        private JsonDotNetDataConverterSingleton() { }

        internal static JsonDotNetJsonConverter Instance
        {
            get { return _instance ?? (_instance = new JsonDotNetJsonConverter()); }
        }
    }
}