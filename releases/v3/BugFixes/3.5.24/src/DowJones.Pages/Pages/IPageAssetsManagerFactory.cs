using DowJones.Preferences;
using DowJones.Session;

namespace DowJones.Pages
{
    public interface IPageAssetsManagerFactory
    {
        IPageAssetsManager Create();

        IPageAssetsManager CreateManager(IControlData controlData, string interfaceLanguage);
        IPageAssetsManager CreateManager(IControlData controlData, IPreferences preferences);

        IPageAssetsManager CreateManager(Factiva.Gateway.Utils.V1_0.ControlData controlData, string interfaceLanguage);
        IPageAssetsManager CreateManager(Factiva.Gateway.Utils.V1_0.ControlData controlData, IPreferences preferences);
    }
}