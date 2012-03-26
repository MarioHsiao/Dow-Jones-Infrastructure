using System;
using System.Runtime.Serialization;
using DowJones.Mapping;

namespace DowJones.Pages.Common
{
    [DataContract(Name = "assetType", Namespace = "")]
    public enum AssetType
    {
        [EnumMember]
        Nothing,
        [EnumMember]
        Page,
        [EnumMember]
        Module,
        [EnumMember]
        Topic
    }

    public class AssetTypeToGWMapper : TypeMapper<AssetType, Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType>
    {
        public override Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType Map(AssetType source)
        {
            switch (source)
            {
                case AssetType.Page:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Page;
                case AssetType.Module:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Module;
                case AssetType.Topic:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Topic;
                case AssetType.Nothing:
                    return Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Nothing;
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public class AssetTypeFromGWMapper : TypeMapper<Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType, AssetType>
    {
        public override AssetType Map(Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType source)
        {
            switch (source)
            {
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Page:
                    return AssetType.Page;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Module:
                    return AssetType.Module;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Topic:
                    return AssetType.Topic;
                case Factiva.Gateway.Messages.Assets.Pages.V1_0.AssetType.Nothing:
                    return AssetType.Nothing;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
