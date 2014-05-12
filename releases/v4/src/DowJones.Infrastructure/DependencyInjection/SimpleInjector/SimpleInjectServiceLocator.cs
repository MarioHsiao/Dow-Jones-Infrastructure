using System;

namespace DowJones.DependencyInjection.SimpleInjector
{
	public class SimpleInjectServiceLocator : IServiceLocator
	{
		private bool _disposed;
		private readonly IContainer _kernel;

		public IContainer Kernel
		{
			get { return _kernel; }
		}

		public SimpleInjectServiceLocator(IContainer kernel)
		{
			_kernel = kernel;
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			// cannot/should not dispose something it din't create.
			//_kernel.Dispose();
			_disposed = true;
		}

		public IServiceLocator Register<TService>(Func<IServiceLocator, TService> factory)
		{
			throw new NotImplementedException();
		}

		public IServiceLocator Register<TService>(TService service) where TService : class
		{
			_kernel.RegisterPerWebRequest<TService, TService>();
			return this;
		}

		public void Inject(object target)
		{
			_kernel.Inject(target);
		}

		public object Resolve(Type targetType)
		{
			return _kernel.GetInstance(targetType);
		}

		public T Resolve<T>() where T : class
		{
			return _kernel.GetInstance<T>();
		}

		

	}
}