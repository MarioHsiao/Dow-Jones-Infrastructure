using DowJones.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure.Converters
{
    [TestClass]
    public class PercentageConverterTests
    {
        private PercentageConverter _converter;


        [TestInitialize]
        public void TestInitialize()
        {
            _converter = new PercentageConverter();
        }


        [TestMethod]
        public void ShouldConvertPositiveDoublesToPercentageString()
        {
            _converter.Precision = 0;

            var convertedValue = _converter.ConvertTo((double)1.23123, typeof(string));
            
            Assert.AreEqual("123%", convertedValue);
        }

        [TestMethod]
        public void ShouldConvertPositiveDoublesToPercentageStringWithPrecision()
        {
            _converter.Precision = 2;

            var convertedValue = _converter.ConvertTo((double)1.23123, typeof(string));

            Assert.AreEqual("123.12%", convertedValue);
        }

        [TestMethod]
        public void ShouldConvertNegativeDoublesToPercentageString()
        {
            _converter.Precision = 0;

            var convertedValue = _converter.ConvertTo((double)-1.23123, typeof(string));

            Assert.AreEqual("-123%", convertedValue);
        }

        [TestMethod]
        public void ShouldConvertNegativeDoublesToPercentageStringWithPrecision()
        {
            _converter.Precision = 2;

            var convertedValue = _converter.ConvertTo((double)-1.23123, typeof(string));

            Assert.AreEqual("-123.12%", convertedValue);
        }
    }
}
