using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net.Config;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DowJones.Web.Showcase")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Dow Jones & Co.")]
[assembly: AssemblyProduct("DowJones.Web.Showcase")]
[assembly: AssemblyCopyright("Copyright © Dow Jones & Co. 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d70d55ee-5029-4222-97cb-1a45fad35c8d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("DowJones.Web.Mvc.Tests")]

// This tells Log4Net to look for a Log4Net.config file in the execution directory, 
// in this case, the web-root.  This file contains Log4Net configuration settings.
[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
