using System;
using System.Collections;
using System.Collections.Generic;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace DowJones.DependencyInjection
{
	public class SimpleInjectContainer : IContainer
	{
		private readonly Container _container;

		public SimpleInjectContainer(Container container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets all instances of the given <typeparamref name="TService"/> currently registered in the container.
		/// </summary>
		/// <typeparam name="TService">Type of object requested.</typeparam>
		/// <returns>A sequence of instances of the requested TService.</returns>
		/// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
		public IEnumerable<TService> GetAllInstances<TService>()
		{
			return _container.GetAllInstances<TService>();
		}

		/// <summary>
		/// Gets all instances of the given <paramref name="serviceType"/> currently registered in the container.
		/// </summary>
		/// <param name="serviceType">Type of object requested.</param>
		/// <returns>A sequence of instances of the requested serviceType.</returns>
		/// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
		public IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.GetAllInstances(serviceType);
		}

		/// <summary>Gets an instance of the given <typeparamref name="TService"/>.</summary>
		/// <typeparam name="TService">Type of object requested.</typeparam>
		/// <returns>The requested service instance.</returns>
		/// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
		public TService GetInstance<TService>() where TService : class
		{
			return _container.GetInstance<TService>();
		}

		/// <summary>Gets an instance of the given <paramref name="serviceType"/>.</summary>
		/// <param name="serviceType">Type of object requested.</param>
		/// <returns>The requested service instance.</returns>
		/// <exception cref="ActivationException">Thrown when there are errors resolving the service instance.</exception>
		public object GetInstance(Type serviceType)
		{
			return _container.GetInstance(serviceType);
		}

		/// <summary>
		/// Registers that a new instance of <typeparamref name="TConcrete"/> will be returned every time it 
		/// is requested (transient). Note that calling this method is redundant in most scenarios, because
		/// the container will return a new instance for unregistered concrete types. Registration is needed
		/// when the security restrictions of the application's sandbox don't allow the container to create
		/// such type.
		/// </summary>
		/// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <typeparamref name="TConcrete"/> has already been registered.
		/// </exception>
		/// <exception cref="ArgumentException">Thrown when the <typeparamref name="TConcrete"/> is a type
		/// that can not be created by the container.</exception>
		public void Register<TConcrete>() where TConcrete : class
		{
			_container.Register<TConcrete>();
		}

		/// <summary>
		/// Registers that a new instance of <typeparamref name="TImplementation"/> will be returned every time a
		/// <typeparamref name="TService"/> is requested.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
		/// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <typeparamref name="TService"/> has already been registered.</exception>
		/// <exception cref="ArgumentException">Thrown when the given <typeparamref name="TImplementation"/> 
		/// type is not a type that can be created by the container.
		/// </exception>
		public void Register<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			_container.Register<TService, TImplementation>();
		}

		/// <summary>
		/// Registers the specified delegate that allows returning transient instances of 
		/// <typeparamref name="TService"/>. The delegate is expected to always return a new instance on
		/// each call.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		/// <param name="instanceCreator">The delegate that allows building or creating new instances.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when the 
		/// <typeparamref name="TService"/> has already been registered.</exception>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="instanceCreator"/> is a null reference.</exception>
		public void Register<TService>(Func<TService> instanceCreator) where TService : class
		{
			_container.Register(instanceCreator);
		}

		/// <summary>
		/// Registers that a new instance of <paramref name="concreteType"/> will be returned every time it 
		/// is requested (transient).
		/// </summary>
		/// <param name="concreteType">The concrete type that will be registered.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="concreteType"/> is a null 
		/// references (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="concreteType"/> represents an 
		/// open generic type or is a type that can not be created by the container.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="concreteType"/> has already been registered.
		/// </exception>
		public void Register(Type concreteType)
		{
			_container.Register(concreteType);
		}

		/// <summary>
		/// Registers that a new instance of <paramref name="implementation"/> will be returned every time a
		/// <paramref name="serviceType"/> is requested. If <paramref name="serviceType"/> and 
		/// <paramref name="implementation"/> represent the same type, the type is registered by itself.
		/// </summary>
		/// <param name="serviceType">The base type or interface to register.</param>
		/// <param name="implementation">The actual type that will be returned when requested.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceType"/> or 
		/// <paramref name="implementation"/> are null references (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="implementation"/> is
		/// no sub type from <paramref name="serviceType"/> (or the same type), or one of them represents an 
		/// open generic type.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="serviceType"/> has already been registered.
		/// </exception>
		public void Register(Type serviceType, Type implementation)
		{
			_container.Register(serviceType, implementation);
		}

		/// <summary>
		/// Registers the specified delegate that allows returning instances of <paramref name="serviceType"/>.
		/// </summary>
		/// <param name="serviceType">The base type or interface to register.</param>
		/// <param name="instanceCreator">The delegate that will be used for creating new instances.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="serviceType"/> or 
		/// <paramref name="instanceCreator"/> are null references (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceType"/> represents an
		/// open generic type.</exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="serviceType"/> has already been registered.
		/// </exception>
		public void Register(Type serviceType, Func<object> instanceCreator)
		{
			_container.Register(serviceType, instanceCreator);
		}

		/// <summary>
		/// Registers a single concrete instance that will be constructed using constructor injection and will
		/// be returned when this instance is requested by type <typeparamref name="TConcrete"/>. 
		/// This <typeparamref name="TConcrete"/> must be thread-safe when working in a multi-threaded 
		/// environment.
		/// </summary>
		/// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when 
		/// <typeparamref name="TConcrete"/> has already been registered.
		/// </exception>
		/// <exception cref="ArgumentException">Thrown when the <typeparamref name="TConcrete"/> is a type
		/// that can not be created by the container.</exception>
		public void RegisterSingle<TConcrete>() where TConcrete : class
		{
			_container.RegisterSingle<TConcrete>();
		}

		/// <summary>
		/// Registers that the same a single instance of type <typeparamref name="TImplementation"/> will be 
		/// returned every time an <typeparamref name="TService"/> type is requested. If 
		/// <typeparamref name="TService"/> and <typeparamref name="TImplementation"/>  represent the same 
		/// type, the type is registered by itself. <typeparamref name="TImplementation"/> must be thread-safe 
		/// when working in a multi-threaded environment.
		/// </summary>
		/// <typeparam name="TService">
		/// The interface or base type that can be used to retrieve the instances.
		/// </typeparam>
		/// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when the 
		/// <typeparamref name="TService"/> has already been registered.</exception>
		/// <exception cref="ArgumentException">Thrown when the given <typeparamref name="TImplementation"/> 
		/// type is not a type that can be created by the container.
		/// </exception>
		public void RegisterSingle<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			_container.RegisterSingle<TService, TImplementation>();
		}

		/// <summary>
		/// Registers a single instance that will be returned when an instance of type 
		/// <typeparamref name="TService"/> is requested. This <paramref name="instance"/> must be thread-safe
		/// when working in a multi-threaded environment.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instance.</typeparam>
		/// <param name="instance">The instance to register.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when the 
		/// <typeparamref name="TService"/> has already been registered.</exception>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="instance"/> is a null reference.
		/// </exception>
		public void RegisterSingle<TService>(TService instance) where TService : class
		{
			_container.RegisterSingle(instance);
		}

		/// <summary>
		/// Registers the specified delegate that allows constructing a single instance of 
		/// <typeparamref name="TService"/>. This delegate will be called at most once during the lifetime of 
		/// the application. The returned instance must be thread-safe when working in a multi-threaded 
		/// environment.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		/// <param name="instanceCreator">The delegate that allows building or creating this single
		/// instance.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when a 
		/// <paramref name="instanceCreator"/> for <typeparamref name="TService"/> has already been registered.
		/// </exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instanceCreator"/> is a 
		/// null reference.</exception>
		public void RegisterSingle<TService>(Func<TService> instanceCreator) where TService : class
		{
			_container.RegisterSingle(instanceCreator);
		}

		/// <summary>
		/// Registers that the same instance of type <paramref name="implementation"/> will be returned every 
		/// time an instance of type <paramref name="serviceType"/> type is requested. If 
		/// <paramref name="serviceType"/> and <paramref name="implementation"/> represent the same type, the 
		/// type is registered by itself. <paramref name="implementation"/> must be thread-safe when working 
		/// in a multi-threaded environment.
		/// </summary>
		/// <param name="serviceType">The base type or interface to register.</param>
		/// <param name="implementation">The actual type that will be returned when requested.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="serviceType"/> or 
		/// <paramref name="implementation"/> are null references (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="implementation"/> is
		/// no sub type from <paramref name="serviceType"/>, or when one of them represents an open generic
		/// type.</exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="serviceType"/> has already been registered.
		/// </exception>
		public void RegisterSingle(Type serviceType, Type implementation)
		{
			_container.RegisterSingle(serviceType, implementation);
		}

		/// <summary>
		/// Registers the specified delegate that allows constructing a single <paramref name="serviceType"/> 
		/// instance. The container will call this delegate at most once during the lifetime of the application.
		/// </summary>
		/// <param name="serviceType">The base type or interface to register.</param>
		/// <param name="instanceCreator">The delegate that will be used for creating that single instance.</param>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceType"/> represents an open
		/// generic type.</exception>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="serviceType"/> or 
		/// <paramref name="instanceCreator"/> are null references (Nothing in
		/// VB).</exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="serviceType"/> has already been registered.
		/// </exception>
		public void RegisterSingle(Type serviceType, Func<object> instanceCreator)
		{
			_container.RegisterSingle(serviceType, instanceCreator);
		}

		/// <summary>
		/// Registers a single instance that will be returned when an instance of type 
		/// <paramref name="serviceType"/> is requested. This <paramref name="instance"/> must be thread-safe
		/// when working in a multi-threaded environment.
		/// </summary>
		/// <param name="serviceType">The base type or interface to register.</param>
		/// <param name="instance">The instance to register.</param>
		/// <exception cref="ArgumentNullException">Thrown when either <paramref name="serviceType"/> or 
		/// <paramref name="instance"/> are null references (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="instance"/> is
		/// no sub type from <paramref name="serviceType"/>.</exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when an 
		/// the <paramref name="serviceType"/> has already been registered.
		/// </exception>
		public void RegisterSingle(Type serviceType, object instance)
		{
			_container.RegisterSingle(serviceType, instance);
		}

		public void RegisterInitializer<TService>(Action<TService> instanceInitializer) where TService : class
		{
			_container.RegisterInitializer(instanceInitializer);
		}

		/// <summary>
		/// Registers an <see cref="Action{InstanceInitializationData}"/> delegate that runs after the 
		/// creation of instances for which the supplied <paramref name="predicate"/> returns true. Please 
		/// note that only instances that are created by the container can be initialized this way.
		/// </summary>
		/// <param name="instanceInitializer">The delegate that will be called after the instance has been
		/// constructed and before it is returned.</param>
		/// <param name="predicate">The predicate that will be used to check whether the given delegate must
		/// be applied to a registration or not. The given predicate will be called once for each registration
		/// in the container.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when either the <paramref name="instanceInitializer"/> or <paramref name="predicate"/> are 
		/// null references.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered.</exception>
		/// <remarks>
		/// <para>
		/// Note: Initializers are guaranteed to be executed in the order they are registered.
		/// </para>
		/// <para>
		/// Note: The <paramref name="predicate"/> is <b>not</b> guaranteed to be called once per registration;
		/// when a registration's instance is requested for the first time simultaniously over multiple thread,
		/// the predicate might be called multiple times. The caller of this method is responsible of supplying
		/// a predicate that is thread-safe.
		/// </para>
		/// </remarks>
		public void RegisterInitializer(Action<InstanceInitializationData> instanceInitializer, Predicate<InitializationContext> predicate)
		{
			_container.RegisterInitializer(instanceInitializer, predicate);
		}

		/// <summary>
		/// Registers a dynamic (container uncontrolled) collection of elements of type 
		/// <typeparamref name="TService"/>. A call to <see cref="IContainer.GetAllInstances{TService}"/> will return the 
		/// <paramref name="collection"/> itself, and updates to the collection will be reflected in the 
		/// result. If updates are allowed, make sure the collection can be iterated safely if you're running 
		/// a multi-threaded application.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		/// <param name="collection">The collection to register.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when a <paramref name="collection"/>
		/// for <typeparamref name="TService"/> has already been registered.
		/// </exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is a null
		/// reference.</exception>
		public void RegisterAll<TService>(IEnumerable<TService> collection) where TService : class
		{
			_container.RegisterAll(collection);
		}

		/// <summary>
		/// Registers a collection of singleton elements of type <typeparamref name="TService"/>.
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		/// <param name="singletons">The collection to register.</param>
		/// <exception cref="InvalidOperationException">
		/// Thrown when this container instance is locked and can not be altered, or when a <paramref name="singletons"/>
		/// for <typeparamref name="TService"/> has already been registered.
		/// </exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="singletons"/> is a null
		/// reference.</exception>
		/// <exception cref="ArgumentException">Thrown when one of the elements of <paramref name="singletons"/>
		/// is a null reference.</exception>
		public void RegisterAll<TService>(params TService[] singletons) where TService : class
		{
			_container.RegisterAll(singletons);
		}

		/// <summary>
		/// Registers an collection of <paramref name="serviceTypes"/>, which instances will be resolved when
		/// enumerating the set returned when a collection of <typeparamref name="TService"/> objects is 
		/// requested. On enumeration the container is called for each type in the list.
		/// </summary>
		/// <typeparam name="TService">The base type or interface for elements in the collection.</typeparam>
		/// <param name="serviceTypes">The collection of <see cref="Type"/> objects whose instances
		/// will be requested from the container.</param>
		/// <exception cref="ArgumentNullException">Thrown when one of the supplied arguments is a null 
		/// reference (Nothing in VB).
		/// </exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceTypes"/> contains a null
		/// (Nothing in VB) element, a generic type definition, or the <typeparamref name="TService"/> is
		/// not assignable from one of the given <paramref name="serviceTypes"/> elements.
		/// </exception>
		public void RegisterAll<TService>(params Type[] serviceTypes)
		{
			_container.RegisterAll(serviceTypes);
		}

		/// <summary>
		/// Registers a collection of instances of <paramref name="serviceTypes"/> to be returned when
		/// a collection of <typeparamref name="TService"/> objects is requested.
		/// </summary>
		/// <typeparam name="TService">The base type or interface for elements in the collection.</typeparam>
		/// <param name="serviceTypes">The collection of <see cref="Type"/> objects whose instances
		/// will be requested from the container.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceTypes"/> is a null 
		/// reference (Nothing in VB).
		/// </exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceTypes"/> contains a null
		/// (Nothing in VB) element, a generic type definition, or the <typeparamref name="TService"/> is
		/// not assignable from one of the given <paramref name="serviceTypes"/> elements.
		/// </exception>
		public void RegisterAll<TService>(IEnumerable<Type> serviceTypes)
		{
			_container.RegisterAll(serviceTypes);
		}

		/// <summary>
		/// Registers an collection of <paramref name="serviceTypes"/>, which instances will be resolved when
		/// enumerating the set returned when a collection of <paramref name="serviceType"/> objects is 
		/// requested. On enumeration the container is called for each type in the list.
		/// </summary>
		/// <param name="serviceType">The base type or interface for elements in the collection.</param>
		/// <param name="serviceTypes">The collection of <see cref="Type"/> objects whose instances
		/// will be requested from the container.</param>
		/// <exception cref="ArgumentNullException">Thrown when one of the supplied arguments is a null 
		/// reference (Nothing in VB).
		/// </exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceTypes"/> contains a null
		/// (Nothing in VB) element, a generic type definition, or the <paramref name="serviceType"/> is
		/// not assignable from one of the given <paramref name="serviceTypes"/> elements.
		/// </exception>
		public void RegisterAll(Type serviceType, IEnumerable<Type> serviceTypes)
		{
			_container.RegisterAll(serviceType, serviceTypes);
		}

		/// <summary>
		/// Registers a dynamic (container uncontrolled) collection of elements of type 
		/// <paramref name="serviceType"/>. A call to <see cref="GetAllInstances{T}"/> will return the 
		/// <paramref name="collection"/> itself, and updates to the collection will be reflected in the 
		/// result. If updates are allowed, make sure the collection can be iterated safely if you're running 
		/// a multi-threaded application.
		/// </summary>
		/// <param name="serviceType">The base type or interface for elements in the collection.</param>
		/// <param name="collection">The collection of items to register.</param>
		/// <exception cref="ArgumentNullException">Thrown when one of the supplied arguments is a null 
		/// reference (Nothing in VB).</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="serviceType"/> represents an
		/// open generic type.</exception>
		public void RegisterAll(Type serviceType, IEnumerable collection)
		{
			_container.RegisterAll(serviceType, collection);
		}

		/// <summary>
		/// Registers that one instance of <typeparamref name="TConcrete"/> will be returned for every web
		///             request and ensures that -if <typeparamref name="TConcrete"/> implements
		///             <see cref="T:System.IDisposable"/>- this instance will get disposed on the end of the web request.
		/// 
		/// </summary>
		/// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
		public void RegisterPerWebRequest<TConcrete>() where TConcrete : class
		{
			_container.RegisterPerWebRequest<TConcrete>();
		}


		/// <summary>
		/// Registers that one instance of <typeparamref name="TImplementation"/> will be returned for every
		///             web request every time a <typeparamref name="TService"/> is requested and ensures that -if
		///             <typeparamref name="TImplementation"/> implements <see cref="T:System.IDisposable"/>- this instance
		///             will get disposed on the end of the web request.
		/// 
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.
		///             </typeparam><typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
		public void RegisterPerWebRequest<TService, TImplementation>()
			where TImplementation : class, TService
			where TService : class
		{
			_container.RegisterPerWebRequest<TService, TImplementation>();
		}


		/// <summary>
		/// Registers the specified delegate that allows returning instances of <typeparamref name="TService"/>
		///             and the returned instance will be reused for the duration of a single web request and ensures that,
		///             if the returned instance implements <see cref="T:System.IDisposable"/>, that instance will get
		///             disposed on the end of the web request.
		/// 
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		public void RegisterPerWebRequest<TService>(Func<TService> func) where TService : class
		{
			_container.RegisterPerWebRequest(func);
		}

		/// <summary>
		/// Registers the specified delegate that allows returning instances of <typeparamref name="TService"/>
		///             and the returned instance will be reused for the duration of a single web request and ensures that,
		///             if the returned instance implements <see cref="T:System.IDisposable"/>, and
		///             <paramref name="disposeInstanceWhenWebRequestEnds"/> is set to <b>true</b>, that instance will get
		///             disposed on the end of the web request.
		/// 
		/// </summary>
		/// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
		public void RegisterPerWebRequest<TService>(Func<TService> instanceCreator, bool disposeInstanceWhenWebRequestEnds) where TService : class
		{
			_container.RegisterPerWebRequest(instanceCreator, disposeInstanceWhenWebRequestEnds);
		}

		public void Dispose()
		{
			// do nothing for simpleinjector
		}

		/// <summary>
		/// Injects all public writable properties of the given <paramref name="instance"/> that have a type
		/// that can be resolved by this container instance.
		/// <b>NOTE:</b> This method will be removed in a future release. To use property injection,
		/// implement a custom the <see cref="IPropertySelectionBehavior"/> instead. For more information,
		/// read the 
		/// <a href="https://simpleinjector.codeplex.com/wikipage?title=Extendibility-Points#Property-Injection">extendibility points</a> 
		/// wiki.
		/// </summary>
		/// <param name="instance">The instance whose properties will be injected.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when the <paramref name="instance"/> is null (Nothing in VB).</exception>
		/// <exception cref="ActivationException">Throw when injecting properties on the given instance
		/// failed due to security constraints of the sandbox. This can happen when injecting properties
		/// on an internal type in a Silverlight sandbox, or when running in partial trust.</exception>
		[Obsolete("Container.Inject has been deprecated and will be removed in a future release. " +
            "See https://bit.ly/1jWlo2S.", error: false)]
		public void Inject(object instance)
		{
			_container.InjectProperties(instance);
		}
	}
}