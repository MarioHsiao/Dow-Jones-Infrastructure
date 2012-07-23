using System.Collections.Generic;

namespace DowJones.OperationalData
{
    public interface IBaseOperationalData
    {
        IDictionary<string, string> GetKeyValues { get; }
    }
}