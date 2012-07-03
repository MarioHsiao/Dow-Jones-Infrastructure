using System.Collections.Generic;

namespace DowJones.Managers.Core
{
    public interface IServicePartResults<out TServicePartResult, TPackage> : IServiceResult
        where TServicePartResult : IServicePartResult<TPackage>
        where TPackage : IPackage
    {
        IEnumerable<TServicePartResult> PartResults
        {
            get;
        }

        int MaxPartsAvailable { get; set; }
    }
}