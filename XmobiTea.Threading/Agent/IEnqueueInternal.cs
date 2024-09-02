using XmobiTea.Threading.Models;

namespace XmobiTea.Threading.Agent
{
    /// <summary>
    /// Provides methods for enqueuing tasks that will be executed.
    /// </summary>
    interface IEnqueueInternal
    {
        /// <summary>
        /// Enqueues a task for execution.
        /// </summary>
        /// <param name="task">The task to be enqueued.</param>
        void Enqueue(SingleTask task);

    }

}
