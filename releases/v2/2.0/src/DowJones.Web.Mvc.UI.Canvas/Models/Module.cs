using System.Collections.Generic;
using System.Linq;
using DowJones.Pages;
using DowJones.Pages.Common;
using DowJones.Preferences;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Components.Common;
using DowJones.Web.Mvc.UI.Components.PersonalizationFilters;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using AccessQualifier = DowJones.Pages.AccessQualifier;
using TagCollection = Factiva.Gateway.Messages.Assets.Pages.V1_0.TagCollection;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public abstract class Module : CompositeComponentModel, IModule
    {
        [ClientProperty("canEdit")]
        public bool CanEdit
        {
            get
            {
                return canEdit
                    ?? (Canvas != null && Canvas.CanEdit);
            }
            set { canEdit = value; }
        }
        private bool? canEdit;

        public bool CanRefresh { get; set; }

        public Canvas Canvas { get; set; }

        public string CanvasId { get; set; }

        public IControlData ControlData
        {
            get
            {
                if (_controlData == null && Canvas != null)
                    return Canvas.ControlData;

                return _controlData;
            }
            set { _controlData = value; }
        }
        private IControlData _controlData;

        [ClientProperty("description")]
        public string Description { get; set; }

        public IViewComponentModel Editor { get; set; }

        [ClientProperty("moduleType")]
        public string ModuleType
        {
            get { return GetType().Name; }
        }

        public ModuleState ModuleState { get; set; }

        [ClientProperty("needsClientData")]
        public bool NeedsClientData { get; set; }

        public override string ID
        {
            get
            {
                if (base.ID == null)
                {
                    return string.Format("{0}-{1}", ModuleType, ModuleId);
                }

                return base.ID;
            }
            set { base.ID = value; }
        }

        [ClientProperty("moduleId")]
        public int ModuleId { get; set; }

        [ClientProperty("rootId")]
        public int RootId { get; set; }

        [ClientProperty("position")]
        public int Position { get; set; }

        public IPreferences Preferences
        {
            get
            {
                if (_preferences == null && Canvas != null)
                    return Canvas.Preferences;

                return _preferences;
            }
            set { _preferences = value; }
        }
        private IPreferences _preferences;

        public TagCollection TagCollection { get; set; }

        [ClientProperty("title")]
        public string Title { get; set; }

        protected Module()
        {
            NeedsClientData = true;
        }

        #region Personalization stuff
        [ClientProperty("inheritPageFilters")]
        public bool InheritPageFilters { get; set; }

        [ClientProperty("regionFilter")]
        public CodeDesc RegionFilter { get; set; }

        [ClientProperty("industryFilter")]
        public CodeDesc IndustryFilter { get; set; }

        [ClientProperty("keywordFilter")]
        public string KeywordFilter { get; set; }
        #endregion

        public void UpdatePersonalizationValues(QueryFilterSet queryFilterSet, AccessQualifier accessQualifier)
        {
            InheritPageFilters = true;

            // don't do any mapping for query filters if it is a factiva module
            if (accessQualifier == AccessQualifier.Global)
                return;

            if (queryFilterSet != null)
            {
                InheritPageFilters = queryFilterSet.Inherit;
                if (queryFilterSet.QueryFilters != null)
                {
                    var industryQueryFilter = queryFilterSet.QueryFilters.Where(queryFilter => queryFilter.Type == Pages.Common.FilterType.Industry).FirstOrDefault();
                    if (industryQueryFilter != null)
                    {
                        IndustryFilter = new CodeDesc(industryQueryFilter.Text.ToLower(), null);
                    }
                    var regionQueryFilter = queryFilterSet.QueryFilters.Where(queryFilter => queryFilter.Type == Pages.Common.FilterType.Region).FirstOrDefault();
                    if (regionQueryFilter != null)
                    {
                        RegionFilter = new CodeDesc(regionQueryFilter.Text.ToLower(), null);
                    }
                    var keywordQueryFilter = queryFilterSet.QueryFilters.Where(queryFilter => queryFilter.Type == Pages.Common.FilterType.Keyword).FirstOrDefault();
                    if (keywordQueryFilter != null)
                    {
                        KeywordFilter = keywordQueryFilter.Text;
                    }
                }
            }
        }
    }
}
