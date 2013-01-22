using System;
using System.Threading;
using System.Web;

namespace factiva.widgets.ui.services
{
    /// <summary>
    /// 
    /// </summary>
    public class RenderAsyncOperation : IAsyncResult
    {
        private readonly AsyncCallback _callback;
        private readonly HttpContext _context;
        private readonly Object _state;
        private bool _completed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderAsyncOperation"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="context">The context.</param>
        /// <param name="state">The state.</param>
        public RenderAsyncOperation(AsyncCallback callback, HttpContext context, Object state)
        {
            _callback = callback;
            _context = context;
            _state = state;
            _completed = false;
        }

        #region IAsyncResult Members

        bool IAsyncResult.IsCompleted
        {
            get { return _completed; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return null; }
        }

        Object IAsyncResult.AsyncState
        {
            get { return _state; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return false; }
        }

        #endregion

        /// <summary>
        /// Starts the async work.
        /// </summary>
        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(StartAsyncTask, null);
        }

        /// <summary>
        /// Starts the async task.
        /// </summary>
        /// <param name="workItemState">State of the work item.</param>
        private void StartAsyncTask(Object workItemState)
        {
            _context.Response.Write("<p>Completion IsThreadPoolThread is " + Thread.CurrentThread.IsThreadPoolThread + "</p>\r\n");
            _context.Response.Write("Hello World from Async Handler!");
            _completed = true;
            _callback(this);
        }
    }
}