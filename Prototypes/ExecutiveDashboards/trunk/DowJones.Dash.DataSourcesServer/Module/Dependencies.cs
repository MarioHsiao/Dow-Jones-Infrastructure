using DowJones.Dash.Caching;
using DowJones.Dash.Serializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ninject.Modules;
using SignalR;

namespace DowJones.Dash.DataSourcesServer.Module
{
    class DependenciesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDashboardMessageCache>().To<DashboardMessageCache>().InSingletonScope();
            Bind<DashboardMessageQueue>().To<DashboardMessageQueue>().InSingletonScope();

            Bind<IJsonSerializer>().ToMethod(x => new CustomJsonNetSerializer(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                // this does not work                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new[] { new StringEnumConverter() },
                TypeNameHandling = TypeNameHandling.Objects,
            })).InRequestScope();
        }
    }
}
