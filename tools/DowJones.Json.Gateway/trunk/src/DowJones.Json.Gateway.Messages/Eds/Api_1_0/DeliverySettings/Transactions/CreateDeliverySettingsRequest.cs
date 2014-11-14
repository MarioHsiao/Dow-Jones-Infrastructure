using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings.Transactions
{
    [ServicePath("1.0/Delivery")]
    [DataContract(Name = "CreateDeliverySettings", Namespace = "")]
    [JsonObject(Title = "CreateDeliverySettings")]
    public class CreateDeliverySettingsRequest : IPostJsonRestRequest
    {
        //To do - Check if this needs to be array
        public CreateDeliverySettingsRequest()
        {
            this.userInformation = new UserInformation();
            this.delivery = new List<Delivery>();
        }

        [JsonProperty(PropertyName = "delivery", Required = Required.Always)]
        public List<Delivery> delivery
        {
            get;
            set;
        }

        public UserInformation userInformation
        {
            get;
            set;
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }

    public class CreateDeliverySettingsExRequest  : IPostJsonRestRequest
    {
        //To do - Check if this needs to be array
        public CreateDeliverySettingsExRequest()
        {
            this.userInformation = new UserInformation();
            this.delivery = new List<DeliveryEx>();
        }

        [JsonProperty(PropertyName = "deliveryEx", Required = Required.Always)]
        public List<DeliveryEx> delivery
        {
            get;
            set;
        }

        public UserInformation userInformation
        {
            get;
            set;
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
