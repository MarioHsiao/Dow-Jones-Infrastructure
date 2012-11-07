// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyChartPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Models.Charting.HistoricalStockData;
using DowJones.Models.Charting.HistoricalNewsData;
//using DowJones.Pages.Company;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Company.Packages
{
    [DataContract(Name = "companyChartPackage", Namespace = "")]
    public class CompanyChartPackage : AbstractCompanyPackage
    {
        [DataMember(Name = "stockDataResult")]
        public HistoricalStockDataResult StockDataResult { get; protected internal set; }

        [DataMember(Name = "newsDataResult")]
        public HistoricalNewsDataResult NewsDataResult { get; protected internal set; }
    }
}
