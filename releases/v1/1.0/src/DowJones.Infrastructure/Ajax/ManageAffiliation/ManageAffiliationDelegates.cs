using DowJones.Tools.Ajax;

namespace DowJones.Utilities.Ajax.ManageAffiliation
{
    public enum ManageAffiliationAction
    {
        Update,
        Add,
        GetAffiliationId
    }
    public class ManageAffiliationRequestDelegate : IAjaxRequestDelegate
    {
        public ManageAffiliationAction Action;
        public string AffId;
        public string ExecCode;
        public string LastName;
        public string FirstName;
        public int Strength;
        public bool IsPrivate;
    }

    public class ManageAffiliationResponseDelegate : AbstractAjaxResponseDelegate
    {
        public string AffId = "";
        public int Strength = 3;
        public string LastName = "";
        public string FirstName = "";
        public bool IsPrivate;
    }
}
