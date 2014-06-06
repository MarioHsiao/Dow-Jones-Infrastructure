using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Canvas.Mapping
{
    // should be used for setting the module type for Swap Editors only
    // workaround enum for now
    // need to remove this and refactor dependent code once DAL comes up with a better solution to pass the IRT codes around
    public enum ModuleType
    {
        
        
        AlertsNewspageModule,
        
        SyndicationNewspageModule,

        NewsstandNewspageModule,
        
        CustomTopicsNewspageModule,

        SourcesNewspageModule,

        RadarNewspageModule,
        
        RegionalMapNewspageModule,

        TopNewsNewspageModule,
        
        TrendingNewsPageModule,

        SummaryNewspageModule,

        CompanyOverviewNewspageModule,

        VideosNewspageModule
    }
}
