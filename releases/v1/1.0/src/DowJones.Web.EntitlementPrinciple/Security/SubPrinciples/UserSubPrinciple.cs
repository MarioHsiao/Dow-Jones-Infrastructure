// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserSubPrinciple.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the UserSubPrinciplecs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Web.EntitlementPrinciple.Security.Interfaces;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Web.EntitlementPrinciple.Security.SubPrinciples
{
    public class UserSubPrinciple : IUserSubPrinciple
    {
        public UserSubPrinciple(GetUserAuthorizationsResponse response)
        {
            IsGroupAdministrator = response.administratorFlag == AdministratorFlag.GroupAdministrator;
            IsAccountAdministrator = response.administratorFlag == AdministratorFlag.AccountAdministrator;
            IsMembershipAdministrator = IsAccountAdministrator || IsGroupAdministrator;

            UserId = response.UserId;
            AccountId = response.AccountId;
            ProductId = response.ProductId;

            IsCreditcardUser = response.UserType == UserType.Creditcard;
            IsCorporateUser = response.UserType == UserType.Corporate;
            IsCustomerServiceUser = response.UserType == UserType.CustomerService;
            IsIndividualUser = response.UserType == UserType.Individual;

            EmailLoginState = response.emailLoginState;
            EmailLoginConversionAllowed = response.emailLoginConversionAllowed;
            AutoLoginToken = response.AutoLoginToken;
            CustomerType = response.CustomerType;

            AutoLoginToken = response.AutoLoginToken;
            EmailAddress = response.EmailAddress;
            SessionId = response.SessionId;
            UserType = response.UserType;
            CustomerType = response.CustomerType;
            externalReaderFlag = response.externalReaderFlag;
            idleTimeout = response.idleTimeout;
            lwrFlag = response.lwrFlag;
            maxSession = response.maxSession;
            userStatus = response.userStatus;
        }

        public bool IsGroupAdministrator { get; private set; }

        public bool IsAccountAdministrator { get; private set; }

        public bool IsMembershipAdministrator { get; private set; }

        public bool IsCreditcardUser { get; private set; }

        public bool IsCorporateUser { get; private set; }

        public bool IsCustomerServiceUser { get; private set; }

        public bool IsIndividualUser { get; private set; }

        public string UserId { get; private set; }

        public string AccountId { get; private set; }

        public string ProductId { get; private set; }

        public EmailLoginState EmailLoginState { get; private set; }

        public EmailLoginConversionAllowed EmailLoginConversionAllowed { get; private set; }

        public string AutoLoginToken { get; private set; }
        
        public string EmailAddress { get; private set; }
        
        public string SessionId { get; private set; }
        
        public UserType UserType { get; private set; }

        public CustomerType CustomerType = CustomerType.Unspecified;

        public bool externalReaderFlag { get; private set; }
    
        public int idleTimeout { get; private set; }
    
        public string lwrFlag { get; private set; }
    
        public int maxSession { get; private set; }
    
        public UserStatus userStatus { get; private set; }
    
    }
}