using System;
using XmobiTea.Threading.Models;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for enqueuing tasks.
    /// </summary>
    public interface IEnqueue
    {
        /// <summary>
        /// Enqueues an action to be executed.
        /// </summary>
        /// <param name="action">The action to be enqueued.</param>
        /// <returns>An instance of <see cref="ISingleTask"/> representing the enqueued task.</returns>
        ISingleTask Enqueue(Action action);

    }

}
