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


    internal class JsonDataConverterDecoratorSingleton
    {
        private static JsonDataConverterDecorator _instance;

        private JsonDataConverterDecoratorSingleton() { }

        internal static JsonDataConverterDecorator Instance
        {
            get { return _instance ?? (_instance = new JsonDataConverterDecorator(JsonDataConverterSingleton.Instance)); }
        }
    }

}