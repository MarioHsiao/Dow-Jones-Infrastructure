﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DowJones.Infrastructure")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyProduct("DowJones.Infrastructure")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ff679928-c0dc-4acf-9d32-d3cb79b4bc83")]

[assembly: InternalsVisibleTo("DowJones.Web.Mvc")]
[assembly: InternalsVisibleTo("DowJones.Web.Mvc.Tests")]
[assembly: InternalsVisibleTo("DowJones.Web.Mvc.UI.Canvas.DataAccess" )]
[assembly: InternalsVisibleTo("DowJones.Web")]
[assembly: InternalsVisibleTo("DowJones.Assemblers")]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]     
[assembly: System.Security.SecurityRules( System.Security.SecurityRuleSet.Level1 )]