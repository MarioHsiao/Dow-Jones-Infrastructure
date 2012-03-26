using System.Collections.Generic;
using System.Web.Razor;

namespace DowJones.Web.Mvc.Razor
{
    public interface IAuxiliaryContentGenerator
    {
        IEnumerable<AuxiliaryContent> GenerateAuxiliaryContent(GeneratorResults generatorResults);
    }
}