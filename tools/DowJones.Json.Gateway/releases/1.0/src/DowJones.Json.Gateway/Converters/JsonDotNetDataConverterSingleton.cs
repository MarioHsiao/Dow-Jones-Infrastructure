namespace DowJones.Json.Gateway.Converters
{
    internal class JsonDotNetDataConverterSingleton
    {
        private static ISerialize _instance;

        private JsonDotNetDataConverterSingleton() { }

        internal static ISerialize Instance
        {
            get { return _instance ?? (_instance = new JsonDotNetJsonConverter()); }
        }
    }
}