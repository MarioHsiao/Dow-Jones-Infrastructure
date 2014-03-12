using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "UpdateItemPropertiesResponse")]
    public class UpdateItemPropertiesResponse
    {

        private long idField;

        [DataMember()]
        public long id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}