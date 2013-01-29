using System;
using DowJones.Mapping;

namespace DowJones.Pages.Common
{
    public class FilterTypeToGWMapper : TypeMapper<FilterType, Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType>
    {
        public override Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType Map(FilterType source)
        {
            switch (source)
            {
                case FilterType.Company:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Company;
                case FilterType.Industry:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Industry;
                case FilterType.Keyword:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Keyword;
                case FilterType.Region:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Region;
                case FilterType.Topic:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Topic;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public class FilterTypeFromGWMapper : TypeMapper<Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType, FilterType>
    {
        public override FilterType Map(Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType source)
        {
            switch (source)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Company:
                    return FilterType.Company;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Industry:
                    return FilterType.Industry;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Keyword:
                    return FilterType.Keyword;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Region:
                    return FilterType.Region;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.FilterType.Topic:
                    return FilterType.Topic;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}