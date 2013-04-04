using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Exceptions;
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

            var itemScopeResult =  service.UpdatePreferenceItemScope(item);
            if (itemScopeResult.Status != null && !String.IsNullOrEmpty(itemScopeResult.Status.Code))
            {
                long errorCode = 0;
                if (Int64.TryParse(itemScopeResult.Status.Code, out errorCode) && errorCode != 0 )
                    throw new DowJonesUtilitiesException(errorCode);
            }

        }

        void IDisposable.Dispose()
        {
            service.Dispose();
        }
    }
}
