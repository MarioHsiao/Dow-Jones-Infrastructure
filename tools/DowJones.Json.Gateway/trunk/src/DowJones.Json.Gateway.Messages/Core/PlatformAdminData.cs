using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Messages.Core
{
    public class PlatformAdminData : AbstractJsonSerializable, IPlatformAdminData
    {
        public int? TransactionTimeout { get; set; }
    }
}
