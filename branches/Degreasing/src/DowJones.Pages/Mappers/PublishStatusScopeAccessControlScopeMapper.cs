using DowJones.Mapping;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages.Mappers
{
    public class PublishStatusScopeAccessControlScopeMapper : TypeMapper<AccessControlScope, PublishStatusScope>
    {
        public override PublishStatusScope Map(AccessControlScope accessControlScope)
        {
            switch (accessControlScope)
            {
                case AccessControlScope.Account:
                    return PublishStatusScope.Account;

                case AccessControlScope.Everyone:
                    return PublishStatusScope.Global;
            }
            return PublishStatusScope.Personal;
        }
    }
}
