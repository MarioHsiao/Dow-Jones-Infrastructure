using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.Diagnostics.Resources
{
    public class EmbeddedResourceDebuggerRequest
    {
        [DisplayName("Assembly Name")]
        public string AssemblyName { get; set; }

        protected Assembly Assembly
        {
            get
            {
                if (_assembly == null && !string.IsNullOrWhiteSpace(AssemblyName))
                    _assembly = Assembly.ReflectionOnlyLoad(AssemblyName);

                return _assembly;
            }
            set { _assembly = value; }
        }
        private Assembly _assembly;

        public string ErrorMessage { get; set; }

        public SelectList ReferencedAssemblies { get; set; }

        [DisplayName("Resource Name")]
        public string ResourceName { get; set; }

        public bool IsEmbeddedResource
        {
            get
            {
                if(Assembly == null)
                    return false;

                var resourceInfo = Assembly.GetManifestResourceInfo(ResourceName);
                return resourceInfo != null;
            }
        }

        public string EmbeddedResourceContents
        {
            get
            {
                string contents = "[[ NOT FOUND ]]";

                if (IsEmbeddedResource)
                {
                    try
                    {
                        using (var reader = new StreamReader(Assembly.GetManifestResourceStream(ResourceName)))
                        {
                            contents = HttpUtility.HtmlEncode(reader.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        contents = string.Format("Error reading embedded resource:\n{0}", ex);
                    }
                }

                return contents;
            }
        }

        public string WebResourceUrl
        {
            get
            {
                if(_webResourceUrl == null && HasAssemblyName && HasResourceName)
                {
                    try
                    {
                        _webResourceUrl = Assembly.GetWebResourceUrl(ResourceName.Trim());
                    }
                    catch(Exception x)
                    {
                        ErrorMessage = x.ToString();
                    }
                }

                return _webResourceUrl;
            }
            set { _webResourceUrl = value; }
        }
        private string _webResourceUrl;


        #region Has[Property] Helpers

        public bool HasAssemblyName
        {
            get { return !string.IsNullOrWhiteSpace(AssemblyName); }
        }

        public bool HasResourceName
        {
            get { return !string.IsNullOrWhiteSpace(ResourceName); }
        }

        public bool HasWebResourceUrl
        {
            get { return !string.IsNullOrWhiteSpace(WebResourceUrl); }
        }

        #endregion

        public EmbeddedResourceDebuggerRequest()
        {
            ReferencedAssemblies = new SelectList(new [] { "[[ No Assemblies Found ]]" });
        }
    }
}