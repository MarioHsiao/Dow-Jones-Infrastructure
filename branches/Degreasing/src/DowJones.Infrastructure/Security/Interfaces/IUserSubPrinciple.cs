using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace DowJones.Security.Interfaces
{
    public interface IUserSubPrinciple
    {
        bool IsGroupAdministrator { get; }
        bool IsAccountAdministrator { get; }
        bool IsMembershipAdministrator { get; }
        bool IsCreditcardUser { get; }
        bool IsCorporateUser { get; }
        bool IsCustomerServiceUser { get; }
        bool IsIndividualUser { get; }
        string UserId { get; }
        string AccountId { get; }
        string ProductId { get; }
        string PlanId { get; }
        EmailLoginState EmailLoginState { get; }
        EmailLoginConversionAllowed EmailLoginConversionAllowed { get; }
        string AutoLoginToken { get; }
        string EmailAddress { get; }
        string SessionId { get; }
        UserType UserType { get; }
        bool externalReaderFlag { get; }
        int idleTimeout { get; }
        string lwrFlag { get; }
        int maxSession { get; }
        UserStatus userStatus { get; }
    }
}