using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.PALService;
using DowJones.Session;
using DowJones.Utilities;
using ControlData = DowJones.PALService.ControlData;

namespace DowJones.Managers.PAL
{
    public class PALPreferenceServiceProvider : IPALPreferenceServiceProvider, IDisposable
    {
        private PreferenceItemScopeService service = null;

        public PALPreferenceServiceProvider(IControlData controlData)
        {
            service = ServiceFactory<PreferenceItemScopeService>.Create(controlData);
            
        }
        void IPALPreferenceServiceProvider.UpdatePreferenceItemScope(PALService.UpdateItemList[] item)
        {
            service.UpdatePreferenceItemScope(item);
        }

        void IDisposable.Dispose()
        {
            service.Dispose();
        }
    }
}
