using System.Runtime.Serialization;

namespace DowJones.Web.Mvc.UI.Components.NewsRadar
{
    [DataContract(Name="timeframe")]
    public enum TimeFrame
    {
        Day,
        Week,
        TwoWeeks,
        Month,
        TwoMonth,
        ThreeMonth

    }
}
