using System.Collections.Generic;
using System.Web.Razor;

namespace DowJones.Web.Razor.AuxiliaryContent
{
    public interface IAuxiliaryContentGenerator
    {
        IEnumerable<AuxiliaryContent> GenerateAuxiliaryContent(GeneratorResults generatorResults);
    }
}