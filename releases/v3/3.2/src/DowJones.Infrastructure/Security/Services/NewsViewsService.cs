// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewsViewsService.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the NewsViewsService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Text.RegularExpressions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    public class NewsViewsService : AbstractService, INewsViewsService
    {
        #region Private Variables

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly AuthorizationComponent component;

        /// <summary>
        /// The main authorization matrix.
        /// </summary>
        private readonly AuthorizationMatrix matrix;

        /// <summary>
        /// The service is on.
        /// </summary>
        private bool isOn;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsViewsService"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        /// <param name="authorizationMatrix">The authorization matrix.</param>
        internal NewsViewsService(RuleSet ruleSet, AuthorizationMatrix authorizationMatrix)
        {
            isOn = ruleSet.IsTrackCoreServiceOn;
            matrix = authorizationMatrix;
            component = authorizationMatrix.Track;
            Initialize();
        }
        
        #region Properties

        public sealed override bool HasOffset
        {
            get { return false; }
        }

        public override int Offset
        {
            get { return -1; }
        }

        public override bool IsOn
        {
            get
            {
                return isOn;
            }
        }

        #endregion

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal sealed override void Initialize()
        {
            var newsViewsPermissions = new NewsViewsPermissions(matrix);
            isOn = newsViewsPermissions.IsNewsViewsAdministratorOn;
        }
    }

    /// <summary>
    /// News Views Permissions Class.
    /// </summary>
    internal class NewsViewsPermissions
    {
        private readonly bool access;
        private readonly string newsViewsAdminStr;
        private readonly AuthorizationMatrix authorizationMatrix;
        private static readonly Regex NewsViewsPermissionRegex = new Regex(@"I\s*c=""(?<x>\d{3})""\s*g=""(?<y>\d*)", RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsViewsPermissions"/> class.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <remarks/>
        public NewsViewsPermissions(AuthorizationMatrix matrix)
        {
            authorizationMatrix = matrix;
            var strGroupNewsViews = authorizationMatrix.Membership.ac8;
            newsViewsAdminStr = authorizationMatrix.Membership.gripAdmin;
            var strGripDefault = authorizationMatrix.Membership.gripDefault;
            access = CheckNewsViewsPermissions(strGripDefault);
            if (strGroupNewsViews == null || strGroupNewsViews.Count <= 0)
            {
                return;
            }

            foreach (var view in strGroupNewsViews)
            {
                if (string.IsNullOrEmpty(strGripDefault))
                {
                    continue;
                }

                access = CheckNewsViewsPermissions(view);
                break;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is news views render on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is news views render on; otherwise, <c>false</c>.
        /// </value>
        public bool IsNewsViewsRenderOn
        {
            get { return access; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is news views administrator on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is news views administrator on; otherwise, <c>false</c>.
        /// </value>
        /// <remarks/>
        public bool IsNewsViewsAdministratorOn
        {
            get
            {
                return access && newsViewsAdminStr == "Y";
            }
        }

        /// <summary>
        /// Checks the news views permissions.
        /// </summary>
        /// <param name="strAccess">The STR access.</param>
        /// <returns><c>true</c> if this instance is news views permissions is on; otherwise, <c>false</c>.</returns>
        private static bool CheckNewsViewsPermissions(string strAccess)
        {
            var bFlag = false;
            var m = NewsViewsPermissionRegex.Match(strAccess);
            while (m.Success)
            {
                var strClassID = m.Groups["x"].ToString();
                var strCount = m.Groups["y"].ToString();

                // Class ID = 139 is the class ID for NewsViews class.
                if (strClassID.CompareTo("139") == 0)
                {
                    if (Convert.ToInt32(strCount) > 0)
                    {
                        bFlag = true;
                    }
                }

                m = m.NextMatch();
            }

            return bFlag;
        }
    }
}
