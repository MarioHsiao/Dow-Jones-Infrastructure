using System;
using System.Collections.Generic;
using DowJones.Attributes;
using DowJones.Globalization;
using DowJones.Token;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DowJones.Infrastructure.Tokens
{
    [TestClass]
    public class TokenRegistryTests : UnitTestFixtureBase<TokenRegistry>
    {
        public const string AssignedTokenValue = "assignedToken";

        private Mock<IResourceTextManager> _mockResourceManager;

        protected TokenRegistry TokenRegistry
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldRetrieveTokenValueForTokenName()
        {
            const string expected = "expected";
            const string tokenName = "tokenName";

            _mockResourceManager
                .Setup(x => x.GetString(tokenName))
                .Returns(expected);

            var actual = TokenRegistry.Get(tokenName);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldUseEnumName()
        {
            const string expected = "expected";

            _mockResourceManager
                .Setup(x => x.GetString(TestEnum.Value.ToString()))
                .Returns(expected);

            var actual = TokenRegistry.Get(TestEnum.Value);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShouldUseTokenNameFromAssignedTokenAttributeWhenItExists()
        {
            const string expected = "expected";

            _mockResourceManager
                .Setup(x => x.GetString(AssignedTokenValue))
                .Returns(expected);

            var actual = TokenRegistry.Get(TestEnum.ValueWithAssignedToken);

            Assert.AreEqual(expected, actual);
        }


        public enum TestEnum
        {
            Value,

            [AssignedToken(AssignedTokenValue)]
            ValueWithAssignedToken,
        }


        protected override TokenRegistry CreateUnitUnderTest()
        {
            _mockResourceManager = new Mock<IResourceTextManager>();

            var enumResolver = new EnumTokenResolver(new AssemblyRegistry(GetType().Assembly));

            return new TokenRegistry(_mockResourceManager.Object, enumResolver);
        }
    }
}
