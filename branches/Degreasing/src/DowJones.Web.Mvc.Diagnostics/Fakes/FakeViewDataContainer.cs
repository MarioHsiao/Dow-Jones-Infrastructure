using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeViewDataContainer : IViewDataContainer
    {
        public ViewDataDictionary ViewData { get; set; }

        public FakeViewDataContainer(object model = null)
        {
            ViewData = new ViewDataDictionary(model);
        }
    }
}