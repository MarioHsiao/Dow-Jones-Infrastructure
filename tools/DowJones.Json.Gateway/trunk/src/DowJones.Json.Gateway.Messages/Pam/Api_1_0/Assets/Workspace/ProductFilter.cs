using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ProductFilter")]
    [Serializable]
    [XmlType(TypeName = "ProductFilter", Namespace = Declarations.SchemaVersion)]
    public class ProductFilter : Filter
    {
        private List<Product> _products;

        [JsonProperty(PropertyName = "product")]
        [XmlElement("product")]
        [DataMember(Name = "product")]
        public List<Product> Products
        {
            get
            {
                if (_products == null) _products = new List<Product>();
                return _products;
            }
            set { _products = value; }
        }
    }
}
