using System.Collections.Generic;

namespace DowJones.OperationalData.EntryPoint
{
    public class BaseEditionOperationalData: AbstractOperationalData
    {
        public BaseEditionOperationalData(IDictionary<string, string> list)
            : base(list)
        {

        }
    }
}
