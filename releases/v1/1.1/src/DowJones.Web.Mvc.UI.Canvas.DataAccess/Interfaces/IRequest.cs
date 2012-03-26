namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces
{
    public interface IRequest : IValidate
    {
    }

    public interface IValidate
    {
        bool IsValid();
    }
}
