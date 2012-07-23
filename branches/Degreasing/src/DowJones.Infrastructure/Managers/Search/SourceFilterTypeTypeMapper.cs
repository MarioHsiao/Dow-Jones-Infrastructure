using System;
using DowJones.Mapping;
using SourceEntityType = Factiva.Gateway.Messages.Assets.Queries.V1_0.SourceEntityType;
using SourceFilterType = DowJones.Search.SourceFilterType;

namespace DowJones.Managers.Search
{
    public class SourceFilterTypeTypeMapper :
        ITypeMapper<SourceEntityType, SourceFilterType>,
        ITypeMapper<SourceFilterType, SourceEntityType>
    {
        object ITypeMapper.Map(object source)
        {
            if (source is SourceEntityType)
                return Map((SourceEntityType)source);
            if (source is SourceFilterType)
                return Map((SourceFilterType)source);

            throw new NotSupportedException();
        }

        public SourceFilterType Map(SourceEntityType source)
        {
            switch (source)
            {
                case SourceEntityType.BY:
                    return SourceFilterType.Byline;

                case SourceEntityType.PDF:
                    return SourceFilterType.ProductDefineCode;

                case SourceEntityType.SC:
                    return SourceFilterType.SourceCode;

                case SourceEntityType.SN:
                    return SourceFilterType.SourceName;

                default:
                    return SourceFilterType.Restrictor;
            }
        }

        public SourceEntityType Map(SourceFilterType source)
        {
            switch (source)
            {
                case SourceFilterType.Byline:
                    return SourceEntityType.BY;

                case SourceFilterType.ProductDefineCode:
                    return SourceEntityType.PDF;

                case SourceFilterType.SourceCode:
                    return SourceEntityType.SC;

                case SourceFilterType.SourceName:
                    return SourceEntityType.SN;

                default:
                    return SourceEntityType.RST;
            }
        }
    }
}