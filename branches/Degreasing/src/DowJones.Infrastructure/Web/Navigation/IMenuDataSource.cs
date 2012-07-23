namespace DowJones.Web.Navigation
{
    public interface IMenuDataSource
    {
        IMenu GetMenu(string menuID);
    }
}