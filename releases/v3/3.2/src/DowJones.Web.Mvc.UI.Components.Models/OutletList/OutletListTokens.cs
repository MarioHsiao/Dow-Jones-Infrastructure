using DowJones.Token;

namespace DowJones.Web.Mvc.UI.Components.OutletList
{
    public class OutletListTokens : AbstractTokenBase
    {
        public string ViewArticles { get; set; }
        public string NoArticles { get; set; }

        // Column names;
        // ******************************************
        // 1. Outlet Name;
        public string OutletName { get; set; }
        // 2. Outlet Origin Country;
        public string OriginCountry { get; set; }
        // 3. Country;
        public string Country { get; set; }
        // 14-Feb-2012 new columns;
        // 4. State
        public string State { get; set; }
        // 5. City
        public string City { get; set; }

        // 6. Media format;
        public string MediaFormat { get; set; }
        // 7. Circulation;
        public string Circulation { get; set; }
        // 8. Outlet type
        public string OutletType { get; set; }
        // 9. Last modified date
        public string DateModified { get; set; }

        // New column names;
        // 10. Language;
        public string Language { get; set; }
        // 11. Frequency;
        public string Frequency { get; set; }
        // 12. Coverage;
        public string Coverage { get; set; }
        // 13. Publisher;
        public string Publisher { get; set; }
        // 14. Regions;
        public string Regions { get; set; }
        // 15. Subjects;
        public string Subjects { get; set; }
        // 16. Industries;
        public string Industries { get; set; }
        // 17. User added information
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
        // 12-Jan-2012: New Action Menu Items;
        public string ExportAll { get; set; }
        public string EmailAll { get; set; }
        public string UnselectAll { get; set; }

        public string NoResults { get; set; }

        public OutletListTokens()
        {
            this.ViewArticles = GetTokenByName("viewArticles");
            this.NoArticles = GetTokenByName("noArticles");

            // Column names;
            this.OutletName = GetTokenByName("outletName");
            this.OutletType = GetTokenByName("outletType");
            this.MediaFormat = GetTokenByName("mediaFormat");
            this.Circulation = GetTokenByName("circulation");
            // 15-Feb-2012 Yet another country;
            this.OriginCountry = GetTokenByName("cmalOutletOriginCountry");

            this.Country = GetTokenByName("countryName");
            // 14-Feb-2012 new columns;
            this.State = GetTokenByName("cmolState");
            this.City = GetTokenByName("cmolCity");

            this.DateModified = GetTokenByName("lastModifiedDate");
            //New column names
            this.Language = GetTokenByName("cmmOtlLanguage");
            this.Frequency = GetTokenByName("cmmOtlFrequency");
            this.Coverage = GetTokenByName("cmmOtlCoverage");
            this.Publisher = GetTokenByName("cmmOtlPublisher");
            this.Regions = GetTokenByName("cmmBtsRegions");
            this.Subjects = GetTokenByName("cmmBtsSubjects");
            this.Industries = GetTokenByName("cmmBtsIndustries");
            this.UserAddedInfo = GetTokenByName("cmmUsrAddedInfo");

            //User Added info
            this.UACity = GetTokenByName("cmmUsrAddCity");
            this.UACountry = GetTokenByName("cmmUsrAddCountry");
            this.UAEmail = GetTokenByName("cmmUsrAddEmail");
            this.UAMobile = GetTokenByName("cmmUsrAddMobile");
            this.UAPhone = GetTokenByName("cmmUsrAddPhone");

            // Action Menu Items;
            this.AddToContactList = GetTokenByName("addToOutletList");
            this.Print = GetTokenByName("cmPrint");
            this.Export = GetTokenByName("export");
            this.Delete = GetTokenByName("delete");
            this.Email = GetTokenByName("email");
            // 12-Jan-2012: New Action Menu Items;
            this.ExportAll = GetTokenByName("cmalExportAll");
            this.EmailAll = GetTokenByName("cmalEmailAll");
            this.UnselectAll = GetTokenByName("cmalUnselectAll");

            this.NoResults = GetTokenByName("noResults");
        }
    }
}