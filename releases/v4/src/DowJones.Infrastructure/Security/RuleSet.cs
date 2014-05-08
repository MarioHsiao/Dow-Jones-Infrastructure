// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleSet.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Class representation of the RuleSet object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using DowJones.Security.Interfaces;

namespace DowJones.Security
{
    /// <summary>
    /// Class representation of the RuleSet object
    /// </summary>
    public class RuleSet : IRuleSet
    {
        private readonly string _ruleSet;

        public RuleSet(string ruleSet)
        {
            _ruleSet = ruleSet.Trim();
          
            IsArchiveCoreServiceOn = IsOffsetOn(0);
            IsCibsCoreServiceOn = IsOffsetOn(2);
            IsTrackCoreServiceOn = IsOffsetOn(4);
            IsEmailCoreServiceOn = IsOffsetOn(6);
            IsIndexCoreServiceOn = IsOffsetOn(8);
            IsMdsCoreServiceOn = IsOffsetOn(10);
            IsNdsCoreServiceOn = IsOffsetOn(12);
            IsSymbologyServiceOn = IsOffsetOn(14);
            IsDotComUiInfoDisplayServiceOn = IsOffsetOn(16);
            IsMembershipCoreServiceOn = IsOffsetOn(18);
            IsSetMobileCookieOn = IsOffsetOn(18);
            IsUerCoreServiceOn = IsOffsetOn(20);
            IsDotComTrackDisplayServiceOn = IsOffsetOn(22);
            IsOffset22On = _ruleSet[22] != '0';
            IsOffset23On = _ruleSet[23] != '0';
            IsDotComNewsPageDisplayServiceOn = IsOffsetOn(24);
            IsDotComBriefcaseDisplayServiceOn = IsOffsetOn(26);
            IsDotComCompanyQuickSearchDisplayServiceOn = IsOffsetOn(28);
            IsDotComQuoteListDisplayServiceOn = IsOffsetOn(30);
            IsDotComCompanyScreeningDisplayServiceOn = IsOffsetOn(32);
            IsDotComCompanyListDisplayServiceOn = IsOffsetOn(34);
            IsDotComQuoteDisplayServiceOn = IsOffsetOn(36);
            IsDotComDisplayServiceOn = IsOffsetOn(38);
            IsDotComPreferencesDisplayServiceOn = IsOffsetOn(40);
            IsDotComChartingDisplayServiceOn = IsOffsetOn(44);
           
            IsInvestextGatewayOn = IsOffsetOn(50);
            IsSearchModulesOn = IsOffsetOn(52);
            IsFcpFdkOffsetOn = IsOffsetOn(60);
            IsDotComSavedSearchesDisplayServiceOn = IsOffsetOn(64);
            IsFreeTextIndexingDisplayServiceOn = IsOffsetOn(62);
            IsClientBillingDisplayServiceOn = IsOffsetOn(66);
            IsDotComIndustryReportsDisplayServiceOn = IsOffsetOn(78);
            IsInformationWorkerDisplayServiceOn = IsOffsetOn(82);
            IsInsightUser = IsOffsetOn(86);
            IsFcpDisplayServiceOn = IsOffsetOn(88);
            IsFcpCompanyTabDisplayServiceOn = IsOffsetOn(90);
            IsFcpIndustryTabDisplayServiceOn = IsOffsetOn(92);
            IsFcpExecutiveTabDisplayServiceOn = IsOffsetOn(94);
            IsGovernmentTabDisplayServiceOn = IsOffsetOn(96);
            IsOffSet100On = IsOffsetOn(100);
            IsMediaMonitor = IsOffsetOn(104);
            IsMRM = IsOffsetOn(106);
            IsInterfaceServiceOn = true;
            IsPAMServiceOn = IsOffsetOn(116);

            IsMigrationCoreServiceOn = false;  //Why and where this flag is used! 46 is for Communicator
            IsCommunicator = IsOffsetOn(46);
            IsDJX = IsOffsetOn(150);
        }

        public bool IsSearchModulesOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is investext gateway on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is investext gateway on; otherwise, <c>false</c>.
        /// </value>
        public bool IsInvestextGatewayOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP FDK offset on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP FDK offset on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFcpFdkOffsetOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is information worker display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is information worker display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsInformationWorkerDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM UI info display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM UI info display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComUiInfoDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is client billing display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is client billing display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsClientBillingDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is archive core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archive core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchiveCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is cibs core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is cibs core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsCibsCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is track core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is track core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsTrackCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is email core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is email core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmailCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is index core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is index core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsIndexCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is MDS core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is MDS core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsMdsCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is NDS core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is NDS core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsNdsCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is symbology service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is symbology service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsSymbologyServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is membership core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is membership core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsMembershipCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is uer core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is uer core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsUerCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is migration core service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is migration core service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsMigrationCoreServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM track display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM track display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComTrackDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM news page display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM news page display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComNewsPageDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company quick search display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company quick search display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComCompanyQuickSearchDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM quote list display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM quote list display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComQuoteListDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company screening display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company screening display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComCompanyScreeningDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM company list display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM company list display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComCompanyListDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM quote display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM quote display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComQuoteDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM preferences display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM preferences display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComPreferencesDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM charting display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM charting display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComChartingDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM saved searches display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM saved searches display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComSavedSearchesDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM industry reports display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM industry reports display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComIndustryReportsDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is offset22 on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is offset22 on; otherwise, <c>false</c>.
        /// </value>
        public bool IsOffset22On { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is interface service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is interface service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsInterfaceServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is offset23 on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is offset23 on; otherwise, <c>false</c>.
        /// </value>
        public bool IsOffset23On { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is dot COM briefcase display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dot COM briefcase display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsDotComBriefcaseDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is free text indexing display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is free text indexing display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFreeTextIndexingDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFcpDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP industry tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP industry tab display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFcpIndustryTabDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP company tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP company tab display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFcpCompanyTabDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is FCP executive tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is FCP executive tab display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsFcpExecutiveTabDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is government tab display service on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is government tab display service on; otherwise, <c>false</c>.
        /// </value>
        public bool IsGovernmentTabDisplayServiceOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is insight user.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is insight user; otherwise, <c>false</c>.
        /// </value>
        public bool IsInsightUser { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is set mobile cookie on.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is set mobile cookie on; otherwise, <c>false</c>.
        /// </value>
        public bool IsSetMobileCookieOn { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOffSet100On { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsMediaMonitor { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsMRM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPAMServiceOn { get; private set; }

        /// <summary>
        /// Communicator product (MADE)
        /// </summary>
        public bool IsCommunicator { get; private set; }

        public bool IsDJX { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="offset">
        /// Offset value starts from zero
        /// </param>
        /// <returns>
        ///  <c>true</c> if offset is on; otherwise, <c>false</c>.
        /// </returns>
        public bool IsOffsetOn(int offset)
        {
            if (_ruleSet != null)
            {
                if (_ruleSet.Length >= (offset + 2))
                {
                    return _ruleSet.Substring(offset, 2) != "00";
                }
            }

            return false;
        }
    }
}