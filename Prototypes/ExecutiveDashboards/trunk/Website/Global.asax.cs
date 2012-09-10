using System;

namespace DowJones.Dash.Website
{
    public partial class MvcApplication
    {
/*
        public static string Version
        {
            get { return _version.Value; }
        }
*/
        public static Lazy<string> Version = 
            new Lazy<string>(typeof(MvcApplication).Assembly.GetName().Version.ToString);
    }
}