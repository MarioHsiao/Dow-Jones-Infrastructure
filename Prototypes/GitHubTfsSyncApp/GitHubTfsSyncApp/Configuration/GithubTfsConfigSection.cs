using System.Configuration;

namespace GitHubTfsSyncApp.Configuration
{
    public class Config
    {
        static public GithubTfsConfigSection GithubTFSConfigSectionInstance
        {
            get { return (GithubTfsConfigSection)ConfigurationManager.GetSection("GithubTFSConfig"); }
        }
    }

    public class GithubTfsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Projects")]
        public ProjectsCollection Projects
        {
            get { return (ProjectsCollection)this["Projects"]; }
        }
    }

    #region ControlData Element

    public class ProjectDetails : ConfigurationElement
    {
        [ConfigurationProperty("gitHubProjectName")]
        public string GitHubProjectName
        {
            get { return (string)this["gitHubProjectName"]; }
            set { this["gitHubProjectName"] = value; }
        }

        [ConfigurationProperty("tfsProjectName")]
        public string TfsProjectName
        {
            get { return (string)this["tfsProjectName"]; }
            set { this["tfsProjectName"] = value; }
        }

        [ConfigurationProperty("gitHubCredentials")]
        public string GitHubCredentials
        {
            get { return (string)this["gitHubCredentials"]; }
            set { this["gitHubCredentials"] = value; }
        }

        [ConfigurationProperty("gitHubClientSecret")]
        public string GitHubClientSecret
        {
            get { return (string)this["gitHubClientSecret"]; }
            set { this["gitHubClientSecret"] = value; }
        }

        [ConfigurationProperty("gitHubClientId")]
        public string GitHubClientId
        {
            get { return (string)this["gitHubClientId"]; }
            set { this["gitHubClientId"] = value; }
        }

        [ConfigurationProperty("tfsLocalWorkspace")]
        public string TfsLocalWorkspace
        {
            get { return (string)this["tfsLocalWorkspace"]; }
            set { this["tfsLocalWorkspace"] = value; }
        }

        [ConfigurationProperty("tfsUserName")]
        public string TfsUserName
        {
            get { return (string)this["tfsUserName"]; }
            set { this["tfsUserName"] = value; }
        }

        [ConfigurationProperty("tfsPassword")]
        public string TfsPassword
        {
            get { return (string)this["tfsPassword"]; }
            set { this["tfsPassword"] = value; }
        }

        [ConfigurationProperty("tfsUrl")]
        public string TfsUrl
        {
            get { return (string)this["tfsUrl"]; }
            set { this["tfsUrl"] = value; }
        }

        [ConfigurationProperty("filters")]
        public FiltersCollection Filters
        {
            get { return (FiltersCollection)this["filters"]; }
        }
    }

    public class FilterDetail:ConfigurationElement
    {
        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("gitSource")]
        public string GitSource
        {
            get { return (string)this["gitSource"]; }
            set { this["gitSource"] = value; }
        }

        [ConfigurationProperty("tfsTarget")]
        public string TfsTarget
        {
            get { return (string)this["tfsTarget"]; }
            set { this["tfsTarget"] = value; }
        }
    }

    public class FiltersCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        public FilterDetail this[int index]
        {
            get
            {
                return base.BaseGet(index) as FilterDetail;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FilterDetail();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilterDetail)element).GitSource;
        }
    }

    public class ProjectsCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        public ProjectDetails this[int index]
        {
            get
            {
                return base.BaseGet(index) as ProjectDetails;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ProjectDetails();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProjectDetails)element).GitHubProjectName;
        }
    }
    #endregion
}