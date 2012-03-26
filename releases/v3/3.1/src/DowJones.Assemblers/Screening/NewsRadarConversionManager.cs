using DowJones.Ajax.NewsRadar;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Assemblers.Screening
{
    public class NewsRadarConversionManager
    {
        /// <summary>
        /// Processes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public NewsRadarResultSet Process(GetCompanyScreeningListExResponse response)
        {
            IListDataResultConverter converter = new GetCompanyScreeningListExResponseConverter(response);
            return (NewsRadarResultSet)converter.Process();
        }
    }
}
