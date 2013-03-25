using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Managers.Alert;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using GWShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;

namespace DowJones.Pages.Common
{
    public class AlertShareAssets : AbstractShareAssets<int>
    {
        private AlertManager alertManager = null;
        public AlertShareAssets(IControlData controlData)
        {
            alertManager = new AlertManager(controlData, null, null);
        }

        public override void Share()
        {
            if (Assets == null || !Assets.Any())
                return;
            var request = new SetFolderSharePropertiesRequest
            {
                folderShareDetails = Assets.Select(alertId => new FolderShareDetails
                {
                    folderId = alertId,
                    SharingData = new Factiva.Gateway.Messages.Assets.V1_0.ShareProperties
                    {
                        accessControlScope = ShareAccessScope.Everyone,
                        allowCopy = false,
                        assignedScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                        externalAccess = ShareAccess.Allow,
                        listingScope = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal,
                        sharePromotion = Factiva.Gateway.Messages.Assets.V1_0.ShareScope.Personal
                    }
                }).ToArray()
            };

            alertManager.SetFolderShareProperties(request);
        }
      
    }
}
