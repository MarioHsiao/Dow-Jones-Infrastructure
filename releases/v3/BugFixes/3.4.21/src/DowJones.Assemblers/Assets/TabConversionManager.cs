using DowJones.Ajax.TabStrip;
using Factiva.Gateway.Messages.Assets.PNP.V1_0;

namespace DowJones.Assemblers.Assets
{
  
    /// <summary>
    /// 
    /// </summary>
    public class TabConversionManager
    {
        public TabStripDataResult Process(GetNewsPageListResponse getNewsPageListResponse, GenerateTabOptions generateTabOptions)
        {
            var getNewsExPageListResponse = new GetNewsExPageListResponse();
            return getNewsExPageListResponse.Process(getNewsPageListResponse, generateTabOptions);
        }
    }
}
