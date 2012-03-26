using System;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    public class FactivaModuleModelMappingNotFoundException : Exception
    {
        public FactivaModuleModelMappingNotFoundException(Type factivaModuleType)
            : base(string.Format("Factiva Module type {0} not mapped to any Canvas Module type", factivaModuleType.FullName))
        {
        }
    }
}