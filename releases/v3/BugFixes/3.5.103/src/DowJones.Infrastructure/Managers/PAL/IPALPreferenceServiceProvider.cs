using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.PALService;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;

namespace DowJones.Managers.PAL
{
    public interface IPALPreferenceServiceProvider
    {
        void UpdatePreferenceItemScope(UpdateItemList[] item);
    }

}
