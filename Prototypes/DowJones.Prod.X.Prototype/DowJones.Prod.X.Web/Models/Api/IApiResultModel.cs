namespace DowJones.Newsletters.App.Web.Models.Api
{
    public interface IApiResultModel
    {
        long ReturnCode { get; }
        string ErrorMessage { get; }
    }
}