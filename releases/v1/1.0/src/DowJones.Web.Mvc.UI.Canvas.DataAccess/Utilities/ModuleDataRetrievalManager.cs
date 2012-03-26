// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleDataRetrievalManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Session;
using DowJones.Utilities.Managers;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    internal class ModuleDataRetrievalManager : AbstractAggregationManager
    {
        private readonly ILog log = LogManager.GetLogger(typeof(ModuleDataRetrievalManager));

        public ModuleDataRetrievalManager(ControlData controlData, IPreferences preferences)
            : base(controlData)
        {
            InterfaceLanguage = preferences.InterfaceLanguage;
        }

        public ModuleDataRetrievalManager(string sessionID, string clientTypeCode, string accessPointCode, IPreferences preferences)
            : base(sessionID, clientTypeCode, accessPointCode)
        {
            InterfaceLanguage = preferences.InterfaceLanguage;
        }

        #region Overrides of AbstractAggregationManager

        public string InterfaceLanguage { get; set; }

        protected override ILog Log
        {
            get { return log; }
        }

        #endregion
    }
}
