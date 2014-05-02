using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;

namespace DowJones.Security
{
    public interface IUserContext
    {

        IControlData ControlData { get; }

        IPreferences Preferences { get; }

        IPrinciple Principle { get; }

        bool IsAuthenticated();

        bool IsValid();

    }
}
