using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Tools.Ajax;
using DowJones.Utilities.Ajax.NewsRadar;
using DowJones.Utilities.Ajax.NewsRadar.Converter;
using Factiva.Gateway.Messages.Screening.V1_1;

namespace DowJones.Utilities.Ajax.Converters
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
