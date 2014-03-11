using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DowJones.Json.Gateway.Messages.Pam.Assets.ItemManager
{
    [DataContract(Name = "UserFileSearchFilter")]
    public class UserFileSearchFilter : Filter
    {
        public UserFileSearchFilter()
        {
            UserFileType = new List<UserFileType>();
        }

        [DataMember(Name = "userFileType")]
        public List<UserFileType> UserFileType { get; set; }
    }
}