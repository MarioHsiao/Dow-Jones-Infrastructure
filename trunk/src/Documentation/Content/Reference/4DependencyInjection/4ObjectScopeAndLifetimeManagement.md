These frameworks often provide even more assistance to developers by providing additional advanced functionality such as object lifetime/scope management. 
Features like these aid in the implementation of loosely-coupled components by abstracting away concepts and patterns such as Singletons. 
Take the following two examples:

#### Usage of Singleton

    public class StockQuoteAnalysisService
    {
        public void AnalyzeStockQuotes()
        {
            Logger.Current.Debug("Starting to analyze stock quotes...");
                    ...
            }
    }

#### Converted to Dependency Injection (with lifetime management)

    public class StockQuoteAnalysisService
    {
        private readonly ILogger _logger;

        public StockQuoteAnalysisService(ILogger logger)
        {
            _logger = logger;
        }

        public void AnalyzeStockQuotes()
        {
            _logger.Debug("Starting to analyze stock quotes...");
                    ...
            }
    }

Since ILoggers are expensive to create, the initial approach relies on a Singleton Logger instance that manages its own lifetime - initializing itself once and only once. 
The latter approach instead advertises the dependence upon an ILogger in its constructor, removing the tight coupling of this service to the Logger singleton. 
This code may continue to avoid duplicating expensive initialization because the Dependency Injection Framework that fulfills the ILogger dependency can be configured to create only one instance of ILogger, returning the same instance every time, effectively replicating the Singleton pattern while providing an important abstraction, allowing for the ILogger implementation to be changed (e.g. from a Singleton to some other pattern) without affecting the rest of the system.

This is a powerful example of how Dependency Injection and Dependency Injection Frameworks can help and encourage better application architecture. 
For a review and recommendation of the various Dependency Injection Frameworks available in the .NET arena as of the time of this writing (Oct. 2010), see Dependency Injection Frameworks Comparison.