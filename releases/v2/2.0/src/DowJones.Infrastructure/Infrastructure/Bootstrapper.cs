using System.Collections.Generic;
using System.Linq;

namespace DowJones.Infrastructure
{
    public class Bootstrapper
    {
        private readonly IEnumerable<IBootstrapperTask> bootstrapperTasks;

        public Bootstrapper(IEnumerable<IBootstrapperTask> bootstrapperTasks)
        {
            this.bootstrapperTasks = bootstrapperTasks ?? Enumerable.Empty<IBootstrapperTask>();
        }

        public void Execute()
        {
            foreach (var bootstrapperTask in bootstrapperTasks)
            {
                bootstrapperTask.Execute();
            }
        }
    }
}
