using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
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
using DowJones.Web.UI;
using log4net;
using DowJones.Web.ClientResources;
using DowJones.Managers.SocialMedia.TweetRiver;
using DowJones.Managers.SocialMedia;

namespace DowJones
{
    public class DowJonesInfrastructureBindingModule : DependencyInjectionModule
    {
		protected override void OnLoad(IContainer container)
        {
			container.Register<Factiva.Gateway.Utils.V1_0.ControlData>(() => ControlDataManager.Convert(container.GetInstance<IControlData>()));

			container.RegisterPerWebRequest<IArticleService, ArticleService>();
            
            container.RegisterPerWebRequest<IPreferenceService, PreferenceService>();

            container.RegisterSingle<ITransactionTimer, BasicTransactionTimer>();

            container.Register<IUserContext, UserContext>();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<ReferringProduct, ReferringProductFactory>().InRequestScope();

			container.RegisterSingle<IMapper>(Mapper.Instance);

			container.RegisterSingle<RouteCollection>(RouteTable.Routes);
			container.Register<HttpContext>(() => HttpContext.Current);
			container.Register<HttpContextBase>(() => new HttpContextWrapper(HttpContext.Current));
			container.Register<HttpResponseBase>(() => container.GetInstance<HttpContextBase>().Response);
			container.Register<HttpRequestBase>(() => container.GetInstance<HttpContextBase>().Request);
			container.Register<Cache>(() => container.GetInstance<HttpContextBase>().Cache);

            container.RegisterSingle<IClientResourceManager, ClientResourceManager>();
            container.RegisterSingle<IClientResourceRepository, LocalFileClientResourceRepository>();
            container.RegisterSingle<IClientResourceRepository, EmbeddedClientResourceRepository>();

            container.RegisterPerWebRequest<IClientResourceProcessor, ClientResourcePopulator>();
            container.RegisterPerWebRequest<IClientResourceProcessor, ClientResourceUrlProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, ClientResourceAppSettingProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, ClientResourceTokenProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, ClientResourceWebResourceProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, StylesheetResourceImageUrlResolver>();
            container.RegisterPerWebRequest<IClientResourceProcessor, ClientTemplateResourceProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, DependentResourceProcessor>();
            container.RegisterPerWebRequest<IClientResourceProcessor, RequireJsScriptModuleWrapper>();
            container.RegisterPerWebRequest<IClientResourceProcessor, JavaScriptMinifier>();


            container.RegisterPerWebRequest<IClientTemplateParser, DoTJsClientTemplateParser>();
            
            container.RegisterSingle<IClientSideObjectWriterFactory, ClientSideObjectWriterFactory>();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<ClientConfiguration, ClientConfigurationFactory>().InRequestScope();

            container.Register<IContentCache, WebContentCache>();

			container.RegisterSingle<IISVersion>(() => container.GetInstance<HttpRequestBase>().GetIISVersion());

            container.RegisterPerWebRequest<ITokenRegistry, TokenRegistry>();

			// TODO: IOC: Update with SimpleInjector Equivalent
            //BindToFactory<IResourceTextManager, ResourceTextManagerFactory>().InThreadScope();

            container.RegisterPerWebRequest<ISearchService, SearchService>();

            container.RegisterPerWebRequest<IContentSearchService, ContentSearchService>();
            container.RegisterPerWebRequest<IAlertSearchService, AlertHeadlineManager>();

			container.RegisterPerWebRequest<CultureInfo>(() =>
				CultureManager.GetCultureInfoFromInterfaceLanguage(
					container.GetInstance<IPreferences>().InterfaceLanguage));


			container.Register<ILog>(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));

            container.RegisterPerWebRequest<ISearchQueryResourceManager, SearchQueryResourceManager>();

            container.RegisterPerWebRequest<ISearchPreferenceService, SearchPreferenceService>();

			container.RegisterSingle<TaskFactory>(TaskFactoryManager.Instance.GetDefaultTaskFactory);

            container.RegisterPerWebRequest<ISocialMediaProvider, TweetRiverProvider>();
            container.RegisterPerWebRequest<IMultimediaManager, MultimediaManager>();
        }

		
    }
}
