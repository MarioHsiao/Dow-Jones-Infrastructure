using DowJones.Mapping;

namespace DowJones.Pages.Mappers
{
    public class AccessQualifierMapper
        : TypeMapper<Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier, AccessQualifier>
    {
        public override AccessQualifier Map(Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier accessQualifier)
        {
            switch (accessQualifier)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Account:
                    return AccessQualifier.Account;

                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Factiva:
                    return AccessQualifier.Global;

                default:
                    return AccessQualifier.User;
            }
        }
    }
}
