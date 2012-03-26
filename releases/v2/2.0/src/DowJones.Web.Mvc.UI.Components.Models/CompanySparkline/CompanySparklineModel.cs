// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanySparklineModel.cs" company="Dow Jones & Company">
//   
// </copyright>
// <summary>
//   The company sparkline model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Ajax.CompanySparkline;
using DowJones.Extensions;
using DowJones.Formatters;

namespace DowJones.Web.Mvc.UI.Components.Models.CompanySparkline
{
    /// <summary>
    /// The company sparkline model.
    /// </summary>
    public class CompanySparklineModel : ViewComponentModel
    {
        /// <summary>
        /// Gets or sets the color of the baseline series.
        /// </summary>
        /// <value>
        /// The color of the baseline series.
        /// </value>
        [ClientProperty("baselineSeriesColor")]
        public string BaselineSeriesColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of a increase series.
        /// </summary>
        /// <value>
        /// The color of the series.
        /// </value>
        [ClientProperty("seriesColorForIncrease")]
        public string SeriesColorForIncrease
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of a decrease series.
        /// </summary>
        /// <value>
        /// The color of the series.
        /// </value>
        [ClientProperty("seriesColorForDecrease")]
        public string SeriesColorForDecrease
        {
            get;
            set;
        }

        [ClientData]
        public CompanySparklineDataResult CompanySparklineDataResult
        {
            get;
            set;
        }
    }
}