using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Infrastructure;

namespace DowJones.Web
{
    public class IISVersion
    {
        private static readonly Regex IISVersionRegex = new Regex(@"Microsoft-IIS/(?<Major>[0-9]*)");

        public static IISVersion Cassini = new IISVersion(null, "Cassini");

        public int? Major { get; private set; }
        public string VersionString { get; private set; }

        protected IISVersion()
        {
        }

        public IISVersion(int? major)
            : this(major, major.ToString())
        {
        }

        public IISVersion(int? major, string version)
        {
            Guard.IsNotNullOrEmpty(version, "version");

            Major = major;
            VersionString = version;
        }

        public bool SupportsRouting
        {
            get
            {
                return (Major == null || Major >= 7);
            }
        }

        public override string ToString()
        {
            return VersionString;
        }

        public static IISVersion ParseServerVersion(string serverVersion)
        {
            if (string.IsNullOrWhiteSpace(serverVersion))
                return IISVersion.Cassini;

            Match versionMatch = IISVersionRegex.Match(serverVersion);

            int? majorVersion = null;

            int major;
            if(int.TryParse(versionMatch.Groups["Major"].Value, out major))
                majorVersion = major;

            return new IISVersion(major: majorVersion, version: serverVersion);
        }
    }

    public static class IISVersionHttpRequestExtensionMethods
    {

        public static IISVersion GetIISVersion(this HttpContextBase context)
        {
            IISVersion version;

            if (HttpRuntime.UsingIntegratedPipeline)
            {
                version = QueryIISExecutableVersion();
            }
            else
            {
                string serverSoftware = context.Request["SERVER_SOFTWARE"];
                version = IISVersion.ParseServerVersion(serverSoftware);
            }

            return version;
        }

        public static IISVersion GetIISVersion(this HttpRequestBase request)
        {
            IISVersion version;

            if (request == null)
            {
                version = QueryIISExecutableVersion();
            }
            else
            {
                string serverSoftware = request["SERVER_SOFTWARE"];
                version = IISVersion.ParseServerVersion(serverSoftware);
            }

            return version;
        }

        private static IISVersion QueryIISExecutableVersion()
        {
            string iisLocation = System.Environment.SystemDirectory + @"\inetsrv\w3wp.exe";

            if (!File.Exists(iisLocation))
                return new IISVersion(7);

            FileVersionInfo iisExe = FileVersionInfo.GetVersionInfo(iisLocation);
            return new IISVersion(iisExe.FileMajorPart, iisExe.FileVersion);
        }
    }
}