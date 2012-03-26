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
		// 4. Outlet Country of Origin;
		public string OutletOriginCountry { get; set; }
		// 5. Outlet Country;
		public string OutletCountry { get; set; }
		// 6. Outlet State;
		public string OutletState { get; set; }
		// 7. Outlet City;
		public string OutletCity { get; set; }
		// 8. Outlet Type;
		public string OutletType { get; set; }
		// 9. Outlet Frequency;
		public string OutletFrequency { get; set; }
		// 10. Circulation;
		public string Circulation { get; set; }
		// 11. Country;
		public string Country { get; set; }
		// 12. State
		public string State { get; set; }
		// 13. City
		public string City { get; set; }
		// 14. E-mail addresses;
		public string EmailAddresses { get; set; }
		// 15. User Added Contact Information;
		public string UserAddedContactInfo { get; set; }
		// 16. Phones;
		public string Phones { get; set; }
		// 17. Job Title;
		public string JobTitle { get; set; }
		// 18. Beats: Industries;
		public string BeatsIndustries { get; set; }
		// 19. Beats: Subjects;
		public string BeatsSubjects { get; set; }
		// 20. Beats: Regions;
		public string BeatsRegions { get; set; }
		// 21. Employment Type;
		public string EmploymentType { get; set; }
		// 22. Prefered Contact Method;
		public string PreferedContact { get; set; }
		public string Method { get; set; }
		// 23. Related Media Contacts;
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
		// 25-Jan-2012 New Action Menu Item;
		public string CreateActivity { get; set; }
		// 14-Feb-2012 New Action Menu Item;
		public string CreateBriefingBook { get; set; }

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
			// 14-Feb-2012 new columns;
			this.OutletOriginCountry = GetTokenByName("cmalOutletOriginCountry");
			this.OutletCountry = GetTokenByName("cmalOutletCountry");
			this.OutletState = GetTokenByName("cmalOutletState");
			this.OutletCity = GetTokenByName("cmalOutletCity");

			this.OutletType = GetTokenByName("cmalOutletType");
			this.OutletFrequency = GetTokenByName("cmalOutletFrequency");
			this.Circulation = GetTokenByName("circulation");
			this.Country = GetTokenByName("cmalCountry");
			// 14-Feb-2012 new columns;
			this.State = GetTokenByName("cmalState");
			this.City = GetTokenByName("cmalCity");

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
			// 25-Jan-2012: New Action Menu Item;
			this.CreateActivity = GetTokenByName("cmCreateActivity");
			// 14-Feb-2012 New Action Menu Item;
			this.CreateBriefingBook = GetTokenByName("cmalCreateBriefingBook");

			this.NoResults = GetTokenByName("noAuthorResults");
		}
	}
}