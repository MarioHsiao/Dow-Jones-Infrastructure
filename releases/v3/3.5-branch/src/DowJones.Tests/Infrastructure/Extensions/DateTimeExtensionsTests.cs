using System;
using DowJones.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Extensions
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void ToElapsedStringOneSecondAgo()
        {
            var expected = "1 second ago";
            var d = DateTime.Now.AddSeconds(-1);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ToElapsedString20SecondsAgo()
        {
            var expected = "20 seconds ago";
            var d = DateTime.Now.AddSeconds(-20);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToElapsedStringOneMinuteAgo()
        {
            var expected = "1 minute ago";
            var d = DateTime.Now.AddMinutes(-1);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToElapsedString5MinutesAgo()
        {
            var expected = "5 minutes ago";
            var d = DateTime.Now.AddMinutes(-5);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToElapsedString1HourAgo()
        {
            var expected = "1 hour ago";
            var d = DateTime.Now.AddHours(-1);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ToElapsedString3HoursAgo()
        {
            var expected = "3 hours ago";
            var d = DateTime.Now.AddHours(-3);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ToElapsedStringYesterday()
        {
            var expected = "yesterday";
            var d = DateTime.Now.AddDays(-1);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToElapsedString4DaysAgo()
        {
            var expected = "4 days ago";
            var d = DateTime.Now.AddDays(-4);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToElapsedString3WeeksAgo()
        {
            var expected = "3 weeks ago";
            var d = DateTime.Now.AddDays(-3 * 7);

            var actual = d.ToElapsedTime();

            Assert.AreEqual(expected, actual);
        }
    }
}
