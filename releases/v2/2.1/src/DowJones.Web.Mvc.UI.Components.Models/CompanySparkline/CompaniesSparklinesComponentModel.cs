// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompaniesSparklinesComponentModel.cs" company="Dow Jones & Company">
//   © 2011 Dow Jones Factiva
// </copyright>
// <summary>
//   The companies sparklines model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Web;
using DowJones.Ajax.CompanySparkline;

namespace DowJones.Web.Mvc.UI.Components.Models.CompanySparkline
{
    /// <summary>
    /// The companies sparklines model.
    /// </summary>
    public class CompaniesSparklinesComponentModel : CompositeComponentModel
    {
        private List<CompanySparklineDataResult> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompaniesSparklinesComponentModel"/> class.
        /// </summary>
        public CompaniesSparklinesComponentModel()
        {
            this.Companies = new List<string>();
            this.data = new List<CompanySparklineDataResult>();
            this.DataServiceUrl = VirtualPathUtility.ToAbsolute("~/search/sparklines");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompaniesSparklinesComponentModel"/> class. 
        /// </summary>
        /// <param name="companies">
        /// The companies.
        /// </param>
        public CompaniesSparklinesComponentModel(IEnumerable<string> companies)
        {
            this.Companies = new List<string>(companies);
            this.Data = new List<CompanySparklineDataResult>();
            this.DataServiceUrl = VirtualPathUtility.ToAbsolute("~/search/sparklines"); 
        }

        /// <summary>
        ///   Gets or sets the color of the baseline series.
        /// </summary>
        /// <value>
        ///   The color of the baseline series.
        /// </value>
        [ClientProperty("baselineSeriesColor")]
        public string BaselineSeriesColor { get; set; }

        /// <summary>
        ///   Gets or sets the color of a increase series.
        /// </summary>
        /// <value>
        ///   The color of the series.
        /// </value>
        [ClientProperty("seriesColorForIncrease")]
        public string SeriesColorForIncrease { get; set; }

        /// <summary>
        ///   Gets or sets the color of a decrease series.
        /// </summary>
        /// <value>
        ///   The color of the series.
        /// </value>
        [ClientProperty("seriesColorForDecrease")]
        public string SeriesColorForDecrease { get; set; }

        /// <summary>
        ///   Gets or sets companies.
        /// </summary>
        [ClientProperty("companies")]
        public IList<string> Companies { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [ClientData]
        public IList<CompanySparklineDataResult> Data
        {
            get
            {
                return this.data;
            }

            set
            {
                if (value.Count <= 0)
                {
                    return;
                }

                this.Companies.Clear();
                foreach (var companySparklineDataResult in value)
                {
                    this.Companies.Add(companySparklineDataResult.Code);
                }

                this.data = new List<CompanySparklineDataResult>(value);
            }
        }
    }
}
