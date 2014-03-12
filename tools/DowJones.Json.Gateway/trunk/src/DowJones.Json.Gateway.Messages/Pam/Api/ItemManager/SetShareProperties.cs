using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "SetShareProperties")]
    public class SetShareProperties
    {
        public SetShareProperties()
        {
            ShareProperties = new ShareProperties();
        }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "shareProperties")]
        public ShareProperties ShareProperties { get; set; }
    }
}