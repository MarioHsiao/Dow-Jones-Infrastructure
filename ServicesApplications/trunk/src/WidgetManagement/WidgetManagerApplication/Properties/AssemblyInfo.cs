// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.InteropServices;
using log4net.Config;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WidgetManagerApplication")]
[assembly: AssemblyDescription("Widget Manager [2010 Q3 Release]")]
#if Debug
[assembly : AssemblyConfiguration("Debug")]
#else

[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("Factiva")]
[assembly: AssemblyProduct("emg.widgets.ui")]
[assembly: AssemblyCopyright("Copyright ©  2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3d5900ae-111a-45be-96b3-d9e4606ca793")]

// Version information for an assembly consists of the following four values:
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("4.24.0.*")]
[assembly: AssemblyFileVersion("4.24.0.0")]

// Enable script combining for all Toolkit scripts
[assembly: AjaxControlToolkit.ScriptCombine]

// This tells Log4Net to look for a Log4Net.config file in the execution directory, 
// in this case, the webroot.  This file contains Log4Net configuration settings.
[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]