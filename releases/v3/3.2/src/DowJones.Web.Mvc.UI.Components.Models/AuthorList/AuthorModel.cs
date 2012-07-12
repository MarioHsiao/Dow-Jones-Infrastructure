using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc.UI.Components.OutletList;

namespace DowJones.Web.Mvc.UI.Components.AuthorList
{
    /// <summary>
    /// Class representing an model for author list.
    /// </summary>
    public class AuthorModel
    {
        /// <summary>
        /// Gets and sets author ID.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets and sets author NNID.
        /// </summary>
        public int AuthorNNID { get; set; }

        /// <summary>
        /// Gets and sets author name.
        /// </summary>
        public string AuthorName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string EmailAddresses { get; set; }
        public ProprietaryInfo UserInfo { get; set; }
        public string Phones { get; set; }
        public string JobTitle { get; set; }
        public string BeatsIndustries { get; set; }
        public string BeatsSubjects { get; set; }
        public string BeatsRegions { get; set; }
        public string RelatedMediaContacts { get; set; }

        /// <summary>
        /// Gets and sets author's prefered contact method.
        /// </summary>
        public ContactMethodProperties ContactMethod { get; set; }

        /// <summary>
        /// Gets and sets collection of outlets.
        /// </summary>
        public IEnumerable<OutletProperties> Outlets { get; set; }

