using DowJones.Infrastructure;
using Ninject;

namespace DowJones.Web.Mvc.UI
{
    public class ScriptRegistryWriterFactory : Factory<IScriptRegistryWriter>
    {
        public override IScriptRegistryWriter Create()
        {
            if (DowJones.Properties.Settings.Default.ScriptLoaderEnabled)
                return Kernel.Get<YepNopeScriptRegistryWriter>();
            else
                return Kernel.Get<ScriptRegistryWriter>();
        }
    }
}
