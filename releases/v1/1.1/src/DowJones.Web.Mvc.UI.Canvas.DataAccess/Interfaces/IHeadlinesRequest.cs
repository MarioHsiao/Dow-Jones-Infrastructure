namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IHeadlinesRequest : IRequest
    {
        string SearchContextRef { get; set; }

        int FirstResultToReturn { get; set; }

        int MaxResultsToReturn { get; set; }
    }
}
