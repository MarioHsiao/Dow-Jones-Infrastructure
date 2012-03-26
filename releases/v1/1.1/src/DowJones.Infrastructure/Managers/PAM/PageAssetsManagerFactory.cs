using DowJones.Session;
using DowJones.Utilities.Managers;

namespace DowJones.Managers.PAM
{
    public class PageAssetsManagerFactory : IPageAssetsManagerFactory
    {
        private readonly IControlData _controlData;
        private readonly IPreferences _preferences;

        public PageAssetsManagerFactory(IControlData controlData, IPreferences preferences)
        {
            _controlData = controlData;
            _preferences = preferences;
        }

        public virtual IPageAssetsManager CreateManager()
        {
            var factivaControlData = ControlDataManager.Convert(_controlData);

            var manager = new PageAssetsManager(factivaControlData, _preferences.InterfaceLanguage);

            return manager;
        }
    }
}