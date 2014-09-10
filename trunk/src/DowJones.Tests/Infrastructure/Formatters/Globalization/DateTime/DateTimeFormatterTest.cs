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
            _dateTimeFormatter = new DateTimeFormatter("fr");
        }

        [TestMethod]
        public void Full_Date_Format_In_French_Should_Be_d_MMMM_YYYY()
        {
            string date = "08/11/2014";
            System.DateTime dt = Convert.ToDateTime(date);
            var formattedDate = _dateTimeFormatter.FormatFullDate(dt, true);
            Assert.AreEqual(formattedDate, "11 août 2014", "Formatted date in french");
        }
    }
}
