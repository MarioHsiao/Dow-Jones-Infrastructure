using DowJones.Mapping;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages.Mappers
{
	public class AccessScopeShareTypeMapper : TypeMapper<ShareType, AccessScope>
	{
		public override AccessScope Map(ShareType shareType)
		{
			switch (shareType)
			{
				case ShareType.Personal:
					return AccessScope.OwnedByUser;

				case ShareType.Assigned:
					return AccessScope.AssignedToUser;

				case ShareType.Subscribed:
					return AccessScope.SubscribedByUser;

				default:
					return AccessScope.OwnedByUser;
			}
		}
	}
}
