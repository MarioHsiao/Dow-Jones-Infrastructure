using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DowJones.Managers.Search.MetaDataSearch;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.Managers;

namespace DowJones.Infrastructure.Web.Search
{
    public class FIICodeInfoUtilityTest : UnitTestFixture
    {
        public void GetFIICodes_Test()
        {
            ControlData controlData = ControlDataManager.GetLightWeightUserControlData("apichecker", "apichecker", "16");
            FIICodeInfoUtility utility = new FIICodeInfoUtility();
            List<FIICodeInfo> fiicodes = utility.GetFIICodesInfo(controlData, new List<string> { "ibm" }, new List<string> { "iacc", "asiaz", "india" }, new List<string> { "iadv" }, null, "en");
        }
    }
}
