using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.DependencyInjection;

namespace DowJones.Dash.DataSources.Gomez
{
    public class PageTimings : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetPageLoadDetails] @seconds = 300 ";
        }
    }
}