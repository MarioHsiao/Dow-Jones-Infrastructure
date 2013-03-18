namespace DowJones.Web.Mvc.UI
{
    public class CompositeComponentModel : ViewComponentModel
    {
        [ClientProperty("dataServiceUrl")]
        public string DataServiceUrl { get; set; }

        [ClientProperty]
        public virtual bool IsClientMode { get; set; }

        public CompositeComponentModel()
        {
            // Pretty much all of our components are client-only at this point (6/24/11)
            // When/if this changes, this can go away or change
            IsClientMode = true;
        }
    }
}