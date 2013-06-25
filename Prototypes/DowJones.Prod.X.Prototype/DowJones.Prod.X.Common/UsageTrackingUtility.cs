using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowJones.Extensions;

namespace DowJones.Prod.X.Common
{
    public class UsageTrackingUtility
    {
        /// <summary>
        /// function that checks whether Omniture data should be captured. It checks for the config entry
        /// as well as if this is an internal account.
        /// </summary>
        /// <returns></returns>
        public static bool LogUsageTrackingMetrics(bool logUsageTracking, string skipUsageTrackingAccounts, string account)
        {
            if (logUsageTracking)
            {
                //Also check for internal accounts
                if (skipUsageTrackingAccounts.IsNotEmpty() && account.IsNotEmpty())
                {
                    var internalAccounts = skipUsageTrackingAccounts.Split(',');
                    if (internalAccounts.Length > 0)
                    {
                        return internalAccounts.All(internalAccount => !account.StartsWith(internalAccount.Trim()));
                    }
                }
                return true;
            }
            return false;
        }
    }

}
