using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Messages.Preferences.V1_0;

namespace DowJones.Security.Services
{
    /// <summary>
    /// The Membership service.
    /// </summary>
    public class MembershipService : AbstractService, IMembershipService
    {
        #region Private Variables

        /// <summary>
        /// The is on.
        /// </summary>
        private readonly bool _isOn;

        /// <summary>
        /// The main authorization component.
        /// </summary>
        private readonly MatrixMembershipService _matrixMembershipService;

        private readonly static Regex regex_NVPermission = new Regex(@"I\s*c=""(?<x>\d{3})""\s*g=""(?<y>\d*)", RegexOptions.Compiled);

        private Hashtable _ac6 = new Hashtable();
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="MembershipService"/> class.
        /// </summary>
        /// <param name="isMembershipCoreServiceOn"></param>
        /// <param name="matrixMembershipService"></param>
        internal MembershipService(bool isMembershipCoreServiceOn, MatrixMembershipService matrixMembershipService)
        {
            _isOn = isMembershipCoreServiceOn;
            _matrixMembershipService = matrixMembershipService;
            Initialize();
        }

        #endregion

        #region Public Properties

        #region Overrides of AbstractService

        /// <summary>
        /// Gets a value indicating whether IsOn.
        /// </summary>
        /// <value><c>true</c> if this instance is on; otherwise, <c>false</c>.</value>
        public override bool IsOn
        {
            get { return _isOn; }
        }

        /// <summary>
        /// Gets a value indicating whether HasOffset.
        /// </summary>
        public override bool HasOffset
        {
            get { return true; }
        }

        /// <summary>
        /// Gets Offset.
        /// </summary>
        public override int Offset
        {
            get { return 18; }
        }

        #endregion

        public  bool CanMakeFolderSubscribable { get; private set; }
        public string MembershipAC2 { get; private set; }
        public string MembershipAC3 { get; private set; }
        public string MembershipAC4 { get; private set; }
        public bool IsPersonalization { get; private set; }
        public string SharingDA { get; private set; }
        public string NewsletterDA { get; private set; }
        public bool IsTimeToLiveToken { get; private set; }
        public bool IsNewsViewsRenderOn { get; private set; }
        public bool IsNewsViewsAdministratorOn { get; private set; }
        

        #endregion

        #region Service Initialization Method

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override sealed void Initialize()
        {
            if(_matrixMembershipService != null)
            {
                CanMakeFolderSubscribable = _matrixMembershipService.ac1.ContainsAtAnyIndex("A");
                MembershipAC2 = _matrixMembershipService.ac2.FirstOrDefault(x => x.HasValue());
                MembershipAC3 = _matrixMembershipService.ac3.FirstOrDefault(x => x.HasValue());
                MembershipAC4 = _matrixMembershipService.ac4.FirstOrDefault(x => x.HasValue());

                // Parse membership.ac6
                string ac6Serialized = "<AC6>" + _matrixMembershipService.ac6.FirstOrDefault(x => x.HasValue()) + "</AC6>";
                _ac6.Clear();
                XmlSerializer ac6Serializer = new XmlSerializer(typeof(MembershipAC6));
                MembershipAC6 ac6 = (MembershipAC6)ac6Serializer.Deserialize(new StringReader(ac6Serialized));
                if (ac6.ITEM != null)
                {
                    foreach (MembershipAC6ITEM item in ac6.ITEM)
                    {
                        _ac6.Add((PreferenceClassID)item.@class, item.count);
                    }
                }
                IsPersonalization = _matrixMembershipService.personalization != "OFF";
                //TODO: verify
                SharingDA = _matrixMembershipService.sharingDA;
                NewsletterDA = _matrixMembershipService.newsletterDA;
                IsTimeToLiveToken = !string.IsNullOrEmpty(NewsletterDA) && (NewsletterDA.Trim().ToUpper() == "TTLT" || NewsletterDA.Trim().ToUpper() == "TTLTOVER") ? true : false;
                IsNewsViewsRenderOn = NewsViewsPermissions();
                IsNewsViewsAdministratorOn = IsNewsViewsRenderOn && (_matrixMembershipService.gripAdmin != null  && _matrixMembershipService.gripAdmin.Trim().ToUpper() == "Y");
            }
        }

        /// <remarks/>
        private bool NewsViewsPermissions()
        {
            bool bAccess = false;
            string strGroupNewsViews = _matrixMembershipService.ac5.FirstOrDefault(x => x.HasValue());
            string strGripDefault = _matrixMembershipService.gripDefault;
            if (string.IsNullOrEmpty(strGroupNewsViews))
            {
                // if AC5 value doesn't exist then use Grip Default values.
                if (!string.IsNullOrEmpty(strGripDefault))
                {
                    bAccess = CheckNewsViewsPermissions(strGripDefault);
                }
            }
            else
            {
                bAccess = CheckNewsViewsPermissions(strGroupNewsViews);
            }
            return bAccess;
        }

        private int GetPreferenceClassMaxItems(PreferenceClassID classId)
        {
            if (_ac6.Contains(classId))
            {
                return (int)_ac6[classId];
            }
            return 25;
        }

        private static bool CheckNewsViewsPermissions(string strAccess)
        {
            bool bFlag = false;
            Match m = regex_NVPermission.Match(strAccess);
            while (m.Success)
            {
                string strClassID = m.Groups["x"].ToString();
                string strCount = m.Groups["y"].ToString();
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
        #endregion
    }

    /// <summary>
    /// Summary description for AC6.
    /// </summary>
    [XmlRoot("AC6")]
    public class MembershipAC6
    {
        [XmlElement]
        public MembershipAC6ITEM[] ITEM;
    }

    public class MembershipAC6ITEM
    {
        [XmlAttribute("class")]
        public int @class;

        [XmlAttribute]
        public int count;
    }
}
