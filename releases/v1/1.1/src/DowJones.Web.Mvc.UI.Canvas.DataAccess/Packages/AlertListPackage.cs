using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages
{
    [DataContract(Name = "alertListPackage", Namespace = "")]
    public class AlertListPackage
    {
        [DataMember(Name = "moduleAlerts")]
        public List<Alert> ModuleAlerts { get; set; }

        [DataMember(Name = "alerts")]
        public List<Alert> Alerts { get; set; }
    }

    [DataContract(Name="alert", Namespace = "")]
    public class Alert
    {
        [DataMember(Name="name")]
        public string Name;

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "isOwner")]
        public bool IsOwner;

        [DataMember(Name = "isActive")]
        public bool IsActive;

        [DataMember(Name = "assetType")]
        public AlertAssetType AssetType;

        //[DataMember(Name = "assetTypeDescriptor")]
        //public string AssetTypeDescriptor
        //{
        //    get { return AssetType.ToString(); }
        //}

        [DataMember(Name = "isInModule")]
        public bool IsInModule;

        [DataMember(Name = "isGroupFolder")]
        public bool IsGroupFolder;

        [DataMember(Name = "deliveryMethod")]
        public AlertDeliveryMethod DeliveryMethod;

        [DataMember(Name = "email")]
        public string Email;

        [DataMember(Name = "newsHits")]
        public bool NewsHits;

        //[DataMember(Name = "deliveryMethodDescriptor")]
        //public string DeliveryMethodDescriptor
        //{
        //    get { return DeliveryMethod.ToString(); }
        //}

        [DataMember(Name = "productType")]
        public AlertProductType ProductType;

        //[DataMember(Name = "deliveryMethodDescriptor")]
        //public string ProductTypeDescriptor
        //{
        //    get { return DeliveryMethod.ToString(); }
        //}

        [DataMember(Name = "deliveryTimes")]
        public AlertDeliveryTimes DeliveryTimes;

        [DataMember(Name = "documentFormat")]
        public string DocumentFormat;

        [DataMember(Name = "publishScope")]
        public AlertShareAccessScope PublishScope;
    }

    [DataContract(Name = "alertShareAccessScope", Namespace = "")]
    public enum AlertShareAccessScope
    {
        [EnumMember] Everyone,
        [EnumMember] Account,
        [EnumMember] Personal,
        [EnumMember] PreviousScope,
    }

    [DataContract(Name="alertAssetType", Namespace = "")]
    public enum AlertAssetType
    {
        [EnumMember] Personal,
        [EnumMember] Subscribed,
        [EnumMember] Assigned,
        [EnumMember] Unknown,
    }

    [DataContract(Name = "alertDeliveryMethod", Namespace = "")]
    public enum AlertDeliveryMethod
    {
        [EnumMember] Batch,
        [EnumMember] Continuous,
        [EnumMember] Feed,
        [EnumMember] Online,
    }

    [DataContract(Name = "alertProductType", Namespace = "")]
    public enum AlertProductType
    {
        [EnumMember] Global,
        [EnumMember] FastTrack,
        [EnumMember] FCPCompany,
        [EnumMember] FCPExecutive,
        [EnumMember] FCPIndustry,
        [EnumMember] IWE,
        [EnumMember] Lexis,
        [EnumMember] Iff,
        [EnumMember] SelectFullText,
        [EnumMember] SelectHeadlines,
        [EnumMember] WealthManagementAlerts,
        [EnumMember] InvestmentBankingAlerts,
        [EnumMember] WealthManagementTriggers,
        [EnumMember] InvestmentBankingTriggers,
        [EnumMember] BRITriggers,
        [EnumMember] BRI,
        [EnumMember] GlobalTrigger,
        [EnumMember] WsjProfessional,
        [EnumMember] DjConsultant,
        [EnumMember] DirectToClient,
        [EnumMember] Author,
        [EnumMember] NewAuthor,
    }

    [DataContract(Name = "deliveryTimes", Namespace = "")]
    public enum AlertDeliveryTimes
    {
        [EnumMember] None,
        [EnumMember] Morning,
        [EnumMember] Afternoon,
        [EnumMember] Both,
        [EnumMember] Continuous,
    }
}
