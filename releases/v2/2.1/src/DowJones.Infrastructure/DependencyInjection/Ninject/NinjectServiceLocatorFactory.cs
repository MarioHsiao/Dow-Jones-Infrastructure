using System.Collections.Generic;
using System.Linq;
using DowJones.Infrastructure;
using Ninject;
using Ninject.Syntax;

namespace DowJones.DependencyInjection.Ninject
{
    public class NinjectServiceLocatorFactory : IServiceLocatorFactory
    {
        private readonly IAssemblyRegistry _assemblyRegistry;

        public DowJonesNinjectSettings Settings { get; private set; }


        public NinjectServiceLocatorFactory(IAssemblyRegistry assemblyRegistry = null, DowJonesNinjectSettings settings = null)
        {
            _assemblyRegistry = assemblyRegistry;
            Settings = settings ?? new DowJonesNinjectSettings();
        }


        public IServiceLocator Create()
        {
            var kernel = CreateKernel();
            var locator = new NinjectServiceLocator(kernel);
            return locator;
        }

        protected IKernel CreateKernel()
        {
            var assemblyRegistry = _assemblyRegistry ?? CreateAssemblyRegistry();

            var kernel = new StandardKernel(Settings);

            kernel.Bind<IResolutionRoot>().ToConstant(kernel);
            kernel.Bind<IAssemblyRegistry>().ToConstant(assemblyRegistry);
            kernel.Load(Settings.AutoLoadedAssemblyFilePatterns);

            return kernel;
        }

        private IAssemblyRegistry CreateAssemblyRegistry()
        {
            IEnumerable<string> filePatterns =
                Settings.AutoLoadedAssemblyFilePatterns
                    .Select(x => x.Replace("*", string.Empty).Replace(".dll", string.Empty));

            return AssemblyRegistry.Create(filePatterns);
        }
    }
}