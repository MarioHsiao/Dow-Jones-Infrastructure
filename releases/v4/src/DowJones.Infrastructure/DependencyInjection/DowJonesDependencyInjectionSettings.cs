using System;
using System.Collections.Generic;
using System.IO;
using DowJones.Properties;

namespace DowJones.DependencyInjection
{
	public class DowJonesDependencyInjectionSettings
	{
		public string AssemblyLocation { get; set; }

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

		public DowJonesDependencyInjectionSettings()
		{
			AssemblyLocation = new FileInfo(GetType().Assembly.Location).DirectoryName;
			
		}

		private static IEnumerable<string> GetAutoLoadedAssemblyFilePatterns()
		{
			var configurationSetting = Settings.Default.DependencyInjectionAutoLoadedAssemblyPatterns;

			if (string.IsNullOrWhiteSpace(configurationSetting))
				throw new ApplicationException("Please provide a comma-delimited list of assemblies to be auto-loaded in the DowJones.Properties.Settings.DependencyInjectionAutoLoadedAssemblyPatterns configuration setting");

			IEnumerable<string> autoLoadedAssemblyFilePatterns =
				configurationSetting.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

			return autoLoadedAssemblyFilePatterns;
		}
	}
}
