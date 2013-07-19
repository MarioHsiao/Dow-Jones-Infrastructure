namespace DowJones.Managers.Core
{
    public interface IPopulate<in TGetRequest> where TGetRequest : IRequest
    {
        void Populate(TGetRequest request);
    }
}