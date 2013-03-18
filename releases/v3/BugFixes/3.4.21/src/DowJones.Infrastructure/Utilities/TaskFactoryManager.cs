// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskFactoryManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using DowJones.Properties;

namespace DowJones.Utilities
{
    public class TaskFactoryManager
    {
        private static readonly int MaxNumberOfThreads = Settings.Default.MaxNumberOfThreads;
        private static TaskFactoryManager hiddenInstance;
        private static StaTaskScheduler staTaskScheduler;
        private static TaskFactory staTaskFactory;
        private static LimitedConcurrencyLevelTaskScheduler limitedConcurrencyLevelTaskScheduler;
        private static TaskFactory limitedConcurrencyLevelTaskFactory;

        public static TaskFactoryManager Instance
        {
            get
            {
                if (hiddenInstance == null)
                {
                    TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionHandler;
                    hiddenInstance = new TaskFactoryManager();
                }
                return hiddenInstance;
            }
        }

        private static void UnobservedTaskExceptionHandler(object sender, UnobservedTaskExceptionEventArgs excArgs)
        {
            // Last line of defense for handling unobserved exceptions
            // Without this, any unobserved exceptions from TPL tasks will terminate the process
            excArgs.SetObserved();

            // Trying to log too extensively here might have caused a memory leak
            //try
            //{
            //    var sb = new System.Text.StringBuilder("\nUnobserved Task Exception:");
            //    if (excArgs.Exception != null)
            //    {
            //        DowJonesUtilitiesException.GetInnerExceptionLog(sb, excArgs.Exception.InnerException);
            //        log4net.LogManager.GetLogger(typeof (TaskFactoryManager)).Error(sb.ToString());
            //    }
            //}
            //catch
            //{
            //    //shouldn't have any errors in this method
            //}
        }

        public TaskFactory GetDefaultTaskFactory()
        {
            switch (Thread.CurrentThread.GetApartmentState())
            {
                case ApartmentState.Unknown:
                case ApartmentState.STA:
                    return GetStaTaskFactory();
                default:
                    return GetLimitedConcurrencyLevelTaskFactory();
            }
        }

        protected internal TaskFactory GetStaTaskFactory()
        {
            if (staTaskFactory == null)
            {
                staTaskScheduler = new StaTaskScheduler(MaxNumberOfThreads);
                staTaskFactory = new TaskFactory(staTaskScheduler);
            }

            return staTaskFactory;
        }

        protected internal TaskFactory GetLimitedConcurrencyLevelTaskFactory()
        {
            if (limitedConcurrencyLevelTaskFactory == null)
            {
                limitedConcurrencyLevelTaskScheduler = new LimitedConcurrencyLevelTaskScheduler(MaxNumberOfThreads);
                limitedConcurrencyLevelTaskFactory = new TaskFactory(limitedConcurrencyLevelTaskScheduler);
            }

            return limitedConcurrencyLevelTaskFactory;
        }

        protected internal TaskFactory GetDefault()
        {
            return Task.Factory;
        }
    }
}
