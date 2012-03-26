using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using DowJones.Articles;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Managers.Alert;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Preference;
using DowJones.Mapping;
using DowJones.Preferences;
using DowJones.Properties;
using DowJones.Security;
using DowJones.Session;
using DowJones.Token;
using DowJones.Web;
using DowJones.Web.UI;
using log4net;
using Ninject;
using Ninject.Activation;

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

            Bind<IUserContext>().To<UserContext>().InTransientScope();
            
            BindToFactory<ReferringProduct, ReferringProductFactory>().InRequestScope();

            Bind<IMapper>().ToConstant(Mapper.Instance).InTransientScope();

            Bind<RouteCollection>().ToConstant(RouteTable.Routes).InTransientScope();
            Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            Bind<HttpResponseBase>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Response).InTransientScope();
            Bind<HttpRequestBase>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Request).InTransientScope();
            Bind<System.Web.Caching.Cache>().ToMethod(x => x.Kernel.Get<HttpContextBase>().Cache).InTransientScope();

            BindToFactory<IClientResourceManager, ClientResourceManagerFactory>().InSingletonScope();

            Bind<IClientResourceProcessor>().To<ClientResourcePopulator>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceAppSettingProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceTokenProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientResourceWebResourceProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<StylesheetResourceImageUrlResolver>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ClientTemplateResourceProcessor>().InRequestScope();
            Bind<IClientResourceProcessor>().To<ScriptResourceUniquenessProcessor>().InRequestScope();
            
            if (Settings.Default.JavaScriptMinificationEnabled)
                Bind<IClientResourceProcessor>().To<JavaScriptMinifier>().InRequestScope();

            Bind<IClientSideObjectWriterFactory>().To<ClientSideObjectWriterFactory>().InSingletonScope();

            Bind<IContentCache>().To<WebContentCache>().InTransientScope();

            Bind<IISVersion>()
                .ToMethod(x => x.Kernel.Get<HttpRequestBase>().GetIISVersion())
                .InSingletonScope();

            Bind<ITokenRegistry>().To<TokenRegistry>().InRequestScope();

            Bind<IResourceTextManager>().ToMethod(x => ResourceTextManager.Instance).InTransientScope();

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
        }
    }
}
