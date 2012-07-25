using System.Web;
using System.Web.Compilation;
using XmlViewEngine;

[assembly:PreApplicationStartMethod(typeof(XmlBuildProvider), "RegisterAsBuildProvider")]

namespace XmlViewEngine
{
    [BuildProviderAppliesTo(BuildProviderAppliesTo.Web | BuildProviderAppliesTo.Code)]
    public class XmlBuildProvider : BuildProvider
    {
        public static void RegisterAsBuildProvider()
        {
            BuildProvider.RegisterBuildProvider(".xml", typeof(XmlBuildProvider));
            BuildProvider.RegisterBuildProvider(".xsl", typeof(XmlBuildProvider));
            BuildProvider.RegisterBuildProvider(".xslt", typeof(XmlBuildProvider));
        }
    }
}
