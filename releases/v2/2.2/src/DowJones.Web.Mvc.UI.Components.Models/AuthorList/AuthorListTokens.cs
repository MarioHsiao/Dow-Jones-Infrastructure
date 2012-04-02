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
		// NAMES & ARTICLES [3];
		// ******************************************
		public string ContactName { get; set; }
		public string Articles { get; set; } // Articles<br />(last 90 days);
		public string Last90Days { get; set; }
		// ******************************************
		// CONTACT INFO [9];
		// ******************************************
		public string EmailAddress { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string Zip { get; set; }
		public string Language { get; set; }
		// ******************************************
		// OUTLET INFO [8]
		// ******************************************
		public string OutletName { get; set; }
		public string OutletEmploymentType { get; set; }
		public string OutletJobTitle { get; set; }
		public string OutletType { get; set; }
		public string OutletCirculation { get; set; }
		public string OutletFrequency { get; set; }
		public string OutletState { get; set; }
		public string OutletCountry { get; set; }
		// ******************************************
		// BEATS [2]
		// ******************************************
		public string BeatsSubjects { get; set; }
		public string BeatsIndustries { get; set; }
		// ******************************************
		// USER INFO [1]
		// ******************************************
		public string UserAddedContactInfo { get; set; }

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
		public string ExportAll { get; set; }
		public string CreateActivity { get; set; }
		public string CreateBriefingBook { get; set; }

		// Show/hide cell items;
		public string ShowCellItems { get; set; }
		public string HideCellItems { get; set; }

		public string NoResults { get; set; }

		// Unselect row checkboxes;
		public string UnselectCheckboxesConfirmTitle { get; set; }
		public string UnselectCheckboxesConfirmMessage { get; set; }
		public string UnselectCheckboxesPostMessage { get; set; }

		public AuthorListTokens()
		{
			this.ViewArticles = GetTokenByName("viewArticles");
			this.NoArticles = GetTokenByName("noArticles");

			// Column names;
			// ******************************************
			// NAMES & ARTICLES [3];
			// ******************************************
			this.ContactName = GetTokenByName("contactName");
			this.Articles = GetTokenByName("articles");
			this.Last90Days = GetTokenByName("last90Days");
			// ******************************************
			// CONTACT INFO [9];
			// ******************************************
			this.EmailAddress = GetTokenByName("cmalEmailAddresses");
			this.Phone = GetTokenByName("cmalPhones");
			this.Fax = GetTokenByName("cmalFaxes");
			this.Address = GetTokenByName("cmalContactAddress");
			this.City = GetTokenByName("cmalCity");
			this.State = GetTokenByName("cmalState");
			this.Country = GetTokenByName("cmalCountry");
			this.Zip = GetTokenByName("cmalContactZipPost");
			this.Language = GetTokenByName("cmalContactLangs");
			// ******************************************
			// OUTLET INFO [8]
			// ******************************************
			this.OutletName = GetTokenByName("outletName");
			this.OutletEmploymentType = GetTokenByName("cmalEmploymentType");
			this.OutletJobTitle = GetTokenByName("cmalJobTitle");
			this.OutletType = GetTokenByName("cmalOutletType");
			this.OutletCirculation = GetTokenByName("circulation");
			this.OutletFrequency = GetTokenByName("cmalOutletFrequency");
			this.OutletState = GetTokenByName("cmalOutletState");
			this.OutletCountry = GetTokenByName("cmalOutletCountry");
			// ******************************************
			// BEATS [2]
			// ******************************************
			this.BeatsSubjects = GetTokenByName("cmalBeatsSubjects");
			this.BeatsIndustries = GetTokenByName("cmalBeatsIndustries");
			// ******************************************
			// USER INFO [1]
			// ******************************************
			this.UserAddedContactInfo = GetTokenByName("cmalUserAddedContactInfo");

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
			this.ExportAll = GetTokenByName("cmalExportAll");
			this.CreateActivity = GetTokenByName("cmCreateActivity");
			this.CreateBriefingBook = GetTokenByName("cmalCreateBriefingBook");

			// Show/hide cell items;
			this.ShowCellItems = GetTokenByName("cmalShowCellItems");
			this.HideCellItems = GetTokenByName("cmalHideCellItems");

			this.NoResults = GetTokenByName("noAuthorResults");

			// Unselect row checkboxes;
			this.UnselectCheckboxesConfirmTitle = GetTokenByName("cmalConfirm");
			this.UnselectCheckboxesConfirmMessage = GetTokenByName("cmalConfirmMessage");
			this.UnselectCheckboxesPostMessage = GetTokenByName("cmalSelectionsHaveBeenCleared ");

		}
	}
}