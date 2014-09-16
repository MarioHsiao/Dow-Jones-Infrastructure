using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Formatters.Globalization.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Formatters.Globalization.DateTime
{
    [TestClass]
    public class DateTimeFormatterTest
    {
        private DateTimeFormatter _dateTimeFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeFormatter = new DateTimeFormatter("fr")
            {
                TimeZoneBuilder = {ConvertToLocalTime = true}
            };
        }

        [TestMethod]
        public void Full_Date_Format_In_French_Should_Be_d_MMMM_YYYY()
        {
            string date = "08/11/2014";
            System.DateTime dt = Convert.ToDateTime(date);
            var formattedDate = _dateTimeFormatter.FormatFullDate(dt, true);
            Assert.AreEqual(formattedDate, "11 août 2014", "Formatted date in french");
        }

        [TestMethod]
        public void Full_Date_And_Time_Format_In_French_Should_Be_d_MMMM_YYYY_HH_MM()
        {
            string date = "08/11/2014 1:14 AM";
            System.DateTime dt = Convert.ToDateTime(date);
            var formattedDate = _dateTimeFormatter.FormatFullDateTime(dt, true);
            Assert.AreEqual(formattedDate, "11 août 2014 1:14 ", "Full date and time format in french");
        }

        [TestMethod]
        public void Month_Day_Year_Format_In_French_Should_Be_d_MMMM_YYYY()
        {
            string date = "08/11/2014";
            System.DateTime dt = Convert.ToDateTime(date);
            var formattedDate = _dateTimeFormatter.FormatMonthDayYear(dt, true);
            Assert.AreEqual(formattedDate, "11 août 2014", "Month day year format in french");
        }
    }
}
