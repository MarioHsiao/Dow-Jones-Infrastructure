using System.IO;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
        }
    }
}