using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Security.Interfaces
{
    public interface ICommunicatorService
    {
        CommunicatorUserType UserType { get; }
    }

    public enum CommunicatorUserType
    {
        Unspecified,
        Silver,
        Glod,
        Platinum
    }
}
