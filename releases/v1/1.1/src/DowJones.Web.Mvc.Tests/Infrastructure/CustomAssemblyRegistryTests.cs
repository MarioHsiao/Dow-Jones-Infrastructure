using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class CustomAssemblyRegistryTests : UnitTestFixtureBase<CustomAssemblyRegistry>
    {
        private static readonly Assembly CurrentAssembly = typeof(CustomAssemblyRegistryTests).Assembly;
        private static readonly Type InterfaceType = typeof(ITestInterfaceForCustomAssemblyRegistryTests);
        private static readonly Type AbstractType = typeof (TestAbstractImplementationForCustomAssemblyRegistryTests);
        private static readonly Type ConcreteType = typeof (TestConcreteImplementationForCustomAssemblyRegistryTests);

        protected CustomAssemblyRegistry AssemblyRegistry
        {
            get { return UnitUnderTest; }
        }

        [TestMethod]
        public void ShouldRegisterInterfaceTypeInExportedTypes()
        {
            CollectionAssert.Contains(AssemblyRegistry.ExportedTypes.ToList(), InterfaceType);
        }

        [TestMethod]
        public void ShouldNotRegisterInterfaceTypeAsConcreteType()
        {
            CollectionAssert.DoesNotContain(AssemblyRegistry.ConcreteTypes.ToList(), InterfaceType);
        }

        [TestMethod]
        public void ShouldRegisterAbstractTypeInExportedTypes()
        {
            CollectionAssert.Contains(AssemblyRegistry.ExportedTypes.ToList(), AbstractType);
        }

        [TestMethod]
        public void ShouldNotRegisterAbstractTypeAsConcreteType()
        {
            CollectionAssert.DoesNotContain(AssemblyRegistry.ConcreteTypes.ToList(), AbstractType);
        }

        [TestMethod]
        public void ShouldRegisterConcreteTypeInExportedTypes()
        {
            CollectionAssert.Contains(AssemblyRegistry.ExportedTypes.ToList(), ConcreteType);
        }

        [TestMethod]
        public void ShouldRegisterConcreteTypeAsConcreteType()
        {
            CollectionAssert.Contains(AssemblyRegistry.ConcreteTypes.ToList(), ConcreteType);
        }


        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldReturnConcreteTypeThatImplementsInterface()
        {
            Type actualType = AssemblyRegistry
                .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>()
                .Single();

            Assert.AreEqual(ConcreteType, actualType);
        }

        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldReturnConcreteTypeThatImplementsAbstractClass()
        {
            Type actualType = AssemblyRegistry
                .GetConcreteTypesDerivingFrom<TestAbstractImplementationForCustomAssemblyRegistryTests>()
                .Single();

            Assert.AreEqual(ConcreteType, actualType);
        }

        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldNotReturnAbstractTypeThatImplementsInterface()
        {
            IEnumerable<Type> actualTypes = AssemblyRegistry
                .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>();

            CollectionAssert.DoesNotContain(actualTypes.ToList(), AbstractType);
        }


        protected override CustomAssemblyRegistry CreateUnitUnderTest()
        {
            var unitUnderTest = new CustomAssemblyRegistry(new[] { CurrentAssembly });
            return unitUnderTest;
        }
    }

    #region Test Interface and Class Definitions

    public interface ITestInterfaceForCustomAssemblyRegistryTests
    {
    }

    public abstract class TestAbstractImplementationForCustomAssemblyRegistryTests 
        : ITestInterfaceForCustomAssemblyRegistryTests
    {
    }

    public class TestConcreteImplementationForCustomAssemblyRegistryTests
        : TestAbstractImplementationForCustomAssemblyRegistryTests
    {
    }
    
    #endregion
}