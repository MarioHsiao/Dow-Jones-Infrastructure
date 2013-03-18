namespace DowJones.Web.Mvc.Search.Requests.Common
{
    public class SourceFilterTypeTypeMapper
    {
        public static DowJones.Search.SourceFilterType Map(string sourceItemType)
        {
            switch ((sourceItemType ?? string.Empty).ToUpper())
            {
                case "BY":
                    return DowJones.Search.SourceFilterType.Byline;
                case "PDF":
                    return DowJones.Search.SourceFilterType.ProductDefineCode;
                case "RST":
                    return DowJones.Search.SourceFilterType.Restrictor;
                case "SC":
                    return DowJones.Search.SourceFilterType.SourceCode;
                case "SN":
                    return DowJones.Search.SourceFilterType.SourceName;
                default:
                    return default(DowJones.Search.SourceFilterType);
            }
        }

        public static string Map(DowJones.Search.SourceFilterType sourceItemType)
        {
            switch (sourceItemType)
            {
                case DowJones.Search.SourceFilterType.Byline:
                    return "BY";
                case DowJones.Search.SourceFilterType.ProductDefineCode:
                    return "PDF";
                case DowJones.Search.SourceFilterType.Restrictor:
                    return "RST";
                case DowJones.Search.SourceFilterType.SourceName:
                    return "SN";
                default:
                    return "SC";
            }
        }
    }
}