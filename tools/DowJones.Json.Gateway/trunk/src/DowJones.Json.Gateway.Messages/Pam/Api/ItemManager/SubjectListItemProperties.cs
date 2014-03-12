using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "SubjectListItemProperties")]
    public class SubjectListItemProperties : ItemProperties
    {
        [DataMember(Name = "operator")]
        public ItemOperator Operator { get; set; }
    }
}