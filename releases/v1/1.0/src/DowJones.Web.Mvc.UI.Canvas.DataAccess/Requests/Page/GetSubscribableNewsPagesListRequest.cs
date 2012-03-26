using System.Collections.Generic;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page
{
    [DataContract(Name = "accessQualifier", Namespace = "")]
    public enum AccessQualifier
    {
        [EnumMember] User,
        [EnumMember] Account,
        [EnumMember] Factiva
    }

    [DataContract(Name = "filterType", Namespace = "")]
    public enum FilterType
    {
        [EnumMember] Unspecified,
        [EnumMember] Author,
        [EnumMember] Category,
        [EnumMember] Company,
        [EnumMember] Executive,
        [EnumMember] Industry,
        [EnumMember] NewsSubject,
        [EnumMember] Region,
        [EnumMember] Source,
        [EnumMember] Topic
    }

    public class SubscribableNewsPagesListRequest : IPageRequest
    {
        public List<AccessQualifier> AccessQualifiers = new List<AccessQualifier>();

        public FilterType FilterType = FilterType.Unspecified;

        public string FilterValue;

        public string PageId { get; set; }

        public bool IsValid()
        {
            if (AccessQualifiers.Count == 0)
            {
                return false;
            }

            return string.IsNullOrEmpty(FilterValue) || FilterType != FilterType.Unspecified;
        }
    }
}
