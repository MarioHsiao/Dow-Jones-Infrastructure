using System.Collections.Generic;

namespace DowJones.Utilities.OperationalData
{
    public interface IBaseOperationalData
    {
        IDictionary<string, string> GetKeyValues { get; }
    }
}