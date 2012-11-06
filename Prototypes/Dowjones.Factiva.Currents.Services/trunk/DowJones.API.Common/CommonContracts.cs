using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DowJones.API.Common
{
    public interface IRequest
    {
        RequestFormat Format { get; set; }
     
    }

    public interface IServiceResponse
    {
        //string StatusMessage { get; set; }

        AuditLog AuditLog { get; set; }
        ARMValues ARMValues { get; set; }
    }

    public enum RequestFormat
    {
        Xml,
        Json,
        Soap
    }
}
