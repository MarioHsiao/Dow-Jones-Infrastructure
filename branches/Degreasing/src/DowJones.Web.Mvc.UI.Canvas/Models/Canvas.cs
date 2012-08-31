// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasModel.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CanvasModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;
using DowJones.Preferences;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Components.PersonalizationFilters;
using DowJones.Web.Mvc.UI.Components.Search;

namespace DowJones.Web.Mvc.UI.Canvas
{
    public class Canvas : ViewComponentModel
    {
        /// <summary>
        /// Gets or sets the url to Add
        /// a Module to an existing Page via AJAX
        /// </summary>
        [ClientProperty("addModuleUrl")]
        public string AddModuleUrl { get; set; }

        public virtual bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the canvas ID.
        /// </summary>
        /// <value>The canvas ID.</value>
        [ClientProperty("canvasId")]
        public string CanvasID { get; set; }

        public override IEnumerable<IViewComponentModel> Children
        {
            get { return _children; }
            set
            {
                var children = value as IList<IViewComponentModel>;

                if (children != null)
                    _children = children;

                _children = new List<IViewComponentModel>(value);
            }
        }
        private IList<IViewComponentModel> _children;

        [ClientProperty]
        public IControlData ControlData { get; set; }

        [ClientProperty("deleteModuleUrl")]
        public string DeleteModuleUrl { get; set; }

        [ClientProperty("isPublished")]
        public bool IsPublished { get; set; }

        [ClientProperty("layout")]
        public CanvasLayout Layout { get; set; }

        /// <summary>
        /// Gets or sets the url to load
        /// a Module via AJAX
        /// </summary>
        [ClientProperty("loadModuleUrl")]
        public string LoadModuleUrl { get; set; }

        [Obsolete("Use Layout")]
        [ClientProperty]
        public int NumberOfGroups
        {
            get
            {
                var groupedLayout = (Layout as ZoneCanvasLayout);

                if (groupedLayout == null || groupedLayout.Zones == null)
                    return 1;

                return groupedLayout.Zones.Count();
            }
        }

        [ClientProperty("regionFilter")]
        public CodeDesc RegionFilter { get; set; }

        [ClientProperty("industryFilter")]
        public CodeDesc IndustryFilter { get; set; }

        [ClientProperty("keywordFilter")]
        public string KeywordFilter { get; set; }
        
        [ClientProperty("companyFilter")]
        public CodeDesc CompanyFilter { get; set; }

        [ClientProperty("lensType")]
        public LensType LensType { get; set; }

        [ClientProperty("metadataCode")]
        public string MetadataCode { get; set; }

        [ClientProperty("parentCodes")]
        public IEnumerable<string> ParentCodes { get; set; }

        /// <summary>
        /// Gets or sets the preference
        /// </summary>
        /// <value>The preference.</value>
        [ClientProperty]
        public IPreferences Preferences { get; set; }

        /// <summary>
        /// Gets or sets the preference
        /// </summary>
        /// <value>The preference.</value>
        [ClientProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the web service path.
        /// </summary>
        /// <value>The web service path.</value>
        [ClientProperty("webServiceBaseUrl")]
        public string WebServiceBaseUrl { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        public Canvas()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Canvas"/> class.
        /// </summary>
        /// <param name="modules">The modules.</param>
        public Canvas(IEnumerable<IModule> modules)
        {
            Children = new List<IModule>(modules ?? Enumerable.Empty<IModule>());
            WebServiceBaseUrl = CanvasSettings.Default.GetDataServiceUrl(CanvasSettings.Default.CanvasServiceUrl);
            DeleteModuleUrl = WebServiceBaseUrl + "/modules/id/json";
        }


        public void AddChild(IModule module)
        {
            Guard.IsNotNull(module, "module");

            module.Canvas = this;
            module.CanvasId = CanvasID;

            if (!_children.Contains(module))
                _children.Add(module);
        }

        public void AddChildren(IEnumerable<IModule> modules)
        {
            foreach (var module in (modules ?? Enumerable.Empty<IModule>()).ToArray())
                AddChild(module);
        }
    }
}
