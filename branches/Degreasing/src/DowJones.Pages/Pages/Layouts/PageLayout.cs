using System;
using System.Linq;
using System.Runtime.Serialization;
using DowJones.DependencyInjection;
using DowJones.Infrastructure;

namespace DowJones.Pages
{
    [KnownType("GetKnownTypes")]
    public abstract class PageLayout
    {
        public abstract void AddModule(int moduleId);
        public abstract void RemoveModule(int moduleId);

        public static Type[] GetKnownTypes()
        {
            return ServiceLocator.Resolve<IAssemblyRegistry>().GetKnownTypesOf<PageLayout>().ToArray();
        }
    }
}