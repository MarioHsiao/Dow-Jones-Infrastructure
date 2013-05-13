using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Managers.PAM;
using DowJones.Session;

namespace DowJones.Pages.Common
{
    public class ChartItemShareAssets : AbstractShareAssets<long>
    {
        private ItemManager itemManager = null;
        public ChartItemShareAssets(IControlData controlData)
        {
            itemManager = new ItemManager(controlData, null);
        }
        public override void Share()
        {
            if (Assets == null || !Assets.Any())
                return;
            itemManager.SetShareProperties(Assets, Scope);
        }
    }
}
