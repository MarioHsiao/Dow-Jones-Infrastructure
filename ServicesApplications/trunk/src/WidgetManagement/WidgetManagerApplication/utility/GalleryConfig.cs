using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using EMG.widgets.ui.dto.request;
using EMG.widgets.ui.gallery;

namespace EMG.widgets.ui.utility
{
    /// <summary>
    /// Summary description for Gallery Configuration.
    /// </summary>
    public class GalleryConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GalleryConfig"/> class.
        /// </summary>
        public GalleryConfig()
        {
            GetGallerySettings();   
        }

        /// <summary>
        /// Gets the gallery settings from cache or the xml config file.
        /// </summary>
        /// <returns></returns>
        public static GalleryConfiguration GetGallerySettings()
        {
            GalleryConfiguration gallerySettings = (GalleryConfiguration)HttpContext.Current.Cache["GallerySettings"];

            // If the GalleryConfiguration isn't cached, load it from the XML file and add it into the cache.
            if (gallerySettings == null)
            {
                // Create the dataset
                gallerySettings = new GalleryConfiguration();

                string galleryConfigFile = string.Empty;
                // Retrieve the location of the XML configuration file
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["galleryConfigFile"]))
                    galleryConfigFile = HttpContext.Current.Server.MapPath("~/GalleryConfiguration/data/GalleryConfiguration.xml");
                else
                    galleryConfigFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["galleryConfigFile"]);

                // Set the AutoIncrement property to true for easier adding of rows
                gallerySettings.Portal.PortalIdColumn.AutoIncrement = true;
                gallerySettings.ModuleDefinition.ModuleDefIdColumn.AutoIncrement = true;

                // Load the XML data into the DataSet
                gallerySettings.ReadXml(galleryConfigFile);

                // Store the dataset in the cache
                HttpContext.Current.Cache.Insert("GallerySettings", gallerySettings, new CacheDependency(galleryConfigFile));
            }

            return gallerySettings;
        }
    }

    /// <summary>
    /// GallerySettings object
    /// </summary>
    public class GallerySettings
    {
        public ModuleDetail[] moduleDetails = null;
        public PortalSettings portalSettings = new PortalSettings();
        
        private bool _foundPortal = false;
        private WidgetManagementDTO _widgetManagementDTO;
        private readonly ArrayList _temp = new ArrayList();

        /// <summary>
        /// Gets or sets a value indicating whether [found portal].
        /// </summary>
        /// <value><c>true</c> if [found portal]; otherwise, <c>false</c>.</value>
        public bool FoundPortal
        {
            get { return _foundPortal; }
            set { _foundPortal = value; }
        }

        /// <summary>
        /// Gets the widget referer product.
        /// </summary>
        /// <value>The widget referer product.</value>
        public WidgetManagementDTO WidgetManagementDTO
        {
            get { return _widgetManagementDTO; }
            set { _widgetManagementDTO = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GallerySettings"/> class.
        /// </summary>
        /// <param name="widgetManagementDTO">The widget referer product.</param>
        public GallerySettings(WidgetManagementDTO widgetManagementDTO)
            : this((int)widgetManagementDTO.refererProduct)
        {
            _widgetManagementDTO = widgetManagementDTO;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GallerySettings"/> class.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        public GallerySettings(int portalId)
        {
            // Gets the config data from cache or xml document
            GalleryConfiguration gallerySettings = GalleryConfig.GetGallerySettings();

            // Read the ModuleDefinitions and add them to the 
            foreach (GalleryConfiguration.ModuleDefinitionRow moduleDefinitionRow in gallerySettings.ModuleDefinition.Select("", "ModuleDefId"))
            {
                if (moduleDefinitionRow != null)
                {
                    ModuleDetail moduleDetail = new ModuleDetail();
                    moduleDetail.moduleId = moduleDefinitionRow.ModuleDefId;
                    moduleDetail.moduleName = moduleDefinitionRow.ModuleName;
                    moduleDetail.moduleType = moduleDefinitionRow.ModuleType;
                    
                    _temp.Add(moduleDetail);
                }
            }
            if (_temp.Count > 0)
            {
                moduleDetails = (ModuleDetail[]) _temp.ToArray(typeof (ModuleDetail));
            }
            _temp.Clear();

            GalleryConfiguration.PortalRow portalRow =
                (GalleryConfiguration.PortalRow) gallerySettings.Portal.Rows.Find(new object[]{portalId});
            if (portalRow != null)
            {
                portalSettings.portalId = portalRow.PortalId;
                portalSettings.portalName = portalRow.PortalName;
                GalleryConfiguration.ModuleRow[] moduleRows = portalRow.GetModuleRows();
                _foundPortal = true;
                foreach (GalleryConfiguration.ModuleRow moduleRow in moduleRows)
                {
                    ModuleSettings moduleSettings = new ModuleSettings();
                    moduleSettings.moduleId = moduleRow.ModuleId;
                    moduleSettings.targetControl = moduleRow.TargetControl;
                    
                    ModuleDetail moduleDetail = new ModuleDetail();
                    moduleDetail.moduleId = moduleRow.ModuleDefinitionRow.ModuleDefId;
                    moduleDetail.moduleName = moduleRow.ModuleDefinitionRow.ModuleName;
                    moduleDetail.moduleType = moduleRow.ModuleDefinitionRow.ModuleType;

                    moduleSettings.moduleDetail = moduleDetail;

                    portalSettings.modules.Add(moduleSettings);
                }
            }
        }

        /// <summary>
        /// Holds information on each modules details.
        /// </summary>
        public class ModuleDetail
        {
            /// <summary>
            /// 
            /// </summary>
            public int moduleId;
            /// <summary>
            /// 
            /// </summary>
            public string moduleName;
            /// <summary>
            /// 
            /// </summary>
            public string moduleType;
        }

        /// <summary>
        /// 
        /// </summary>
        public class PortalSettings
        {
            /// <summary>
            /// 
            /// </summary>
            public int portalId;
            /// <summary>
            /// 
            /// </summary>
            public string portalName;
            /// <summary>
            /// 
            /// </summary>
            public ArrayList modules = new ArrayList();
        }

        /// <summary>
        /// Holds information for each 
        /// </summary>
        public class ModuleSettings
        {
            /// <summary>
            /// 
            /// </summary>
            public int moduleId;
            /// <summary>
            /// 
            /// </summary>
            public ModuleDetail moduleDetail;
            /// <summary>
            /// 
            /// </summary>
            public string targetControl;
        }

    }
}
