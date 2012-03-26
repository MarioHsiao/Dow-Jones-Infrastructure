// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasModel.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CanvasModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

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

        protected string BaseWebServicePath
        {
            get { return Settings.Default.DashboardServiceBaseUrl; }
        }

        public virtual bool CanEdit
        {
            get
            {
                if (canEdit != null)
                    return canEdit.Value;

                if(Page == null)
                    return false;

                var properties = Page.ShareProperties as SharePropertiesResponse;
                
                if(properties == null)
                    return false;

                return properties.IsOwner;
            }
            set { canEdit = value; }
        }
        private bool? canEdit;

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

        /// <summary>
        /// Gets or sets the url to load
        /// a Module via AJAX
        /// </summary>
        [ClientProperty("loadModuleUrl")]
        public string LoadModuleUrl { get; set; }

        [ClientProperty]
        public int NumberOfGroups { get; set; }

        public Page Page { get; set; }

        /// <summary>
        /// Gets or sets the preference
        /// </summary>
        /// <value>The preference.</value>
        [ClientProperty]
        public IPreferences Preferences { get; set; }


        /// <summary>
        /// Gets or sets the web service path.
        /// </summary>
        /// <value>The web service path.</value>
        [ClientProperty("webServiceBaseUrl")]
        public string WebServiceBaseUrl
        {
            get
            {
                return webServiceBaseUrl
                    ?? BaseWebServicePath + Settings.Default.CanvasServiceBaseUrl;
            }
            set { webServiceBaseUrl = value; }
        }
        private string webServiceBaseUrl;


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
            DeleteModuleUrl = WebServiceBaseUrl + "/modules/id/json";
            NumberOfGroups = 1;
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
            foreach(var module in (modules ?? Enumerable.Empty<IModule>()).ToArray())
                AddChild(module);
        }
    }
}
