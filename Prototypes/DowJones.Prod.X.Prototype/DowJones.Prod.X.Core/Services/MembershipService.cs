using System;
using DowJones.Loggers;
using DowJones.Managers.Abstract;
using DowJones.Prod.X.Core.Interfaces;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using log4net;

namespace DowJones.Prod.X.Core.Services
{
    public class MembershipService : AbstractAggregationManager, IMembershipService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MembershipService));

        public long LastError { get; set; }

        public MembershipService(IControlData controlData, ITransactionTimer transactionTimer)
            : base(controlData, transactionTimer)
        {
        }

        public bool IsSessionValid()
        {

            if (String.IsNullOrWhiteSpace(ControlData.SessionID))
            {
                return false;
            }
            var cdata = new Factiva.Gateway.Utils.V1_0.ControlData { SessionID = ControlData.SessionID };
            var valRequest = new ValidateSessionIdRequest();
            var sResponse = Factiva.Gateway.Services.V1_0.MembershipService.ValidateSessionId(cdata, valRequest);
            LastError = sResponse.rc;
            return sResponse.rc == 0;
        }

        protected override ILog Log
        {
            get { return _log; }
        }
    }
}
