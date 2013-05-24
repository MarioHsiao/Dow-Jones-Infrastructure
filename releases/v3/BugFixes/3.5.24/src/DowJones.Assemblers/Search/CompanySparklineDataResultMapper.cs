// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanySparklineDataResultMapper.cs" company="">
//   
// </copyright>
// <summary>
//   The company sparkline data result mapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DowJones.Ajax.CompanySparkline;
using DowJones.Formatters;
using DowJones.Managers.Sparkline;
using DowJones.Mapping;
using Factiva.Gateway.Messages.MarketData.V1_0;

namespace DowJones.Assemblers.Search
{
    /// <summary>
    /// The company sparkline data result mapper.
    /// </summary>
    public class CompanySparklineDataResultMapper : TypeMapper<SparklineDataSet, CompanySparklineDataResult>,
                                                    ITypeMapper<IEnumerable<SparklineDataSet>, IEnumerable<CompanySparklineDataResult>>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public override CompanySparklineDataResult Map(SparklineDataSet source)
        {
            var data = new CompanySparklineDataResult
                           {
                               Name = source.Company.Name.Value, 
                               Code = source.Company.PrimaryDowJonesTicker, 
                           };
            if (source.HistoricalDataResult != null &&
                source.HistoricalDataResult.dataPoints != null &&
                source.HistoricalDataResult.dataPoints.Count() > 0)
            {
                data.ClosePrices = MapDatapont(source.HistoricalDataResult.dataPoints);
            }

            return data;
        }

        public override IEnumerable<object> Map( IEnumerable<object> sources )
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<CompanySparklineDataResult> Map( IEnumerable<SparklineDataSet> sources )
        {
            return ( from sparklineDataSet in sources
                     where ( sparklineDataSet.HistoricalDataResult != null &&
                            sparklineDataSet.HistoricalDataResult.dataPoints != null &&
                            sparklineDataSet.HistoricalDataResult.dataPoints.Count() > 0 )
                     select this.Map( sparklineDataSet ) ).ToList();
        }
        
        /// <summary>
        /// The map datapont.
        /// </summary>
        /// <param name="dataPoints">
        /// The data points.
        /// </param>
        /// <returns>
        /// </returns>
        private static ClosePriceCollection MapDatapont(IEnumerable<DataPoints> dataPoints)
        {
            var list = new ClosePriceCollection();
            if (dataPoints != null)
            {
                list.AddRange(from dataPoint in dataPoints
                              where dataPoint.closePriceSpecified
                              select new DoubleNumberStock(dataPoint.closePrice));
            }

            return list;
        }
    }
}