using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using DowJones.Articles;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Loggers;
using DowJones.Managers.Alert;
using DowJones.Managers.Multimedia;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Preference;
using DowJones.Mapping;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Session;
using DowJones.Token;
using DowJones.Utilities;
using DowJones.Web;
using DowJones.Web.Configuration;
using DowJones.Web.UI;
using log4net;
using Ninject;
using Ninject.Activation;
using DowJones.Web.ClientResources;
using DowJones.Managers.SocialMedia.TweetRiver;
using DowJones.Managers.SocialMedia;
using Ninject.Web.Common;

namespace DowJones
{
    public class DowJonesInfrastructureBindingModule : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            Bind<Factiva.Gateway.Utils.V1_0.ControlData>()
                .ToMethod(x => ControlDataManager.Convert(x.Kernel.Get<IControlData>()))
                .InTransientScope();

            Bind<IArticleService>().To<ArticleService>().InRequestScope();
            
            Bind<IPreferenceService>().To<PreferenceService>().InRequestScope();

            Bind<ITransactionTimer>().To<BasicTransactionTimer>().InSingletonScope();

            Bind<IUserContext>().To<UserContext>().InTransientScope();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<ReferringProduct, ReferringProductFactory>().InRequestScope();

            Bind<IMapper>().ToConstant(Mapper.Instance).InTransientScope();

            Bind<RouteCollection>().ToConstant(RouteTable.Routes).InTransientScope();
            Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            Bind<HttpResponseBase>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Response).InTransientScope();
            Bind<HttpRequestBase>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Request).InTransientScope();
            Bind<System.Web.Caching.Cache>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Cache).InTransientScope();

            Bind<IClientResourceManager>().To<ClientResourceManager>().InSingletonScope();
            Bind<IClientResourceRepository>().To<LocalFileClientResourceRepository>().InSingletonScope();
            Bind<IClientResourceRepository>().To<EmbeddedClientResourceRepository>().InSingletonScope();

            Bind<IClientResourceProcessor>().To<ClientResourcePopulator>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceUrlProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceAppSettingProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceTokenProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceWebResourceProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<StylesheetResourceImageUrlResolver>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientTemplateResourceProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<DependentResourceProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<RequireJsScriptModuleWrapper>().InRequestScope();
            Bind<IClientResourceProcessor>().To<JavaScriptMinifier>().InRequestScope();


            Bind<IClientTemplateParser>().To<DoTJsClientTemplateParser>().InRequestScope();
            
            Bind<IClientSideObjectWriterFactory>().To<ClientSideObjectWriterFactory>().InSingletonScope();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<ClientConfiguration, ClientConfigurationFactory>().InRequestScope();

            Bind<IContentCache>().To<WebContentCache>();

            Bind<IISVersion>()
                .ToMethod(x => x.Kernel.Get<HttpRequestBase>().GetIISVersion())
                .InSingletonScope();

            Bind<ITokenRegistry>().To<TokenRegistry>().InRequestScope();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<IResourceTextManager, ResourceTextManagerFactory>().InThreadScope();

            Bind<ISearchService>().To<SearchService>().InRequestScope();

            Bind<IContentSearchService>().To<ContentSearchService>().InRequestScope();
            Bind<IAlertSearchService>().To<AlertHeadlineManager>().InRequestScope();

            Bind<CultureInfo>()
                .ToMethod(x =>
                    CultureManager.GetCultureInfoFromInterfaceLanguage(
                        x.Kernel.Get<IPreferences>().InterfaceLanguage))
                .InRequestScope();

            Bind<ILog>()
                .ToMethod(x => {
                    IRequest parentRequest = x.Request.ParentRequest;
                    
                    if(parentRequest == null)
                        throw new ApplicationException("Cannot retrieve an ILog instance directly - it must be injected");

                    return LogManager.GetLogger(parentRequest.Service);
                }).InTransientScope();

            Bind<ISearchQueryResourceManager>().To<SearchQueryResourceManager>().InRequestScope();

            Bind<ISearchPreferenceService>().To<SearchPreferenceService>().InRequestScope();

            Bind<TaskFactory>().ToMethod(x => TaskFactoryManager.Instance.GetDefaultTaskFactory()).InSingletonScope();

            Bind<ISocialMediaProvider>().To<TweetRiverProvider>().InRequestScope();
            Bind<IMultimediaManager>().To<MultimediaManager>().InRequestScope();
        }
    }
}
