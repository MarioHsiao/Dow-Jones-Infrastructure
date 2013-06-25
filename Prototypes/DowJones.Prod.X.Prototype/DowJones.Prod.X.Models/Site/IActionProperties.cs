namespace DowJones.Prod.X.Models.Site
{
    public interface IActionProperties
    {
        string Description { get; set; }
        string Title { get; set; }
        string BodyClassName { get; set; }
        bool IncludeFooter { get; set; }
    }
}