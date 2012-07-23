using DowJones.Extensions;
using DowJones.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Services
{
    public class CommunicatorService : AbstractService, ICommunicatorService
    {
        private readonly bool _isOn;
        private readonly AuthorizationComponent _matrix;
        private CommunicatorUserType _userType = CommunicatorUserType.Unspecified;

        public CommunicatorService(bool isCoreServiceOn, AuthorizationComponent matrix)
        {
            _matrix = matrix;
            _isOn = isCoreServiceOn;
            Initialize();
        }

        public override bool HasOffset
        {
            get { return true; }
        }

        public override int Offset
        {
            get { return 46; }
        }

        public override bool IsOn
        {
            get { return _isOn; }
        }

        #region ICommunicatorService Members

        public CommunicatorUserType UserType
        {
            get { return _userType; }
        }

        #endregion

        #region Service Initialization Method

        internal override sealed void Initialize()
        {
            if (_matrix == null)
            {
                return;
            }
            if (_matrix.ac1.ContainsAtAnyIndex("P"))
            {
                _userType = CommunicatorUserType.Platinum;
            }
            else if (_matrix.ac1.ContainsAtAnyIndex("G"))
            {
                _userType = CommunicatorUserType.Glod;
            }
            else if (_matrix.ac1.ContainsAtAnyIndex("S"))
            {
                _userType = CommunicatorUserType.Silver;
            }
        }

        #endregion
    }
}