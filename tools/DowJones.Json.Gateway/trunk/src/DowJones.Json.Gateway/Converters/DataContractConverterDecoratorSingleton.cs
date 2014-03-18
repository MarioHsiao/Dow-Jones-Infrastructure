namespace DowJones.Json.Gateway.Converters
{
    internal class DataContractConverterDecoratorSingleton
    {
        private static DataConverterDecorator _instance;

        private DataContractConverterDecoratorSingleton() { }

        internal static DataConverterDecorator Instance
        {
            get { return _instance ?? (_instance = new DataContractDataConverterDecorator(DataContractConverterSingleton.Instance)); }
        }
    }

    internal class JsonDotNetConverterDecoratorSingleton
    {
        private static DataConverterDecorator _instance;

        private JsonDotNetConverterDecoratorSingleton() { }

        internal static DataConverterDecorator Instance
        {
            get { return _instance ?? (_instance = new JsonDotNetDataConverterDecorator(JsonDotNetDataConverterSingleton.Instance)); }
        }
    }
}