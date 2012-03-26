using DowJones.Tools.Ajax.FamilyTree;
using Factiva.Gateway.Messages.FCE.DnB.V1_0;
using DowJones.Utilities.Ajax.FamilyTree.Converter;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Utilities.Ajax.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class FamilyTreeConversionManager
    {
        public FamilyTreeDataResult Process(ControlData cData, GetPathToGlobalUltimateDunsResponse getPathToGlobalUltimateDunsResponse)
        {
            var getDnbRecordsExResponseConverter = new GetDnbRecordsExResponseConverter();
            return getDnbRecordsExResponseConverter.Process(cData, getPathToGlobalUltimateDunsResponse);
        }
    }
}
