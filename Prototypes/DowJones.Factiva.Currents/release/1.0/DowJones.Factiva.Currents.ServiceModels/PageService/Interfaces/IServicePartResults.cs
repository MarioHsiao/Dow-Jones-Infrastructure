using System.Collections.Generic;

namespace DowJones.Factiva.Currents.ServiceModels.PageService.Interfaces
{
    public interface IServicePartResults<TServicePartResult, TPackage> : IServiceResult
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {
		IEnumerable<TServicePartResult> PartResults { get; set; }

        int MaxPartsAvailable { get; set; }
    }
}