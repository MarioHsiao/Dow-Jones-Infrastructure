using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DowJones
{
    public abstract class PerformanceTestFixture
    {
        public const string PerformanceTestCategory = "Performance";

        protected Timings ExecuteWithTiming(int iterations, Action action, int logInterval = 100)
        {
            var actionTimer = new Stopwatch();
            var individualTimes = new List<long>();
            var exceptions = new List<Exception>();

            for (int i = 0; i < iterations; i++)
            {
                actionTimer.Restart();

                try
                {
                    action();
                    
                    long elapsedMilliseconds = actionTimer.ElapsedMilliseconds;
                    
                    // Yes, this is possible... it's a bug: 
                    // http://connect.microsoft.com/VisualStudio/feedback/details/94083/stopwatch-returns-negative-elapsed-time
                    if (elapsedMilliseconds <= 0)
                        throw new ApplicationException("Invalid timing metric");

                    individualTimes.Add(elapsedMilliseconds);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EXCEPTION: " + ex.Message);
                    exceptions.Add(ex);
                }

                if (i > 0 && i % logInterval == 0)
                    Debug.WriteLine("Test run {0}...", i);
            }

            var timings = new Timings(individualTimes, exceptions);
            Debug.WriteLine(timings);
            return timings;
        }


        public class Timings
        {
            public IEnumerable<Exception> Exceptions { get; private set; }

            public double AverageTime
            {
                get
                {
                    return IndividualTimings.Count() == 0 ? 0 : IndividualTimings.Average();
                }
            }

            public long LowestTime
            {
                get
                {
                    return IndividualTimings.Count() == 0 ? 0 : IndividualTimings.Min();
                }
            }

            public long HighestTime
            {
                get
                {
                    return IndividualTimings.Count() == 0 ? 0 : IndividualTimings.Max();
                }
            }

            public long OverallTime
            {
                get
                {
                    return IndividualTimings.Count() == 0 ? 0 : IndividualTimings.Max();
                }
            }

            public IEnumerable<long> IndividualTimings { get; private set; }


            public Timings(IEnumerable<long> individualTimings, IEnumerable<Exception> exceptions)
            {
                Exceptions = exceptions ?? Enumerable.Empty<Exception>();
                IndividualTimings = individualTimings ?? Enumerable.Empty<long>();
            }

            public override string ToString()
            {
                return string.Format("Iterations: {0};  Exceptions: {1};   Overall: {2}ms;   Average: {3}ms;   Lowest: {4}ms;   Highest: {5}ms",
                                     IndividualTimings.Count(), Exceptions.Count(), OverallTime, AverageTime, LowestTime, HighestTime);
            }
        }

    }
}