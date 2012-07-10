using DowJones.Mapping;
using GatewayShareScope = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareScope;

namespace DowJones.Pages.Mappers
{
	public class ShareScopeTypeMapper : TypeMapper<GatewayShareScope, ShareScope>
    {
		public override ShareScope Map(GatewayShareScope gatewayShareScope)
        {
			switch (gatewayShareScope)
            {
				case GatewayShareScope.Personal:
					return ShareScope.Personal;

				case GatewayShareScope.Everyone:
					return ShareScope.Everyone;

				case GatewayShareScope.AccountAdmin:
					return ShareScope.AccountAdmin;

				case GatewayShareScope.Account:
					return ShareScope.Account;
                
                default:
					return ShareScope.Personal;
            }
        }
    }
}
