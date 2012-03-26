using DowJones.Web.EntitlementPrinciple.Security.Interfaces;

namespace DowJones.Web.EntitlementPrinciple.Security.Interfaces
{
    public interface IPrinciple
    {
        /// <summary>
        /// Gets UserServices.
        /// </summary>
        IUserSubPrinciple UserServices { get; }

        /// <summary>
        /// Gets CoreServices.
        /// </summary>
        ICoreServicesSubPrinciple CoreServices { get; }

        /// <summary>
        /// Gets RuleSet.
        /// </summary>
        IRuleSet RuleSet { get; }
    }
}