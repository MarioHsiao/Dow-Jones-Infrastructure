using DowJones.Utilities.Ajax.TabStrip;
using DowJones.Utilities.Ajax.TabStrip.Converters;
using Factiva.Gateway.Messages.Assets.PNP.V1_0;


namespace DowJones.Utilities.Ajax.Converters
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
