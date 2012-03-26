using System;
using System.ServiceModel.Dispatcher;
using System.Threading;
using System.ServiceModel;

namespace DowJones.Web.Mvc.UI.Canvas.REST.STAOperation
{
    class STAOperationInvoker : IOperationInvoker
    {
        private IOperationInvoker _iOperationInvoker;

        public STAOperationInvoker(IOperationInvoker iOperationInvoker)
        {
            this._iOperationInvoker = iOperationInvoker;
        }

        #region Implementation of IOperationInvoker

        /// <summary>
        /// Returns an <see cref="T:System.Array"/> of parameter objects.
        /// </summary>
        /// <returns>
        /// The parameters that are to be used as arguments to the operation.
        /// </returns>
        public object[] AllocateInputs()
        {
            return _iOperationInvoker.AllocateInputs();
        }

        /// <summary>
        /// Returns an object and a set of output objects from an instance and set of input objects.  
        /// </summary>
        /// <returns>
        /// The return value.
        /// </returns>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="outputs">The outputs from the method.</param>
        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            Object result = null;
            Object[] staOutputs = null;
            OperationContext context = OperationContext.Current;
            var thread = new Thread(new ThreadStart(delegate
            {
                using (var scope = new OperationContextScope(context))
                {
                    result = _iOperationInvoker.Invoke(instance, inputs, out staOutputs);
                }
            }));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            //+
            outputs = staOutputs;
            //+
            return result;

        }

        /// <summary>
        /// An asynchronous implementation of 
        /// the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)" /> method.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.IAsyncResult"/> used to complete the asynchronous call.
        /// </returns>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="callback">The asynchronous callback object.</param>
        /// <param name="state">Associated state data.</param>
        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _iOperationInvoker.InvokeBegin(instance, inputs, callback, state);

        }

        /// <summary>
        /// The asynchronous end method.
        /// </summary>
        /// <returns>
        /// The return value.
        /// </returns>
        /// <param name="instance">The object invoked.</param>
        /// <param name="outputs">The outputs from the method.</param>
        /// <param name="result">The <see cref="T:System.IAsyncResult"/> object.</param>
        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return _iOperationInvoker.InvokeEnd(instance, out outputs, result);
        }

        /// <summary>
        /// Gets a value that specifies whether 
        /// the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)"/> 
        /// or <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.InvokeBegin(System.Object,System.Object[],System.AsyncCallback,System.Object)"/> method is called by the dispatcher.
        /// </summary>
        /// <returns>
        /// true if the dispatcher invokes the synchronous operation; otherwise, false.
        /// </returns>
        public bool IsSynchronous
        {
            get { return _iOperationInvoker.IsSynchronous; }
        }

        #endregion
    }
}
