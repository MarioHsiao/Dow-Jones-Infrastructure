namespace DowJones.Json.Gateway.Converters
{
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