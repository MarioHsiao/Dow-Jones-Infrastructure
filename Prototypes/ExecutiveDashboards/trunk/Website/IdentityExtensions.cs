using System;
using System.Configuration;
using System.Linq;
using System.Security.Principal;

namespace DowJones.Dash.Website
{
	public static class IdentityExtensions
	{
		public static bool IsAdmin(this IIdentity identity)
		{
			var adminSetting = ConfigurationManager.AppSettings.Get("security:admins");
			if (string.IsNullOrWhiteSpace(adminSetting))
				return false;

			var admins = adminSetting.Split(',');
			return admins.Any(a => a.Equals(identity.Name, StringComparison.CurrentCultureIgnoreCase));
		}
	}
}