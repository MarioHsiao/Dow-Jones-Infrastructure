using DowJones.Mapping;
using GWShareType = Factiva.Gateway.Messages.Assets.Pages.V1_0.ShareType;

namespace DowJones.Pages.Mappers
{
	public class ShareTypeMapper : TypeMapper<GWShareType, ShareType>
	{
		public override ShareType Map(GWShareType shareType)
		{
			switch (shareType)
			{
				case GWShareType.Personal:
					return ShareType.OwnedByUser;

				case GWShareType.Assigned:
					return ShareType.AssignedToUser;

				case GWShareType.Subscribed:
					return ShareType.SubscribedByUser;

				default:
					return ShareType.OwnedByUser;
			}
		}
	}
}
