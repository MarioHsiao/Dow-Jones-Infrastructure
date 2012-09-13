using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.DependencyInjection;

namespace DowJones.Dash.Website.DataSources
{
    public class PageTimings : GomezDataSource, IInitializable
    {
        private const string QueryTemplate =
            @"
            WITH CaptureTimes
            AS (
              SELECT
	               [page_id]
                  ,[capture_time]
                  ,[dom_ready_time]
                  ,[page_load_time]
                  ,[perceived_render]
              FROM [SplunkExport].[dbo].[gomez_rum] with (nolock) 
            )

            SELECT
	             pg.[page_name],
	             pg.[URL],
	             Timings.*
            FROM (
                [[PAGE_TIMINGS]]
            ) Timings
            INNER JOIN [SplunkExport].[dbo].[gomez_page] pg with (nolock) on pg.[page_id] = Timings.[page_id]";

        public IEnumerable<int> PageIds { get; set; }

        public PageTimings()
        {
            PageIds = new[] {421139, 421143, 1940521};
        }

        public void Initialize()
        {
            var pageTimings =
                PageIds.Select(x =>
                               "SELECT * FROM ( SELECT TOP 1 * FROM CaptureTimes WHERE page_id  = " + x +
                               " ORDER BY [capture_time] DESC ) T");

            var query = new StringBuilder(QueryTemplate);
            query.Replace("[[PAGE_TIMINGS]]", string.Join("\r\nUNION\r\n", pageTimings));

            Query = query.ToString();
        }
    }
}