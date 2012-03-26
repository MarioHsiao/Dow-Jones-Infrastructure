using System;
using System.Text.RegularExpressions;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Utilities.Security
{
    /// <remarks/>
    internal class NewsViewsPermissions
    {
        private readonly bool _bAccess;
        private readonly string _strNewsViewsAdmin;
        private readonly AuthorizationMatrix _authorization;
        private readonly static Regex NewsViewsPermissionRegex = new Regex(@"I\s*c=""(?<x>\d{3})""\s*g=""(?<y>\d*)", RegexOptions.Compiled);

        /// <remarks/>
        public NewsViewsPermissions(GetUserAuthorizationsResponse auth)
        {
            _authorization = auth.AuthorizationMatrix;
            var strGroupNewsViews = _authorization.Membership.ac5;
            _strNewsViewsAdmin = _authorization.Membership.gripAdmin;
            string strGripDefault = _authorization.Membership.gripDefault;
            _bAccess = CheckNewsViewsPermissions(strGripDefault);
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
                _bAccess = CheckNewsViewsPermissions(view);
                break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        public NewsViewsPermissions(AuthorizationMatrix authorization)
        {
            var strGroupNewsViews = authorization.Membership.ac5;
            _strNewsViewsAdmin = authorization.Membership.gripAdmin;
            var strGripDefault = authorization.Membership.gripDefault;
            _bAccess = CheckNewsViewsPermissions(strGripDefault);
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
                _bAccess = CheckNewsViewsPermissions(view);
                break;
            }
        }

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

        /// <remarks/>
        public bool IsNewsViewsRenderOn
        {
            get { return _bAccess; }
        }

        /// <remarks/>
        public bool IsNewsViewsAdministratorOn
        {
            get
            {
                return _bAccess && _strNewsViewsAdmin == "Y";
            }
        }
    }
}
