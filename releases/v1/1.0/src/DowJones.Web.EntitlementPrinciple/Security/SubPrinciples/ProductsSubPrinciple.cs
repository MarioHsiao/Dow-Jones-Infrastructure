using Dow.Jones.Utility.Security.Interfaces;
using Dow.Jones.Utility.Security.Products;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;

namespace Dow.Jones.Utility.Security.SubPrinciples
{
    public class ProductsSubPrinciple : ISubPrinciple
    {
        internal ProductsSubPrinciple(CoreServicesSubPrinciple coreServices, RuleSet ruleSet, AuthorizationComponent authComponent)
        {
            IWorks = new IWorksProduct(ruleSet, coreServices, authComponent);
        }

        public IWorksProduct IWorks { get; private set; }
        public ReaderProduct Reader { get; private set; }
    }
}