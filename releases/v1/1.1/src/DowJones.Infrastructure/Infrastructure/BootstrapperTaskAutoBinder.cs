using DowJones.DependencyInjection;

namespace DowJones.Infrastructure
{
    public class BootstrapperTaskAutoBinder : DependencyInjectionModule
    {
        protected override void OnLoad()
        {
            AutoBind<IBootstrapperTask>();
        }
    }


    /// <summary>
    /// A dummy bootstrapper task that should always get registered
    /// so calls to Get&lt;IEnumerable&lt;IBootstrapperTask&gt;&gt; should never fail.
    /// </summary>
    public class DummyBootstrapperTask : IBootstrapperTask
    {
        public void Execute()
        {
            // Don't do anything....
        }
    }
}
