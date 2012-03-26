using System.Collections.Generic;
using DowJones.Tools.Ajax;

namespace DowJones.Utilities.Ajax.CompanySearch
{
    public class Company
    {
        public string code;
        public string name;
    }

    public class CompanySearchRequestDelegate : IAjaxRequestDelegate
    {
        public string SearchString;
    }

    public class CompanySearchResponseDelegate : AbstractAjaxResponseDelegate
    {
        public List<Company> Companies = new List<Company>();
    }
}