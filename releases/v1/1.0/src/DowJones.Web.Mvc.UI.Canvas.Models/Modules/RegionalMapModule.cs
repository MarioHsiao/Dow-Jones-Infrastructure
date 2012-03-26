using System.Runtime.Serialization;
using DowJones.Utilities.Search.Attributes;
using DowJones.Web.Mvc.UI.Components.RegionalMap;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Utilities.Attributes;
using System;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Modules
{
    public enum RegionalMapTimeFrame
    {   
        [EnumMember]
        [TimeSlice(-33)]
        [AssignedToken("oneMonth")]
        LastMonth = -33,

        [EnumMember]
        [TimeSlice(-9)]
        [AssignedToken("oneWeek")]
        LastWeek = -9
    }

    public class RegionalMapModule : Module<RegionalMapNewspageModule>
    {

        public RegionalMapModel RegionalMap { get; set; }

        [ClientProperty("regtimeframe")]
        public RegionalMapTimeFrame TimeFrame
        {
            get
            {
                if (_timeFrame != null)
                    return _timeFrame.Value;

                switch(Asset.TimePeriod)
                {
                    case (Factiva.Gateway.Messages.Assets.Pages.V1_0.TimePeriod.OneMonth):
                        return RegionalMapTimeFrame.LastMonth;
                    default:
                        return RegionalMapTimeFrame.LastWeek;
                }
            }
            set { _timeFrame = value; }
        }

        public string GetAssignedToken(RegionalMapTimeFrame timeFrame)
        {
            return ((AssignedToken)Attribute.GetCustomAttribute(typeof(RegionalMapTimeFrame).GetField(timeFrame.ToString()), typeof(AssignedToken))).Token;
        }

        public int GetTimeFrameValue(RegionalMapTimeFrame timeFrame)
        {
            return ((TimeSlice)Attribute.GetCustomAttribute(typeof(RegionalMapTimeFrame).GetField(timeFrame.ToString()), typeof(TimeSlice))).Slice;
        }

        private RegionalMapTimeFrame? _timeFrame;

        public RegionalMapModule()
        {
            RegionalMap = new RegionalMapModel();
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
                            ModuleType = Mapping.ModuleType.RegionalMapNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }
    }
}