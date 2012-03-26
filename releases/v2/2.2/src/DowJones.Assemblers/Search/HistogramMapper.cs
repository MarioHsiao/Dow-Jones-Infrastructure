// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigatorToHistogramMapper.cs" company="Dow Jones @ Company">
//      Dow Jones Enterprise Markets. 
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Mapping;
using DowJones.Models.Search;
using DowJones.Preferences;
using DowJones.Search.Core.ISO8601;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Search
{
    /// <summary>
    /// </summary>
    public class HistogramMapper : 
        ITypeMapper<Navigator, Histogram>,
        ITypeMapper<ContentSearchResult, Histogram>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public HistogramMapper(IPreferences preferences)
        {
            dateTimeFormatter = new DateTimeFormatter(preferences);
        }

        public object Map(object source)
        {
            if (source is ContentSearchResult)
                return Map((ContentSearchResult) source);
            
            if (source is Navigator)
                return Map((Navigator)source);

            throw new NotSupportedException();
        }

        public Histogram Map(ContentSearchResult source)
        {
            if (
                source == null ||
                source.TimeNavigatorSet == null ||
                source.TimeNavigatorSet.NavigatorCollection == null
              )
                return null;

            var navigator = source.TimeNavigatorSet.NavigatorCollection.FirstOrDefault();

            var histogram = Map(navigator);

            return histogram;
        }

        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="nav">The nav.</param>
        /// <returns>A Date Histogram object.</returns>
        public Histogram Map(Navigator nav)
        {
            var histogram = new Histogram();

            if (nav == null)
                return histogram;

            var histogramItems = new List<HistogramItem>();

            var min = DateTime.MaxValue;
            var max = DateTime.MinValue;
            var gregorianCalendar = new GregorianCalendar();
            switch (nav.Id)
            {
                case "pd":
                    foreach (var bucket in nav.BucketCollection)
                    {
                        var currentDate = new DateTime(
                            Convert.ToInt32(bucket.Id.Substring(0, 4)), 
                            Convert.ToInt32(bucket.Id.Substring(4, 2)), 
                            Convert.ToInt32(bucket.Id.Substring(6, 2)));

                        max = (max < currentDate) ? currentDate : max;
                        min = (min > currentDate) ? currentDate : min;
                        histogramItems.Add(new HistogramItem
                                               {
                                                   HitCount = new WholeNumber(bucket.HitCount), 
                                                   CurrentDate = currentDate,
                                                   CurrentDateText = this.dateTimeFormatter.FormatStandardDate(currentDate),
                                                   IsoStartDate = currentDate.ToString("yyyyMMdd"),
                                                   IsoEndDate = currentDate.ToString("yyyyMMdd"),
                                               });
                    }

                    histogram.StartDate = min;
                    histogram.EndDate = max;
                    histogram.Distribution = Distribution.Daily;
                    histogram.StartDateText = this.dateTimeFormatter.FormatStandardDate(min);
                    histogram.EndDateText = this.dateTimeFormatter.FormatStandardDate(max);
                    histogram.DistributionText = string.Concat(ResourceTextManager.Instance.GetString("distribution"), ": ", ResourceTextManager.Instance.GetString("daily"));
                    break;
                case "pw":
                    foreach (var bucket in nav.BucketCollection)
                    {
                        var year = Convert.ToInt32(bucket.Id.Substring(0, 4));
                        var week = Convert.ToInt32(bucket.Id.Substring(4));
                        var startDate = Iso8601Date.GetIso8601Week(year, week);
                        var endDate = startDate.AddDays(6);
                        max = (max < endDate) ? endDate : max;
                        min = (min > startDate) ? startDate : min;
                        histogramItems.Add(new HistogramItem
                                               {
                                                   HitCount = new WholeNumber(bucket.HitCount), 
                                                   StartDate = startDate, 
                                                   StartDateText = this.dateTimeFormatter.FormatStandardDate(startDate), 
                                                   EndDate = endDate, 
                                                   EndDateText = this.dateTimeFormatter.FormatStandardDate(endDate),
                                                   IsoStartDate = startDate.ToString("yyyyMMdd"),
                                                   IsoEndDate = endDate.ToString("yyyyMMdd"),  
                                               });
                    }

                    histogram.StartDate = min;
                    histogram.EndDate = max;
                    histogram.Distribution = Distribution.Weekly;
                    histogram.StartDateText = this.dateTimeFormatter.FormatStandardDate(min);
                    histogram.EndDateText = this.dateTimeFormatter.FormatStandardDate(max);
                    histogram.DistributionText = string.Concat(ResourceTextManager.Instance.GetString("distribution"), ": ", ResourceTextManager.Instance.GetString("weekly"));
                    break;
                case "pm":
                    foreach (var bucket in nav.BucketCollection)
                    {
                        var year = Convert.ToInt32(bucket.Id.Substring(0, 4));
                        var month = Convert.ToInt32(bucket.Id.Substring(4));
                        var startDate = new DateTime(year, month, 1);
                        max = (max < startDate) ? startDate : max;
                        min = (min > startDate) ? startDate : min;
                        var endDate = startDate.AddDays(gregorianCalendar.GetDaysInMonth(startDate.Year, startDate.Month) - 1);
                        histogramItems.Add(new HistogramItem
                                               {
                                                   HitCount = new WholeNumber(bucket.HitCount), 
                                                   StartDate = startDate, 
                                                   StartDateText = this.dateTimeFormatter.FormatStandardDate(startDate), 
                                                   EndDate = endDate,
                                                   EndDateText = this.dateTimeFormatter.FormatStandardDate(endDate),
                                                   IsoStartDate = startDate.ToString("yyyyMMdd"),
                                                   IsoEndDate = endDate.ToString("yyyyMMdd"),
                                               });
                    }

                    max = max.AddDays(gregorianCalendar.GetDaysInMonth(max.Year, max.Month) - 1);
                    histogram.StartDate = min;
                    histogram.EndDate = max;
                    histogram.Distribution = Distribution.Monthy;
                    histogram.StartDateText = this.dateTimeFormatter.FormatStandardDate(min);
                    histogram.EndDateText = this.dateTimeFormatter.FormatStandardDate(max);
                    histogram.DistributionText = string.Concat(ResourceTextManager.Instance.GetString("distribution"), ": ", ResourceTextManager.Instance.GetString("monthly"));
                    break;
                case "py":
                    foreach (var bucket in nav.BucketCollection)
                    {
                        var year = Convert.ToInt32(bucket.Id.Substring(0, 4));
                        var startDate = new DateTime(year, 1, 1);
                        max = (max < startDate) ? startDate : max;
                        min = (min > startDate) ? startDate : min;
                        var endDate = startDate.AddDays(gregorianCalendar.GetDaysInYear(startDate.Year) - 1);
                        histogramItems.Add(new HistogramItem
                                               {
                                                   HitCount = new WholeNumber(bucket.HitCount), 
                                                   StartDate = startDate, 
                                                   StartDateText = this.dateTimeFormatter.FormatStandardDate(startDate), 
                                                   EndDate = endDate, 
                                                   EndDateText = this.dateTimeFormatter.FormatStandardDate(endDate),   
                                                   IsoStartDate = startDate.ToString("yyyyMMdd"),
                                                   IsoEndDate = endDate.ToString("yyyyMMdd"),
                                               });
                    }

                    max = max.AddDays(gregorianCalendar.GetDaysInYear(max.Year) - 1);
                    histogram.StartDate = min;
                    histogram.EndDate = max;
                    histogram.Distribution = Distribution.Yearly;
                    histogram.StartDateText = this.dateTimeFormatter.FormatStandardDate(min);
                    histogram.EndDateText = this.dateTimeFormatter.FormatStandardDate(max);
                    histogram.DistributionText = string.Concat(ResourceTextManager.Instance.GetString("distribution"), ": ", ResourceTextManager.Instance.GetString("yearly"));
                    break;
            }

            histogram.Items = histogramItems;
            return histogram;
        }
    }
}