using System.Collections.Generic;
using System.Web.Routing;
using DowJones.Assemblers;
using DowJones.Assemblers.Charting.MarketData;
using DowJones.Assemblers.Preferences;
using DowJones.Assemblers.Security;
using DowJones.Assemblers.Session;
using DowJones.DependencyInjection;
using DowJones.Infrastructure.Common;
using DowJones.Managers.Charting.MarketData;
using DowJones.Managers.RelatedConcept;
using DowJones.Managers.Sparkline;
using DowJones.Models.Charting.MarketData;
using DowJones.Pages;
using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.Search.UI.Components.Builders;
using DowJones.Web.Navigation;
using DowJones.Web.Showcase.Connections;
using DowJones.Web.Showcase.Connections.RealTimeAlerts;
using DowJones.Web.Showcase.Mocks;
using SignalR.Hosting.AspNet.Routing;

namespace DowJones.Web.Showcase
{
    public class ShowcaseDependencyBindingsModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {

            RouteTable.Routes.MapConnection<RealTimeAlertsConnection>("realtimealertsconnection", "realtimealertsconnection/{*operation}");
            RouteTable.Routes.MapConnection<MyConnection>("echo", "echo/{*operation}");
            
            BindToFactory<IPrinciple, PrincipleFactory>().InRequestScope();
            BindToFactory<IControlData, ControlDataFactory>().InRequestScope();
            BindToFactory<IPreferences, PreferencesFactory>().InRequestScope();
            BindToFactory<IUserSession, DevelopmentSessionFactory>().InRequestScope();
//            RebindToFactory<IPreferenceService, CachedPreferenceServiceFactory>().InRequestScope();

            Bind<IMenuDataSource>().To<ShowcaseMenuDataSource>().InSingletonScope();

            Bind<IPageAssetsManager>().To<MockPageAssetsManager>().InRequestScope();

            // In a production app, this should be replaced with something meaningful rather than hard coded values
            Bind<Product>().ToConstant(new Product("CM", "Communicator", "Communicator")).InSingletonScope();

            Bind<ISparklineService>().To<SparklineService>();

            Bind<ISourceListService>().To<SourceListService>();

            Bind<IRelatedConceptService>().To<RelatedConceptService>();

            //Bind<IJsonSerializer>().To<JsonSerializers.JsonConvertAdapter>();

            //Bind<SignalR.Infrastructure.IDependencyResolver>().To<NinjectDependencyResolver>();

            Bind <IAssembler<MarketDataInstrumentIntradayResultSet, IEnumerable<MarketChartDataServicePartResult<MarketChartDataPackage>>>>().To<MarketDataInstrumentIntradayResultSetAssembler>();
        }
    }
}
