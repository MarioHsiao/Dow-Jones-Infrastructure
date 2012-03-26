using DowJones.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DowJones.Web.Mvc.UI
{
    [TestClass]
    public class ViewComponentModelMapperTests
    {

        [TestMethod]
        public void ShouldReturnComponentTypeForModelType()
        {
            var mapping = CreateMapping<TestViewComponent1, TestViewComponentModel>();
            var mapper = new ViewComponentModelMapper(new[] { mapping });

            var actualComponentType = mapper.GetComponentTypeFromDataType(typeof (TestViewComponentModel));
            Assert.AreEqual(typeof(TestViewComponent1), actualComponentType);
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousGenericTypeMappingException))]
        public void ShouldThrowAnExceptionWhenMultipleBindingsAreSuppliedForSameModel()
        {
            var mapping1 = CreateMapping<TestViewComponent1, TestViewComponentModel>();
            var mapping2 = CreateMapping<TestViewComponent2, TestViewComponentModel>();
            var mapper = new ViewComponentModelMapper(new[] { mapping1, mapping2 });

            mapper.GetComponentTypeFromDataType(typeof (TestViewComponentModel));
        }

        [TestMethod]
        public void ShouldReturnPreferredMappingWhenMultipleBindingsAreSuppliedForSameModel()
        {
            var mapping1 = CreateMapping<TestViewComponent1, TestViewComponentModel>();
            var mapping2 = CreateMapping<TestViewComponent2, TestViewComponentModel>();
            mapping2.GenericTypeMapping.Preferred = true;

            var mapper = new ViewComponentModelMapper(new[] { mapping1, mapping2 });

            var actualComponentType = mapper.GetComponentTypeFromDataType(typeof(TestViewComponentModel));
            Assert.AreEqual(typeof(TestViewComponent2), actualComponentType);
        }


        protected ViewComponentModelMapping CreateMapping<TViewComponent, TModel>()
        {
            return new ViewComponentModelMapping(new GenericTypeMapping {DeclaringType = typeof (TViewComponent), GenericType = typeof (TModel)});
        }


        #region Test classes

        class TestViewComponent1 : ViewComponentBase<TestViewComponentModel>
        {
            public override string ClientPluginName
            {
                get { return null; }
            }
        }

        class TestViewComponentModel : ViewComponentModel
        {
        }

        class TestViewComponent2 : TestViewComponent1
        {
            public override string ClientPluginName
            {
                get { return null; }
            }
        }

        class DerivedTestViewComponentModel : TestViewComponentModel
        {
        }

        #endregion
    }
}
