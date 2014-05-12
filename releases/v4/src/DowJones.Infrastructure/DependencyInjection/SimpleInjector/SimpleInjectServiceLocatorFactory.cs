using System.Linq;
using DowJones.Infrastructure;
using SimpleInjector;

namespace DowJones.DependencyInjection.SimpleInjector
{
	public class SimpleInjectServiceLocatorFactory : IServiceLocatorFactory
	{
		private readonly IAssemblyRegistry _assemblyRegistry;

		public DowJonesDependencyInjectionSettings Settings { get; private set; }


		public SimpleInjectServiceLocatorFactory(IAssemblyRegistry assemblyRegistry = null, DowJonesDependencyInjectionSettings settings = null)
		{
			_assemblyRegistry = assemblyRegistry;
			Settings = settings ?? new DowJonesDependencyInjectionSettings();
		}


		public IServiceLocator Create()
		{
			var kernel = CreateKernel();
			var locator = new SimpleInjectServiceLocator(kernel);
			return locator;
		}

		protected IContainer CreateKernel()
		{
			var assemblyRegistry = _assemblyRegistry ?? CreateAssemblyRegistry();

			var kernel = new SimpleInjectContainer(new Container());

			kernel.RegisterSingle(assemblyRegistry);

			return kernel;
		}

		private IAssemblyRegistry CreateAssemblyRegistry()
		{
			var filePatterns =
				Settings.AutoLoadedAssemblyFilePatterns
					.Select(x => x.Replace("*", string.Empty).Replace(".dll", string.Empty));

			return AssemblyRegistry.Create(filePatterns);
		}
	}
}