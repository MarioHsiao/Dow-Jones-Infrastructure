using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.CompanyInfo
{
    public class CompanyInfoModel : ViewComponentModel
    {
        [ClientData]
        public CompanyInfoData Data { get; set; }

        [ClientProperty("companySnapshotLink")]
        public string CompanySnapshotLink { get; set; }
    }

    public class CompanyInfoData
    {
        [JsonProperty("quoteData")]
        public Quote QuoteData { get; set; }

        [JsonProperty("keyFinancialData")]
        public KeyFinancial KeyFinancialData { get; set; }
    }

    public class Quote
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("tickerSymbol")]
        public string TickerSymbol { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("stockExchange")]
        public string StockExchange { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("currentValue")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("differenceValue")]
        public string DifferenceValue { get; set; }
        
        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("differencePercentage")]
        public string DifferencePercentage { get; set; }
    }

    public class KeyFinancial
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("reportCurrency")]
        public string ReportCurrency { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("sales")]
        public string Sales { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("salesGrowth")]
        public string SalesGrowth { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("employees")]
        public string Employees { get; set; }
        
        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("employeesGrowth")]
        public string EmployeesGrowth { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("lastReportedAuditor")]
        public string LastReportedAuditor { get; set; } 

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("marketCapitalization")]
        public string MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("marketCapitalizationDate")]
        public string MarketCapitalizationDate { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("netIncome")]
        public string NetIncome { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("netProfitMargin")]
        public string NetProfitMargin { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("eps")]
        public string EPS { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        [JsonProperty("fiscalYearEndDate")]
        public string FiscalYearEndDate { get; set; }
    }
}
