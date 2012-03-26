
namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Tests.Interfaces
{
    interface IPopulateTest
    {
        void PopulateMethodTest();
    }

    internal interface IUpdateTest
    {
        void UpdateMethodTest();
    }

    internal interface ISampleModule<out T>
    {
        T GetSampleModule();
    }
}
