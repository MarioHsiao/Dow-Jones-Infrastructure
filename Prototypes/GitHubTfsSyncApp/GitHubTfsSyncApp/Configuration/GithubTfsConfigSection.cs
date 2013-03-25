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

    public class ProjectMapping : ConfigurationElement
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
    }

    public class ProjectsCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        public ProjectMapping this[int index]
        {
            get
            {
                return base.BaseGet(index) as ProjectMapping;
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
            return new ProjectMapping();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProjectMapping)element).GitHubProjectName;
        }
    }
    #endregion
}