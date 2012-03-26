// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProxyManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Utilities.Configuration;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Utilities
{
    public interface IProxyManager
    {
        LightWeightUser GetProxyUser();
    }

    public class SnapshotProxyUserManager : IProxyManager
    {
        private static readonly SnapshotProxyUserManager HiddenInstance = new SnapshotProxyUserManager();
        private readonly List<LightWeightUser> proxyUsers = new List<LightWeightUser>();
        private const int MaxNumberOfProxies = 10;
        private int index;
        private object syncObject = new object();
       
        #region Implementation of IProxyManager

        protected SnapshotProxyUserManager()
        {
            for (var i = 0; i < MaxNumberOfProxies; i++)
            {
                proxyUsers.Add(new LightWeightUser
                {
                    userId = "snapproxy" + i,
                    userPassword = "pa55w0rd",
                    productId = "16",
                    accessPointCode = "SC",
                    accessPointCodeUsage = "SC",
                    clientCodeType = "D",
                });
            }
        }

        public static SnapshotProxyUserManager Instance
        {
            get { return HiddenInstance; }
        }

        public LightWeightUser GetProxyUser()
        {
            if (proxyUsers.Count == 0)
            {
                throw new NullReferenceException("proxyUsers are empty");
            }

            if (index >= proxyUsers.Count)
            {
                index = 0;
            }

            var user = proxyUsers[index];
            index++;
            return user;
        }

        #endregion
    }
}
