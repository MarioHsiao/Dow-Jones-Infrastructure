namespace DowJones.Web.Mvc.UI.Components.Article
{
    public class EntityLinkData
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }

    public class PostProcessData
    {
        public DowJones.Infrastructure.PostProcessing Type { get; set; }
        public string ElinkValue {get;set;}
        public string ElinkText{get;set;}
    }
}
