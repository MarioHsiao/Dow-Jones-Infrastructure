using System.Linq;
using DowJones.Attributes;
using DowJones.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class EnumHelperTests : UnitTestFixture
    {
        private const string Value1Token = "Data 1";
        private const string Value2Token = "Data 2";

        [TestInitialize]
        public override void SetUp()
        {
            EnumHelper.TokenRegistryThunk = () => new MockTokenRegistry();
        }

        [TestMethod]
        public void ShouldConvertEnumsToKeyValuePairs()
        {
            var keyValues = EnumHelper.ToKeyValuePairs<TestEnum>();

            Assert.AreNotEqual(0, keyValues.Count());
        }

        [TestMethod]
        public void ShouldConvertEnumsToKeyValuePairsWithoutExclusions()
        {
            const TestEnum ExcludedValue = TestEnum.Value2;

            var keyValues = EnumHelper.ToKeyValuePairs<TestEnum>(new [] {ExcludedValue});

            Assert.IsFalse(keyValues.Any(x => x.Key == ExcludedValue),
                           "Found the excluded value, but it should have been excluded!");
        }

        [TestMethod]
        public void ShouldResolveTokenNamesForAssignedTokens()
        {
            var keyValues = EnumHelper.ToKeyValuePairs<TestEnum>();

            var expected = EnumHelper.TokenRegistryThunk().Get(TestEnum.Value1);
            var actual = keyValues.Single(x => x.Key == TestEnum.Value1).Value;

            Assert.AreEqual(expected, actual);
        }


        enum TestEnum
        {
            Unknown = 0,

            [AssignedToken(Value1Token)]
            Value1,

            [AssignedToken(Value2Token)]
            Value2
        }
    }
}