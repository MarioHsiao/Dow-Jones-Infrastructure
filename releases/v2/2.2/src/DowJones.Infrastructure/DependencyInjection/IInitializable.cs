using NinjectIInitializable = Ninject.IInitializable;

namespace DowJones.DependencyInjection
{
    /// <summary>
    /// An class that should be initialized 
    /// after it is instantiated
    /// </summary>
    /// <remarks>
    /// Pass-through interface to Ninject.IInitializable
    /// to provide remove direct dependence on Ninject
    /// </remarks>
    public interface IInitializable : NinjectIInitializable
    {
    }
}