        /// <summary>
        /// Gets whether the author have at least one outlet.
        /// </summary>
        public bool HasOutlets
        {
            get
            {
                if (this.Outlets == null || this.Outlets.Any() == false)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets whether the author have more than one outlet.
        /// </summary>
        public bool ExpandableOutlet
        {
            get
            {
                if (this.Outlets.Any() == false)
                    return false;
                else if (this.Outlets.Count() > 1)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Gets and sets whether an author has articles in last tree months.
        /// </summary>
        public bool HasArticles { get; set; }

        public AuthorModel()
        {
            this.Outlets = Enumerable.Empty<OutletProperties>();
        }

        public string CreateTextFromUserAddedContactInfo()
        {
            if (this.UserInfo == null)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            AuthorListTokens t = new AuthorListTokens();

            if (String.IsNullOrEmpty(this.UserInfo.City) == false)
            {
                sb.AppendFormat("{0}: {1}<br />", t.UACity, this.UserInfo.City);
            }

            if (String.IsNullOrEmpty(this.UserInfo.Country) == false)
            {
                sb.AppendFormat("{0}: {1}<br />", t.UACountry, this.UserInfo.Country);
            }

            if (String.IsNullOrEmpty(this.UserInfo.Phone) == false)
            {
                sb.AppendFormat("{0}: {1}<br />", t.UAPhone, this.UserInfo.Phone);
            }

            if (String.IsNullOrEmpty(this.UserInfo.Mobile) == false)
            {
                sb.AppendFormat("{0}: {1}<br />", t.UAMobile, this.UserInfo.Mobile);
            }

            if (String.IsNullOrEmpty(this.UserInfo.Email1) == false
                || String.IsNullOrEmpty(this.UserInfo.Email2) == false
                || String.IsNullOrEmpty(this.UserInfo.Email3) == false)
            {
                StringBuilder esb = new StringBuilder();
                if (String.IsNullOrEmpty(this.UserInfo.Email1) == false)
                {
                    esb.AppendFormat("{0}; ", this.UserInfo.Email1);
                }

                if (String.IsNullOrEmpty(this.UserInfo.Email2) == false)
                {
                    esb.AppendFormat("{0}; ", this.UserInfo.Email2);
                }

                if (String.IsNullOrEmpty(this.UserInfo.Email3) == false)
                {
                    esb.AppendFormat("{0}; ", this.UserInfo.Email3);
                }

                sb.AppendFormat("{0}: {1}", t.UAEmail, esb.ToString());
            }

            string retval = sb.ToString();
            if (retval.EndsWith("<br />"))
            {
                int pos = retval.LastIndexOf("<br />");
                retval.Remove(pos);
            }

            return retval;
        }
    }

    /// <summary>
    /// Class represents an outlet.
    /// Used in DowJones.Web.Mvc.UI.Components.AuthorList.AuthorModel class.
    /// </summary>
    public class OutletProperties
    {
        /// <summary>
        /// Gets and sets outlet ID.
        /// </summary>
        public int OutletId { get; set; }

        /// <summary>
        /// Gets and sets outlet name.
        /// </summary>
        public string OutletName { get; set; }

        /// <summary>
        /// Gets and sets outlet origin country.
        /// </summary>
        public string OutletOriginCountry { get; set; }

        /// <summary>
        /// Gets and sets outlet country.
        /// </summary>
        public string OutletCountry { get; set; }

        /// <summary>
        /// Gets and sets outlet state.
        /// </summary>
        public string OutletState { get; set; }

        /// <summary>
        /// Gets and sets outlet city.
        /// </summary>
        public string OutletCity { get; set; }

        /// <summary>
        /// Gets and sets outlet type.
        /// </summary>
        public OutletType Type { get; set; }

        public Frequency Frequency { get; set; }

        /// <summary>
        /// Gets and sets outlet circulation.
        /// </summary>
        public int Circulation { get; set; }

        /// <summary>
        /// Gets and sets employment type.
        /// </summary>
        public string EmploymentType { get; set; }

        /// <summary>
        /// Create an instanse of OutletProperties class.
        /// </summary>
        public OutletProperties()
        {
        }

        /// <summary>
        /// Create an instanse of ContactMethodProperties class and populate properties.
        /// </summary>
        /// <param name="outletId">Outlet ID.</param>
        /// <param name="outletName">Outlet name.</param>
        /// <param name="circulation">Outlet circulation.</param>
        public OutletProperties(
            int outletId,
            string outletName,
            string outletOriginCountry,
            string outletCountry,
            string outletState,
            string outletCity,
            OutletType type,
            Frequency frequency,
            int circulation,
            string employmentType)
        {
            this.OutletId = outletId;
            this.OutletName = outletName;
            this.OutletOriginCountry = outletOriginCountry;
            this.OutletCountry = outletCountry;
            this.OutletState = outletState;
            this.OutletCity = outletCity;
            this.Type = type;
            this.Frequency = frequency;
            this.Circulation = circulation;
            this.EmploymentType = employmentType;
        }
    }

    /// <summary>
    /// Class represents author's contact method.
    /// Used in DowJones.Web.Mvc.UI.Components.AuthorList.AuthorModel class.
    /// </summary>
    public class ContactMethodProperties
    {
        /// <summary>
        /// Gets and sets contact method type (eg. via e-mail, telephone...).
        /// </summary>
        public ContactMethodTypes Type { get; set; }

        /// <summary>
        /// Gets and sets contact (e.g. e-mail address, telephone number).
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Create an instanse of ContactMethodProperties class.
        /// </summary>
        public ContactMethodProperties()
        {
        }

        /// <summary>
        /// Create an instanse of ContactMethodProperties class and populate properties.
        /// </summary>
        /// <param name="type">Contact method type.</param>
        /// <param name="contact">Contact.</param>
        public ContactMethodProperties(ContactMethodTypes type, string contact)
        {
            this.Type = type;
            this.Contact = contact;
        }
    }

    /// <summary>
    /// Author's contact methods enumerator.
    /// Used in DowJones.Web.Mvc.UI.Components.AuthorList.ContactMethodProperties class.
    /// </summary>
    public enum ContactMethodTypes
    {
        /// <summary>
        /// No contact method
        /// </summary>
        None = 0,

        /// <summary>
        /// Email, Personal
        /// </summary>
        [Description("Email, Personal")]
        Email = 1,

        /// <summary>
        /// Email, Work
        /// </summary>
        [Description("Email, Work")]
        EmailWork = 2,

        /// <summary>
        /// Email, Company General
        /// </summary>
        [Description("Email, Company General")]
        EmailOutlet = 3,

        /// <summary>
        /// Telephone, Home
        /// </summary>
        [Description("Telephone, Home")]
        Phone = 4,

        /// <summary>
        /// Telephone, Work, Direct Dial
        /// </summary>
        [Description("Telephone, Work, Direct Dial")]
        PhoneWork = 5,

        /// <summary>
        /// Telephone, Office, Department
        /// </summary>
        [Description("Telephone, Office, Department")]
        PhoneOutlet = 6,

        /// <summary>
        /// Telephone, Office, General
        /// </summary>
        [Description("Telephone, Office, General")]
        PhoneOutlet2 = 7,

        /// <summary>
        /// Telephone, Mobile, Personal
        /// </summary>
        [Description("Telephone, Mobile, Personal")]
        Mobile = 8,

        /// <summary>
        /// Telephone, Mobile, Corporate
        /// </summary>
        [Description("Telephone, Mobile, Corporate")]
        MobileOutlet = 9,

        /// <summary>
        /// Fax, Home
        /// </summary>
        [Description("Fax, Home")]
        Fax = 10,

        /// <summary>
        /// Fax, Corporate, Direct
        /// </summary>
        [Description("Fax, Corporate, Direct")]
        FaxWork = 11,

        /// <summary>
        /// Fax, Corporate, General
        /// </summary>
        [Description("Fax, Corporate, General")]
        FaxOutlet = 12,

        /// <summary>
        /// IM - AOL
        /// </summary>
        [Description("IM - AOL")]
        IMAOL = 13,

        /// <summary>
        /// IM - MSN
        /// </summary>
        [Description("IM - MSN")]
        IMMSN = 14,

        /// <summary>
        /// IM - Reuters
        /// </summary>
        [Description("IM - Reuters")]
        IMReuters = 15,

        /// <summary>
        /// IM - Yahoo
        /// </summary>
        [Description("IM - Yahoo")]
        IMYahoo = 16,

        /// <summary>
        /// IM/VoIP - Skype
        /// </summary>
        [Description("IM/VoIP - Skype")]
        IMVoipSkype = 17,

        /// <summary>
        /// IM/VoIP - Google Talk
        /// </summary>
        [Description("IM/VoIP - Google Talk")]
        IMVoipGoogleTalk = 18
    }
}