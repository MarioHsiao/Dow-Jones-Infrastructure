using System.Collections.Generic;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;

namespace DowJones.Ajax.AddressContact
{
    public class AddressContactData
    {
        public AddressContactData()
        {
            AdditionalCompanyLinks = new WebSiteCollection();
        }

        #region General company information

        public string CompanyName { get; set; }

        public Address Address { get; set; }

        // May need to break up phone/fax into phone/areacode/countrycode
        public string FormattedPhone { get; set; }
        public string FormattedFax { get; set; }

        public WebSite CompanyWebSite { get; set; }
        public WebSiteCollection AdditionalCompanyLinks { get; set; }

        #endregion

        #region Additional company info

        public string TradeName { get; set; }
        public int YearStarted { get; set; }
        public int EmployeesHere { get; set; }
        public string LegalStatusString { get; set; }
        /// <summary>
        /// Hoppenstedt registration , Credinform registration
        /// </summary>
        public string FilingOfficeName { get; set; }
        /// <summary>
        /// Hoppenstedt registration , Credinform registration
        /// </summary>
        public string RegistrationId { get; set; }
        /// <summary>
        /// CredInform registration inn
        /// </summary>
        public string Inn { get; set; }
        /// <summary>
        /// CredInform registration okpo
        /// </summary>
        public string Okpo { get; set; }

        public OwnershipDisplayType OwnershipType { get; set; }
        public string Duns { get; set; }
        public string AuditorName { get; set; }
        public List<BankDisplayDetails> BankDetails { get; set; }
        public IssueSplitCollection Splits { get; set; }
        
        public string CountryISOCode { get; set; }
        public string RegionCode { get; set; }

        #endregion

        #region Providers

        public Provider PrimaryCompanyProvider { get; set; }
        public Provider SecondaryCompanyProvider { get; set; }

        /// <summary>
        /// Indicates whether the primary company provider is delisted.  
        /// </summary>
        /// <remarks>
        /// Must show a "*" next to the company name if delisted and show a footnote.
        /// </remarks>
        public bool IsDelisted { get; set; }

        #endregion 

        public string LocationType { get; set; }

        public FamilyTreeDisplayInfo FamilyTreeInfo { get; set; }
        public FamilyTreeParentDisplayInfo GlobalUltimateParent { get; set; }
        public FamilyTreeParentDisplayInfo DomesticUltimateParent { get; set; }
        public FamilyTreeParentDisplayInfo ImmediateParent { get; set; }
    }

    public enum OwnershipDisplayType
    {
        Unspecified = 0,
        Listed,
        Unlisted,
    }

    public class FamilyTreeDisplayInfo
    {
        public string Fcode { get; set; }
        public string Duns { get; set; }
        public int NumberFamilyMembers { get; set; }
    }

    public class FamilyTreeParentDisplayInfo
    {
        public string Fcode { get; set; }
        public string Duns { get; set; }
        public bool IsCompanyReportAvailable { get; set; }
        public string Value { get; set; }
    }

    public class WebSiteCollection : List<WebSite>
    {
    }

    public class WebSite
    {
        public string Url { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class BankDisplayDetails
    {
        public string NameAndAddress { get; set; }
    }
}
