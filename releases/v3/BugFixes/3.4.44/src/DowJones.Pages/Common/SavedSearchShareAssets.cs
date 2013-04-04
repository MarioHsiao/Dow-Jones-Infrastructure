using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Managers.PAL;
using DowJones.Session;

namespace DowJones.Pages.Common
{
    public class SavedSearchShareAssets : AbstractShareAssets<long>
    {
        private PALServiceManager palSvcMgr = null;
        public SavedSearchShareAssets(IControlData controlData)
        {
            var palPrefProv = new PALPreferenceServiceProvider(controlData);
            palSvcMgr = new PALServiceManager(palPrefProv);
        }

        public override void Share()
        {
            if (Assets == null || !Assets.Any())
                return;

            palSvcMgr.UpdateSavedSearchScope(Assets, Scope);
        }
    }
}
