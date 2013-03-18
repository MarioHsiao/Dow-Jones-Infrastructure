using System.Collections.Generic;

namespace DowJones.Web.ClientResources
{
    public interface IClientResourceRepository
    {
        IEnumerable<ClientResource> GetClientResources();
    }
}