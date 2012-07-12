using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class OutletListTokens : AbstractTokenBase
    {
        public string ViewArticles { get; set; }
        public string NoArticles { get; set; }

        // Column names;
        // ******************************************
        // About;
        public string OutletName { get; set; }
        public string MediaContacts { get; set; }
        public string Articles { get; set; } // Articles<br />(last 90 days);
        public string Last90Days { get; set; }
        public string Circulation { get; set; }
        public string Type { get; set; }
        public string Website { get; set; }

        // Contact information;
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        // Editorial focus;
        public string Industries { get; set; }
        public string Subjects { get; set; }
        public string Regions { get; set; }
        // Additioanal metadata;
        public string Language { get; set; }
        public string Frequency { get; set; }
        public string Coverage { get; set; }
        public string Audience { get; set; }
        public string Publisher { get; set; }
        // User added information;
        public string UserAddedInfo { get; set; }

        // User Added Information Tokens
        public string UACity { get; set; }
        public string UACountry { get; set; }
        public string UAPhone { get; set; }
        public string UAMobile { get; set; }
        public string UAEmail { get; set; }

        // Action Menu Items;
        public string AddToContactList { get; set; }
        public string Print { get; set; }
        public string Export { get; set; }
        public string Delete { get; set; }
        public string Email { get; set; }
        public string ExportAll { get; set; }
        public string DistributeList { get; set; }
        public string EmailAll { get; set; }

        // Show/hide cell items;
        public string ShowCellItems { get; set; }
        public string HideCellItems { get; set; }

        // Unselect row checkboxes;
        public string UnselectCheckboxesConfirmTitle { get; set; }
        public string UnselectCheckboxesConfirmMessage { get; set; }
        public string UnselectCheckboxesPostMessage { get; set; }

        // Media contacts hints;
        public string ViewMediaContacts { get; set; }
        public string SendMailToMediaLab { get; set; }

        public string NoResults { get; set; }

        public OutletListTokens()
        {
            this.ViewArticles = GetTokenByName("viewArticles");
            this.NoArticles = GetTokenByName("noArticles");

            // Column names;
            // About;
            this.OutletName = GetTokenByName("outletName");
            this.MediaContacts = GetTokenByName("cmolMediaContacts");
            this.Articles = GetTokenByName("articles");
            this.Last90Days = GetTokenByName("last90Days");
            this.Circulation = GetTokenByName("circulation");
            this.Type = GetTokenByName("outletType");
            this.Website = GetTokenByName("cmolWebsite");
            // contact information;
            this.Address = GetTokenByName("cmolAddress");
            this.City = GetTokenByName("cmolCity");
            this.State = GetTokenByName("cmolState");
            this.Zip = GetTokenByName("cmolZipPost");
            this.Country = GetTokenByName("cmolCountry");
            this.EmailAddress = GetTokenByName("cmolEmail");
            this.Phone = GetTokenByName("cmolPhone");
            this.Fax = GetTokenByName("cmolFax");
            // editorial focus;
            this.Industries = GetTokenByName("cmmBtsIndustries");
            this.Subjects = GetTokenByName("cmmBtsSubjects");
            this.Regions = GetTokenByName("cmmBtsRegions");
            // additioanal metadata;
            this.Language = GetTokenByName("cmmOtlLanguage");
            this.Frequency = GetTokenByName("cmmOtlFrequency");
            this.Coverage = GetTokenByName("cmmOtlCoverage");
            this.Audience = GetTokenByName("cmolAudience");
            this.Publisher = GetTokenByName("cmmOtlPublisher");
            // user added info;
            this.UserAddedInfo = GetTokenByName("cmmUsrAddedInfo");

            // user added info fields;
            this.UACity = GetTokenByName("cmmUsrAddCity");
            this.UACountry = GetTokenByName("cmmUsrAddCountry");
            this.UAEmail = GetTokenByName("cmmUsrAddEmail");
            this.UAMobile = GetTokenByName("cmmUsrAddMobile");
            this.UAPhone = GetTokenByName("cmmUsrAddPhone");

            // action menu items;
            this.AddToContactList = GetTokenByName("addToOutletList");
            this.Print = GetTokenByName("cmPrint");
            this.Export = GetTokenByName("export");
            this.Delete = GetTokenByName("delete");
            this.Email = GetTokenByName("email");
            this.ExportAll = GetTokenByName("cmalExportAll");
            this.DistributeList = GetTokenByName("distributeList");
            this.EmailAll = GetTokenByName("cmalEmailAll");

            // show/hide cell items;
            this.ShowCellItems = GetTokenByName("cmalShowCellItems");
            this.HideCellItems = GetTokenByName("cmalHideCellItems");

            // unselect row checkboxes;
            this.UnselectCheckboxesConfirmTitle = GetTokenByName("cmalConfirm");
            this.UnselectCheckboxesConfirmMessage = GetTokenByName("cmalConfirmMessage");
            this.UnselectCheckboxesPostMessage = GetTokenByName("cmalSelectionsHaveBeenCleared ");

            // media contacts hints;
            this.ViewMediaContacts = GetTokenByName("cmolViewMediaContacts");
            this.SendMailToMediaLab = GetTokenByName("cmolSendMailToMediaLab");

            this.NoResults = GetTokenByName("noResults");
        }
    }
}