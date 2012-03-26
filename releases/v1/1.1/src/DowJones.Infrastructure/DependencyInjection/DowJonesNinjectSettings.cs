using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Properties;
using Ninject;

namespace DowJones.DependencyInjection
{
    public class DowJonesNinjectSettings : NinjectSettings
    {
        public IEnumerable<string> AutoLoadedAssemblyFilePatterns
        {
            get
            {
                return 
                    _autoLoadedAssemblyFilePatterns = 
                           _autoLoadedAssemblyFilePatterns 
                        ?? GetAutoLoadedAssemblyFilePatterns();
            }
            set { _autoLoadedAssemblyFilePatterns = value; }
        }
        private IEnumerable<string> _autoLoadedAssemblyFilePatterns;

        public IEnumerable<string> CustomAssemblyNames
        {
            get
            {
                return
                    _customAssemblyNames =
                           _customAssemblyNames
                        ?? GetCustomAssemblyNames();
            }
            set { _customAssemblyNames = value; }
        }
        private IEnumerable<string> _customAssemblyNames;


        public DowJonesNinjectSettings()
        {
            InjectAttribute = typeof (InjectAttribute);
            InjectNonPublic = true;
            LoadExtensions = true;
        }


        private IEnumerable<string> GetCustomAssemblyNames()
        {
            IEnumerable<string> assemblyNames = 
                from filePattern in AutoLoadedAssemblyFilePatterns
                let wildcardIndex = filePattern.IndexOf('*')
                let stopIndex = (wildcardIndex > -1) ? wildcardIndex : filePattern.IndexOf(".dll")
                let assemblyName = (stopIndex > 0)
                                        ? filePattern.Substring(0, stopIndex)
                                        : filePattern
                select assemblyName;

            return assemblyNames.ToArray();
        }

        private static IEnumerable<string> GetAutoLoadedAssemblyFilePatterns()
        {
            string configurationSetting =
                Settings.Default.DependencyInjectionAutoLoadedAssemblyPatterns;

            if (string.IsNullOrWhiteSpace(configurationSetting))
                throw new ApplicationException("Please provide a comma-delimited list of assemblies to be auto-loaded in the DowJones.Properties.Settings.DependencyInjectionAutoLoadedAssemblyPatterns configuration setting");

            IEnumerable<string> autoLoadedAssemblyFilePatterns = 
                configurationSetting.Split(new[] { ',', ';' });

            return autoLoadedAssemblyFilePatterns;
        }
    }
}
