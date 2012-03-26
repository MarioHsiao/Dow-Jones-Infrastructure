using System;
using System.Collections.Generic;
using System.Text;

namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class BaseEditionOperationalData: AbstractOperationalData
    {
        public BaseEditionOperationalData(IDictionary<string, string> list)
            : base(list)
        {

        }
    }
}
