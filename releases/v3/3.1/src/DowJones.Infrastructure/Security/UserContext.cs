using DowJones.Preferences;
using DowJones.Security.Interfaces;
using DowJones.Session;

namespace DowJones.Security
{
    public class UserContext : IUserContext
    {
        private readonly IControlData controlData;
        private readonly IPreferences preferences;
        private readonly IPrinciple principle;

        public IControlData ControlData
        {
            get { return controlData; }
        }

        public IPreferences Preferences
        {
            get { return preferences; }
        }

        public IPrinciple Principle
        {
            get { return principle; }
        }


        public UserContext(IControlData controlData, IPreferences preferences, IPrinciple principle)
        {
            this.controlData = controlData;
            this.principle = principle;
            this.preferences = preferences;
        }


        public virtual bool IsAuthenticated()
        {
            return IsValid() && controlData.IsValid();
        }

        public bool IsValid()
        {
            return controlData != null
                && preferences != null
                && principle != null;
        }
    }
}