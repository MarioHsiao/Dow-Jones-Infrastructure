using System.Web;

namespace DowJones.Web.Mvc.UI.Components.RelatedConcepts
{
    public class RelatedConceptsComponentModel : CompositeComponentModel
    {
        private int maxNumberToReturn = 10;

        public RelatedConceptsComponentModel()
        {
            this.DataServiceUrl = VirtualPathUtility.ToAbsolute("~/search/related");
        }

        [ClientProperty("keywords")]
        public string Keywords
        {
            get;
            set;
        }

        [ClientProperty("maxNumberOfTerms")]
        public int MaxNumberOfTerms
        {
            get
            {
                return this.maxNumberToReturn;
            }

            set
            {
                if (value > 10 || value <= 0)
                {
                    return;
                }

                this.maxNumberToReturn = value;
            }
        }
    }
}