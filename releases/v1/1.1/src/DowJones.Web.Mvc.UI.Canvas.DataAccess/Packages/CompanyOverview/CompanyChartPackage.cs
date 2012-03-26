// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyChartPackage.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.Serialization;
using DowJones.Web.Mvc.Models.News;
using DowJones.Web.Mvc.UI.Models.Company;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages.CompanyOverview
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
