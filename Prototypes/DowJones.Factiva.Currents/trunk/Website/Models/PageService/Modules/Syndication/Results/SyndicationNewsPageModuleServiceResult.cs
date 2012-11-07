﻿using System.Runtime.Serialization;
using DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Packages;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Factiva.Currents.Website.Models.PageService.Modules.Syndication.Results
{
    [DataContract(Name = "syndicationNewsPageModuleServiceResult", Namespace = "")]
    public class SyndicationNewsPageModuleServiceResult :
        AbstractModuleServiceResult<SyndicationNewsPageServicePartResult<SyndicationPackage>, SyndicationPackage, SyndicationNewspageModule>
    {
         }
}
