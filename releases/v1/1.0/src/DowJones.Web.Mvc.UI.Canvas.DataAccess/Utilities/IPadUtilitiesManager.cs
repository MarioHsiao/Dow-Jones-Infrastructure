// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPadUtilitiesManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
// ReSharper disable InconsistentNaming
    public class IPadUtilitiesManager : AbstractAggregationManager
// ReSharper restore InconsistentNaming
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IPadUtilitiesManager));

        public IPadUtilitiesManager(ControlData controlData, IPreferences preferences) : base(controlData)
        {
            Preferences = preferences;
        }

        public IPadUtilitiesManager(IControlData controlData, IPreferences preferences) : this(ControlDataManager.Convert(controlData), preferences)
        {
        }

        public IPreferences Preferences { get; protected internal set; }

        #region Overrides of AbstractAggregationManager

        protected override ILog Log
        {
            get { return Logger; }
        }

        public string GetMobileLoginToken()
        {
            var request = new GetEncryptedCredentialsForCurrentSessionRequest();
            var response = Invoke<GetEncryptedCredentialsForCurrentSessionResponse>(request);

            if (response.rc != 0)
            {
                throw new DowJonesUtilitiesException();
            }

            var collection = new NameValueCollection
                                 {
                                     //// Agreed DateTime format display with the ipad team.
                                     { "expires", DateTime.Now.AddDays(2).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'") }, 
                                     { "encryptedCredentials", response.ObjectResponse.EncryptedCredentials },
                                     { "userid", ControlData.UserID },
                                     { "namespace", ControlData.ProductID },
                                 };
            var utility = new AESEncryptionUtility();
            return utility.Encrypt(collection);
        }

        #endregion
    }
}
