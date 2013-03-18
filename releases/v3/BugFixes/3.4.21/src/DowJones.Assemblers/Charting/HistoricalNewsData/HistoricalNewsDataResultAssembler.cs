using System;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Infrastructure;
using DowJones.Models.Charting;
using DowJones.Models.Charting.HistoricalNewsData;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Charting.HistoricalNewsData
{
    public class HistoricalNewsDataResultAssembler : IAssembler<HistoricalNewsDataResult, Navigator>
    {
        private readonly DateTimeFormatter dateTimeFormatter;

        public HistoricalNewsDataResultAssembler(DateTimeFormatter formatter)
        {
            dateTimeFormatter = formatter;
        }

        #region Implementation of IAssembler<out HistoricalNewsDataResult,in NavigatorControl>

        public HistoricalNewsDataResult Convert(Navigator source)
        {
            Guard.IsNotNull(source, "source");

            if (source.Count <= 0)
            {
                return null;
            }

            var result = new HistoricalNewsDataResult
            {
                //DataPoints = new List<NewsDataPoint>(),
                Frequency = MapDataPointFrequency(source.Id),
            };

            var maxDate = DateTime.Now.AddHours(12);
            foreach (var bucket in source.BucketCollection)
            {
                var currentDate = new DateTime(int.Parse(bucket.Id.Substring(0, 4)), int.Parse(bucket.Id.Substring(4, 2)), int.Parse(bucket.Id.Substring(6, 2)));
                if (currentDate < maxDate)
                {
                    result.DataPoints.Add(new NewsDataPoint
                    {
                        Date = DateTimeFormatter.ConvertToUtc(currentDate),
                        DateDisplay = dateTimeFormatter.FormatDate(currentDate),
                        DataPoint = new WholeNumber(bucket.HitCount),
                    });
                }
            }

            return result;
        }

        #endregion

        private static DataPointFrequency MapDataPointFrequency(string id)
        {
            switch (id.ToLower())
            {
                default:
                    return DataPointFrequency.Daily;
                case "pw":
                    return DataPointFrequency.Weekly;
                case "pm":
                    return DataPointFrequency.Monthly;
                case "py":
                    return DataPointFrequency.Yearly;
            }
        }
    }
}
