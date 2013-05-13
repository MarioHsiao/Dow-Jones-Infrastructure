using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{

    [DataContract(Name = "instrumentReference")]
    public class InstrumentReferenceModel
    {
        [DataMember(Name = "dunsNumber")]
        public string DunsNumber { get; set; }

        [DataMember(Name = "fcode")]
        public string FCode { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "source")]
        public int Source { get; set; }

        [DataMember(Name = "ticker")]
        public string Ticker { get; set; }
    }
}
