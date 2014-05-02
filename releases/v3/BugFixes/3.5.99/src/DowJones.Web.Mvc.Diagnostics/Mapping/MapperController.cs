using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics.Mapping
{
    public class MapperController : Controller
    {
        public ActionResult Index()
        {
            if (Mapper.Instance == null)
                return Content("Mapper is not initialized - nothing to debug");

            if (!(Mapper.Instance is Mapper))
                return Content("Mapper is not of type DowJones.Mapper - I don't know how to handle that!");
            
            var mapper = (Mapper)Mapper.Instance;
            var typeMappers = mapper.TypeMappers.ToList().OrderBy(x => x.SourceType.FullName);
            var viewModel = new MapperDebuggerViewModel(typeMappers);

            return new DiagnosticsViewAction<MapperDebuggerView>(viewModel);
        }
    }
}


