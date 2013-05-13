using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class CustomAssemblyRegistryTests : UnitTestFixtureBase<AssemblyRegistry>
    {
        private static readonly Assembly CurrentAssembly = typeof(CustomAssemblyRegistryTests).Assembly;
        private static readonly Type InterfaceType = typeof(ITestInterfaceForCustomAssemblyRegistryTests);
        private static readonly Type AbstractType = typeof (TestAbstractImplementationForCustomAssemblyRegistryTests);
        private static readonly Type ConcreteType = typeof (TestConcreteImplementationForCustomAssemblyRegistryTests);
        private static readonly Type GenericType = typeof (TestGenericImplementationForCustomAssemblyRegistryTests<>);

        protected AssemblyRegistry AssemblyRegistry
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
            IEnumerable<Type> types = 
                AssemblyRegistry
                    .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>();

            CollectionAssert.Contains(types.ToList(), ConcreteType);
        }

        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldReturnConcreteTypeThatImplementsAbstractClass()
        {
            IEnumerable<Type> types =
                AssemblyRegistry
                    .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>();

            CollectionAssert.Contains(types.ToList(), ConcreteType);
        }

        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldNotReturnAbstractTypeThatImplementsInterface()
        {
            IEnumerable<Type> actualTypes = AssemblyRegistry
                .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>();

            Assert.AreNotEqual(0, actualTypes.Count(), "Found no types, but expected at least one");
            CollectionAssert.DoesNotContain(actualTypes.ToList(), AbstractType);
        }

        [TestMethod]
        public void GetConcreteTypesDerivingFrom_ShouldNotReturnInterface()
        {
            IEnumerable<Type> actualTypes = AssemblyRegistry
                .GetConcreteTypesDerivingFrom<ITestInterfaceForCustomAssemblyRegistryTests>();

            Assert.AreNotEqual(0, actualTypes.Count(), "Found no types, but expected at least one");
            CollectionAssert.DoesNotContain(actualTypes.ToList(), InterfaceType);
        }

        [TestMethod]
        public void GetKnownTypes_ShouldReturnConcreteImplementationsOfABaseClass()
        {
            IEnumerable<Type> actualTypes = AssemblyRegistry
                .GetKnownTypesOf<TestAbstractImplementationForCustomAssemblyRegistryTests>();

            CollectionAssert.Contains(actualTypes.ToList(), ConcreteType);
        }

        [TestMethod]
        public void GetKnownTypes_ShouldNotReturnGenericImplementations()
        {
            IEnumerable<Type> actualTypes = AssemblyRegistry
                .GetKnownTypesOf<ITestInterfaceForCustomAssemblyRegistryTests>();

            Assert.AreNotEqual(0, actualTypes.Count(), "Found no types, but expected at least one");
            CollectionAssert.DoesNotContain(actualTypes.ToList(), GenericType);
        }


        protected override AssemblyRegistry CreateUnitUnderTest()
        {
            var unitUnderTest = new AssemblyRegistry(new[] { CurrentAssembly });
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

    public class TestGenericImplementationForCustomAssemblyRegistryTests<T>
        : TestAbstractImplementationForCustomAssemblyRegistryTests
    {
    }
    
    public class TestConcreteImplementationForCustomAssemblyRegistryTests
        : TestAbstractImplementationForCustomAssemblyRegistryTests
    {
    }
    
    #endregion
}