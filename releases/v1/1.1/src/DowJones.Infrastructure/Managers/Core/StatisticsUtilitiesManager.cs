using System;
using System.Collections.Generic;

namespace DowJones.Utilities.Managers.Core
{
    public sealed class StatisticsUtilitiesManager
    {
        private static readonly StatisticsUtilitiesManager m_Instance = new StatisticsUtilitiesManager();

        public static StatisticsUtilitiesManager Instance
        {
            get { return m_Instance; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsUtilitiesManager"/> class.
        /// </summary>
        private StatisticsUtilitiesManager()
        {
        }

        public static double Mean(IEnumerable<double> values)
        {
            double sum = 0;
            var count = 0;

            foreach (var d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        public static double StdDev(IEnumerable<double> values, out double mean)
        {
            mean = Mean(values);
            double sumOfDiffSquares = 0;
            var count = 0;

            foreach (var d in values)
            {
                var diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }
            return Math.Sqrt(sumOfDiffSquares / count);
        }

        public static double StdDev(IEnumerable<double> values)
        {
            double mean;
            return StdDev(values, out mean);
        }     
    }
}
