using System;
using DowJones.Web.Mvc.UI.Canvas.Editors;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using System.Runtime.Serialization;


namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class TrendingNewsModule : Module<TrendingNewsPageModule>
    {
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

        [ClientProperty("entityType")]
        public EntityType EntityType
        {
            get
            {
                return EntityType.Companies; // its not persisted in PAM           
            }
        }


        public new SwapModuleEditor Editor
        {
            get
            {
                if ((base.Editor as SwapModuleEditor) == null)
                    base.Editor = GetDefaultEditor();

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }

        private SwapModuleEditor GetDefaultEditor()
        {
            return new SwapModuleEditor
            {
                ModuleId = ModuleId,
                ModuleType = Mapping.ModuleType.TrendingNewsPageModule.ToString()
            };
        }
        

    }

    public enum TimePeriod
    {
        [DataMember(Name = "lastWeek")]
        OneWeek,

        [DataMember(Name = "lastmonth")]
        OneMonth,

        [DataMember(Name = "threeMonths")]
        ThreeMonths
    }

    public enum EntityType
    {
        [DataMember(Name = "companies")]
        Companies = 0,

        [DataMember(Name = "people")]
        People = 1,

        [DataMember(Name = "subjects")]
        Subjects = 2,
    }
}
