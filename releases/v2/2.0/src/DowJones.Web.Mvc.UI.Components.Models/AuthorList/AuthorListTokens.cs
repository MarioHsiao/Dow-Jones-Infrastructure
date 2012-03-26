using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class AuthorListTokens : AbstractTokenBase
	{
		public string ViewArticles { get; set; }
		public string NoArticles { get; set; }

		// Column names;
		// ******************************************
		// 1. Contact Name;
		public string ContactName { get; set; }
		// 2. Articles<br />(last 90 days);
		public string Articles { get; set; }
		public string Last90Days { get; set; }
		// 3. Outlet Name;
		public string OutletName { get; set; }
		// 4. Outlet Type;
		public string OutletType { get; set; }
		// 5. Outlet Frequency;
		public string OutletFrequency { get; set; }
		// 6. Circulation;
		public string Circulation { get; set; }
		// 7. Country;
		public string Country { get; set; }
		// 8. E-mail addresses;
		public string EmailAddresses { get; set; }
		// 9. User Added Contact Information;
		public string UserAddedContactInfo { get; set; }
		// 10. Phones;
		public string Phones { get; set; }
		// 11. Job Title;
		public string JobTitle { get; set; }
		// 12. Beats: Industries;
		public string BeatsIndustries { get; set; }
		// 13. Beats: Subjects;
		public string BeatsSubjects { get; set; }
		// 14. Beats: Regions;
		public string BeatsRegions { get; set; }
		// 15. Employment Type;
		public string EmploymentType { get; set; }
		// 16. Prefered Contact<br />Method;
		public string PreferedContact { get; set; }
		public string Method { get; set; }
		// 17. Related Media Contacts;
		public string RelatedMediaContacts { get; set; }

		// User Added Information Tokens;
		public string UACity { get; set; }
		public string UACountry { get; set; }
		public string UAPhone { get; set; }
		public string UAMobile { get; set; }
		public string UAEmail { get; set; }

		// Action Menu Items;
		public string AddToContactList { get; set; }
		public string CreateAlert { get; set; }
		public string Print { get; set; }
		public string Export { get; set; }
		public string Delete { get; set; }
		public string Email { get; set; }
		// 11-Jan-2012: New Action Menu Items;
		public string ExportAll { get; set; }
		public string EmailAll { get; set; }
		public string UnselectAll { get; set; }
		// 25-Jan-2012 New Action Menu Item
		public string CreateActivity { get; set; }

		public string NoResults { get; set; }

		public AuthorListTokens()
		{
			this.ViewArticles = GetTokenByName("viewArticles");
			this.NoArticles = GetTokenByName("noArticles");

			// Column names;
			this.ContactName = GetTokenByName("contactName");
			this.Articles = GetTokenByName("articles");
			this.Last90Days = GetTokenByName("last90Days");
			this.OutletName = GetTokenByName("outletName");
			this.OutletType = GetTokenByName("cmalOutletType");
			this.OutletFrequency = GetTokenByName("cmalOutletFrequency");
			this.Circulation = GetTokenByName("circulation");
			this.Country = GetTokenByName("cmalCountry");
			this.EmailAddresses = GetTokenByName("cmalEmailAddresses");
			this.UserAddedContactInfo = GetTokenByName("cmalUserAddedContactInfo");
			this.Phones = GetTokenByName("cmalPhones");
			this.JobTitle = GetTokenByName("cmalJobTitle");
			this.BeatsIndustries = GetTokenByName("cmalBeatsIndustries");
			this.BeatsSubjects = GetTokenByName("cmalBeatsSubjects");
			this.BeatsRegions = GetTokenByName("cmalBeatsRegions");
			this.EmploymentType = GetTokenByName("cmalEmploymentType");
			this.PreferedContact = GetTokenByName("preferedContact");
			this.Method = GetTokenByName("method");
			this.RelatedMediaContacts = GetTokenByName("cmalRelatedMediaContacts");

			// User Added info;
			this.UACity = GetTokenByName("cmmUsrAddCity");
			this.UACountry = GetTokenByName("cmmUsrAddCountry");
			this.UAEmail = GetTokenByName("cmmUsrAddEmail");
			this.UAMobile = GetTokenByName("cmmUsrAddMobile");
			this.UAPhone = GetTokenByName("cmmUsrAddPhone");

			// Action Menu Items;
			this.AddToContactList = GetTokenByName("addToContactList");
			this.CreateAlert = GetTokenByName("cmCreateAlert");
			this.Print = GetTokenByName("cmPrint");
			this.Export = GetTokenByName("export");
			this.Delete = GetTokenByName("delete");
			this.Email = GetTokenByName("cmalgmEmail");
			// 11-Jan-2012: New Action Menu Items;
			this.ExportAll = GetTokenByName("cmalExportAll");
			this.EmailAll = GetTokenByName("cmalEmailAll");
			this.UnselectAll = GetTokenByName("cmalUnselectAll");
			//25-Jan-2012: New Action Menu Item;
			this.CreateActivity = GetTokenByName("cmCreateActivity");

			this.NoResults = GetTokenByName("noAuthorResults");
		}
	}
}