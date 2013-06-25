using DowJones.Security.Interfaces;

namespace DowJones.Prod.X.Common.Extentions
{
    public static class PrincipleExtentions
    {
        public static bool HasAccessToProductX(this IPrinciple principle)
        {
            var isPersonalization = principle.CoreServices.MembershipService.IsPersonalization;

            var isWmMembershipAdmin = principle.UserServices.IsAccountAdministrator ||
                                      principle.UserServices.IsGroupAdministrator;

            return true;

            /* return principle.CoreServices.InterfaceService.IsInterfaceAC5On &&
                  !principle.CoreServices.AlertsService.IsSelectFullUser &&
                  !principle.CoreServices.AlertsService.IsSelectHeadlinesUser &&
                  !principle.CoreServices.InterfaceService.IsWMSegmentationPrdFA &&
                  (isWmMembershipAdmin || (!principle.CoreServices.InterfaceService.IsAcademicUser && !principle.UserServices.IsCreditcardUser && isPersonalization));*/
        }

        public static bool IsAccountAdministrator(this IPrinciple principle)
        {
            return principle.UserServices.IsAccountAdministrator;
        } 
    }
}