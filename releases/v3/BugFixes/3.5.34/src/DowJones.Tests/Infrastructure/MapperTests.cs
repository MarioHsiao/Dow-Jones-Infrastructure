using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Infrastructure
{
    [TestClass]
    public class MapperTests : UnitTestFixture
    {

        [TestMethod]
        public void ShouldConvertFromOneTypeToAnotherGivenARegisteredConverter()
        {
            IMapper mapper = CreateFactory<string>(new TestTypeMapper<string, int>(int.Parse));

            Assert.AreEqual(4, mapper.Map<int>("4"));
        }

        [TestMethod]
        public void ShouldThrowATypeConverterNotFoundExceptionWhenNoTypeConverterExistsForASourceType()
        {
            IMapper mapper = CreateFactory(typeConverters: null);

            ExpectException<TypeMappingNotFoundException>(() =>
                mapper.Map<int>("4")
            );
        }

        [TestMethod]
        public void ShouldThrowAnInvalidCastExceptionWhenTargetTypeIsDifferentThanRequested()
        {
            // Create an string => int mapping
            IMapper mapper = CreateFactory<string>(new TestTypeMapper<string, int>(str => int.Parse(str)));

            try
            {
                // Try to convert string => double (not int)
                mapper.Map<double>("4");
            }
            catch(TypeMappingException)
            {
                // PASS!
                return;
            }

            Assert.Fail("Expected invalid cast exception");
        }

        [TestMethod]
        public void ShouldMapToSpecificTargetTypeEvenThoughMultipleMappersExistForSourceType()
        {
            var expectedResult = new DerivedClassA();

            IMapper mapper = new Mapper(new [] {
                TypeMapperDefinition.Create(new TestTypeMapper<string, DerivedClassA>(x => expectedResult)),
                TypeMapperDefinition.Create(new TestTypeMapper<string, DerivedClassB>(x => { throw new ApplicationException("Wrong type!"); }))
            });

            var actualResult = mapper.Map<DerivedClassA>("Test");

            Assert.AreSame(expectedResult, actualResult);
        }

        [TestMethod]
        public void ShouldFallBackToBaseTypeConverterWhenThereIsNoDirectMatchForARegisteredConverter()
        {
            bool mappingExecuted = false;

            // Create a mapping from some arbitrary type (string) to the
            // BASE class (not the derived class)
            IMapper mapper = CreateFactory<BaseClass>(
                new TestTypeMapper<BaseClass, string>(x =>
                {
                                                                mappingExecuted = true;
                                                                return string.Empty;
                                                            })
            );

            // Try to convert the arbitrary type from the DERIVED class
            mapper.Map<string>(new DerivedClassA());

            // Make sure the BASE class mapping was executed
            Assert.IsTrue(mappingExecuted, "Mapping was not executed");
        }

        [TestMethod]
        public void ShouldConvertStringToEnum()
        {
            const DayOfWeek ExpectedDayOfWeek = DayOfWeek.Wednesday;

            IMapper mapper = new Mapper();
            var actualDayOfWeek = mapper.Map<DayOfWeek>(ExpectedDayOfWeek.ToString());

            Assert.AreEqual(ExpectedDayOfWeek, actualDayOfWeek);
        }

        [TestMethod]
        public void ShouldConvertStringToNullableEnum()
        {
            const DayOfWeek ExpectedDayOfWeek = DayOfWeek.Wednesday;

            IMapper mapper = new Mapper();
            var actualDayOfWeek = mapper.Map<DayOfWeek?>(ExpectedDayOfWeek.ToString());

            Assert.AreEqual(ExpectedDayOfWeek, actualDayOfWeek);
        }

        [TestMethod]
        public void ShouldConvertNullStringToNullWhenTargetTypeIsNullableEnum()
        {
            IMapper mapper = new Mapper();
            var actualDayOfWeek = mapper.Map<DayOfWeek?>((string)null);

            Assert.IsNull(actualDayOfWeek);
        }

        [TestMethod]
        public void ShouldConvertEmptyStringToDefaultEnumValue()
        {
            IMapper mapper = new Mapper();
            var actualDayOfWeek = mapper.Map<DayOfWeek>(string.Empty);

            Assert.AreEqual(default(DayOfWeek), actualDayOfWeek);
        }

        [TestMethod]
        public void ShouldConvertNullStringToDefaultEnumValue()
        {
            IMapper mapper = new Mapper();
            var actualDayOfWeek = mapper.Map<DayOfWeek>((string)null);

            Assert.AreEqual(default(DayOfWeek), actualDayOfWeek);
        }

        [TestMethod]
        public void ShouldConvertTwoDifferentEnumTypesWithTheSameNames()
        {
            IMapper mapper = new Mapper();
            Assert.AreEqual(EnumB.Value2, mapper.Map<EnumB>(EnumA.Value2));
        }

        [TestMethod]
        public void ShouldConvertTwoDifferentEnumTypesWithTheSameNamesRegardlessOfCase()
        {
            IMapper mapper = new Mapper();
            Assert.AreEqual(EnumB.WaCkycaSe, mapper.Map<EnumB>(EnumA.WackyCase));
        }


        protected Mapper CreateFactory<T>(ITypeMapper mapper)
        {
            return CreateFactory(new Dictionary<Type, ITypeMapper> { { typeof(T), mapper } });
        }

        protected Mapper CreateFactory(IEnumerable<KeyValuePair<Type, ITypeMapper>> typeConverters = null)
        {
            typeConverters = typeConverters ?? Enumerable.Empty<KeyValuePair<Type, ITypeMapper>>();
            return new Mapper(typeConverters.Select(x => new TypeMapperDefinition(x.Value) { SourceType = x.Key }));
        }


        class TestTypeMapper<TSource, TTarget> : TypeMapper<TSource, TTarget>
        {
            public TestTypeMapper(Func<TSource, TTarget> func)
                : base(func)
            {
            }
        }


        class BaseClass {}
        class DerivedClassA : BaseClass { }
        class DerivedClassB : BaseClass { }

        enum EnumA { Value1, Value2, WackyCase }
        enum EnumB { Value1, Value2, WaCkycaSe }
    }
}