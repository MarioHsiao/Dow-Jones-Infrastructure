﻿using DowJones.DependencyInjection;

namespace DowJones.Dash.DataSources.Gomez
{
    public class PageLoadHistoricalDetails : GomezDataSource, IInitializable
    {
        public void Initialize()
        {
            Query = @"exec [SplunkExport].[dbo].[GetPageLoadHistoricalDetails] @pageid = 421139, @days = 1";
        }
    }
}