using System;
using System.Globalization;
using System.Web;
using DowJones.Core;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Managers.PAM;
using DowJones.Properties;
using DowJones.Session;
using DowJones.Token;
using DowJones.Utilities.Managers.Core;
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
            Bind<IClientResourceManager>()
                .ToMethod(x => x.Kernel.Get<IClientResourceManagerFactory>().Create())
                .InSingletonScope();

            Bind<IClientResourceManagerFactory>().To<ClientResourceManagerFactory>();

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
                .ToMethod(x => Kernel.Get<HttpRequestBase>().GetIISVersion())
                .InSingletonScope();

            Bind<IPageAssetsManagerFactory>()
                .To<PageAssetsManagerFactory>()
                .InRequestScope();

            Bind<IPageAssetsManager>()
                .ToMethod(x => Kernel.Get<IPageAssetsManagerFactory>().CreateManager())
                .InRequestScope();

            Bind<ITokenRegistry>().To<TokenRegistry>().InRequestScope();

            Bind<IResourceTextManager>()
                .ToMethod(x => ResourceTextManager.Instance)
                .InSingletonScope();

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
        }
    }
}
