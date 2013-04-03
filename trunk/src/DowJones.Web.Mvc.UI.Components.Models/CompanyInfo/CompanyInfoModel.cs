using System.Collections.Generic;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.CompanyInfo
{
    public class CompanyInfoModel : ViewComponentModel
    {
        [ClientData]
        public CompanyInfoData Data { get; set; }

        [ClientProperty("enableCompanySnapshotLink")]
        public bool EnableCompanySnapshotLink { get; set; }
    }

    public class CompanyInfoData
    {
        [JsonProperty("companyCode")]
        public string CompanyCode { get; set; }

        [JsonProperty("quoteData")]
        public Quote QuoteData { get; set; }

        [JsonProperty("keyFinancialData")]
        public KeyFinancial KeyFinancialData { get; set; }
    }

    public class Quote
    {
        /// <summary>
        /// Gets or sets TickerSymbol.
        /// </summary>
        [JsonProperty("tickerSymbol")]
        public string TickerSymbol { get; set; }

        /// <summary>
        /// Gets or sets StockExchange.
        /// </summary>
        [JsonProperty("stockExchange")]
        public string StockExchange { get; set; }

        /// <summary>
        /// Gets or sets CurrentValue.
        /// </summary>
        [JsonProperty("currentValue")]
        public string CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets DifferenceValue.
        /// </summary>
        [JsonProperty("differenceValue")]
        public string DifferenceValue { get; set; }
        
        /// <summary>
        /// Gets or sets DifferencePercentage.
        /// </summary>
        [JsonProperty("differencePercentage")]
        public string DifferencePercentage { get; set; }
    }

    public class KeyFinancial
    {
        /// <summary>
        /// Gets or sets ReportCurrency.
        /// </summary>
        [JsonProperty("reportCurrency")]
        public string ReportCurrency { get; set; }

        /// <summary>
        /// Gets or sets Sales.
        /// </summary>
        [JsonProperty("sales")]
        public string Sales { get; set; }

        /// <summary>
        /// Gets or sets SalesGrowth.
        /// </summary>
        [JsonProperty("salesGrowth")]
        public string SalesGrowth { get; set; }

        /// <summary>
        /// Gets or sets Employees.
        /// </summary>
        [JsonProperty("employees")]
        public string Employees { get; set; }
        
        /// <summary>
        /// Gets or sets EmployeesGrowth.
        /// </summary>
        [JsonProperty("employeesGrowth")]
        public string EmployeesGrowth { get; set; }

        /// <summary>
        /// Gets or sets LastReportedAuditor.
        /// </summary>
        [JsonProperty("lastReportedAuditor")]
        public string LastReportedAuditor { get; set; } 

        /// <summary>
        /// Gets or sets MarketCapitalization.
        /// </summary>
        [JsonProperty("marketCapitalization")]
        public string MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets MarketCapitalizationDate.
        /// </summary>
        [JsonProperty("marketCapitalizationDate")]
        public string MarketCapitalizationDate { get; set; }

        /// <summary>
        /// Gets or sets NetIncome.
        /// </summary>
        [JsonProperty("netIncome")]
        public string NetIncome { get; set; }

        /// <summary>
        /// Gets or sets NetProfitMargin.
        /// </summary>
        [JsonProperty("netProfitMargin")]
        public string NetProfitMargin { get; set; }

        /// <summary>
        /// Gets or sets EPS.
        /// </summary>
        [JsonProperty("eps")]
        public string EPS { get; set; }

        /// <summary>
        /// Gets or sets FiscalYearEndDate.
        /// </summary>
        [JsonProperty("fiscalYearEndDate")]
        public string FiscalYearEndDate { get; set; }
        
        /// <summary>
        /// Gets or sets Provider.
        /// </summary>
        [JsonProperty("provider")]
        public List<ProviderInfo> Provider { get; set; }
    }

    public class ProviderInfo
    {
        /// <summary>
        /// Gets or sets ProviderCode.
        /// </summary>
        [JsonProperty("providerCode")]
        public string ProviderCode { get; set; }

        /// <summary>
        /// Gets or sets ProviderName.
        /// </summary>
        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets ProviderTooltip.
        /// </summary>
        [JsonProperty("providerTooltip")]
        public string ProviderTooltip { get; set; }
        
        /// <summary>
        /// Gets or sets ProviderURL.
        /// </summary>
        [JsonProperty("providerURL")]
        public string ProviderURL { get; set; }
    }
}
