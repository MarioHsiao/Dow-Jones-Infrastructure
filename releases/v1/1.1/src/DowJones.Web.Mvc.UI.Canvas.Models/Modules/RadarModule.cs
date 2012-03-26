using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using System.Runtime.Serialization;
using DowJones.Web.Mvc.UI.Components.Radar;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Modules
{
    public enum TimePeriod
    {
        [DataMember(Name = "lastWeek")]
        OneWeek,

        [DataMember(Name = "lastmonth")]
        OneMonth,

        [DataMember(Name = "threeMonths")]
        ThreeMonths
    }

    public class RadarModule : Module<RadarNewspageModule>
    {
        public RadarModel Radar { get; set; }

        private TimePeriod? timePeriod;


        [ClientProperty("timePeriod")]
        public TimePeriod TimePeriod
        {
            get
            {
                if (!timePeriod.HasValue)
                {
                    if (HasAsset)
                    {
                        var timePeriodFromModule = Asset.TimePeriod;
                        switch (timePeriodFromModule)
                        {
                            case Factiva.Gateway.Messages.Assets.Pages.V1_0.TimePeriod.OneWeek:
                                timePeriod = TimePeriod.OneWeek;
                                break;
                            case Factiva.Gateway.Messages.Assets.Pages.V1_0.TimePeriod.OneMonth:
                                timePeriod = TimePeriod.OneMonth;
                                break;
                            case Factiva.Gateway.Messages.Assets.Pages.V1_0.TimePeriod.ThreeMonths:
                                timePeriod = TimePeriod.ThreeMonths;
                                break;
                            default:
                                timePeriod = TimePeriod.OneWeek;
                                break;
                        }

                    }
                    timePeriod = TimePeriod.OneWeek;//default to one week
                }
                return timePeriod.Value;

            }
        }

        public RadarModule()
        {
            Radar = new RadarModel();
        }

        public new SwapModuleEditor Editor
        {
            get
            {
                if ((base.Editor as SwapModuleEditor) == null)
                    base.Editor =
                        new SwapModuleEditor
                        {
                            ModuleId = ModuleId,
                            ModuleType = Mapping.ModuleType.RadarNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }

    }
}