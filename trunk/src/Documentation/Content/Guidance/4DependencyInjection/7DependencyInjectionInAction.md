Without Dependency Injection. 
	public class ConsoleRunner
		{
			public static void Main(string[] args)
			{
				var analysisService = new StockQuoteAnalysisService();
				analysisService.AnalyzeStockQuotes();
			}
		}

With Dependency Injection. 
    public class +StockQuoteAnalysisService+
    {
        public void AnalyzeStockQuotes()
        {
            var +StockQuoteDataService+ = new StockQuoteDataService();
            var stockQuotes = StockQuoteDataService.GetStockQuotes();
            // [  Analyze stock quotes... ]
        }
    }

    public class ConsoleRunner
    {
       public static void Main(string[] args)
       {
           // The high-level ConsoleRunner component is now responsible
           // for creating and managing the instance of StockQuoteDataService
           var dataService = new StockQuoteDataService();

           // ConsoleRunner passes the StockQuoteDataService instance
           // via the object's constructor (Constructor Injection)
           var analysisService = new StockQuoteAnalysisService(dataService);
           analysisService.AnalyzeStockQuotes();
       }
    }

    public class StockQuoteAnalysisService
    {
        private readonly IStockQuoteDataService _dataService;

        public StockQuoteAnalysisService(IStockQuoteDataService dataService)
        {
            _dataService = dataService;
        }

        public void AnalyzeStockQuotes()
        {
            // The analysis service uses the instance it was provided
            // rather than creating its own.  Thus, it can focus on
            // the work of analysis and not data retrieval.
            var stockQuotes = _dataService.GetStockQuotes();

            // ...

 Types of Dependency Injection in action

    public class StockQuoteAnalysisService
    {
        private readonly IStockQuoteDataService _dataService;

        // Property injection:  consumers can provide an alternate ILogger
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
        private ILogger _logger = new NullLogger();

        // Constructor injection:  consumers must supply an IStockQuoteDataService
        public StockQuoteAnalysisService(IStockQuoteDataService+ dataService)
        {
            _dataService = dataService;
        }

        // Method injection:  consumers supply their own TextWriter
        public void AnalyzeStockQuotes(TextWriter writer)
        {
            // Service Location: the method requests an instance from the
            //                   Service Locator class
            var auxiliaryService = ServiceLocator.Resolve<IAuxiliaryService>();
        }
    }
