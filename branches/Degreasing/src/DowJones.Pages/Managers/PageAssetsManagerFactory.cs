using DowJones.DependencyInjection;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Session;

namespace DowJones.Pages
{
    public abstract class PageAssetsManagerFactory : Factory<IPageAssetsManager>, IPageAssetsManagerFactory
    {
        public override IPageAssetsManager Create()
        {
            var controlData = ServiceLocator.Resolve<IControlData>();
            var preferences = ServiceLocator.Resolve<IPreferences>();

            return CreateManager(controlData, preferences);
        }

        public IPageAssetsManager CreateManager(IControlData controlData, string interfaceLanguage)
        {
            var manager = CreateManager(controlData, new Preferences.Preferences(interfaceLanguage));
            return manager;
        }

        public IPageAssetsManager CreateManager(Factiva.Gateway.Utils.V1_0.ControlData controlData, string interfaceLanguage)
        {
            var manager = CreateManager(ControlDataManager.Convert(controlData), interfaceLanguage);
            return manager;
        }

        public IPageAssetsManager CreateManager(Factiva.Gateway.Utils.V1_0.ControlData controlData, IPreferences preferences)
        {
            var manager = CreateManager(ControlDataManager.Convert(controlData), preferences);
            return manager;
        }

        public abstract IPageAssetsManager CreateManager(IControlData controlData, IPreferences preferences);
    }
}