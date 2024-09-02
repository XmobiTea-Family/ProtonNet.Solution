using System;
using System.Collections.Generic;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading.Agent
{
    /// <summary>
    /// Manages and processes tasks that need to be executed.
    /// Implements task enqueuing and processing with statistics tracking.
    /// </summary>
    class EnqueueAgent : IEnqueueInternal, IEnqueue, IService
    {
        private object _lockSingleTaskPending { get; }
        private Queue<SingleTask> singleTaskPending { get; }

        private IStatisticsCounterChange counter { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnqueueAgent"/> class.
        /// </summary>
        /// <param name="counter">The counter used for tracking statistics.</param>
        public EnqueueAgent(IStatisticsCounterChange counter)
        {
            this._lockSingleTaskPending = new object();
            this.singleTaskPending = new Queue<SingleTask>();

            this.counter = counter;
        }

        /// <summary>
        /// Enqueues a new action task for execution.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <returns>An <see cref="ISingleTask"/> representing the enqueued task.</returns>
        public ISingleTask Enqueue(Action action)
        {
            var answer = new SingleTask(action);

            this.Enqueue(answer);

            return answer;
        }

        /// <summary>
        /// Enqueues a <see cref="SingleTask"/> for execution.
        /// </summary>
        /// <param name="task">The task to be enqueued.</param>
        public void Enqueue(SingleTask task)
        {
            this.counter.ChangePending(1);

            lock (this._lockSingleTaskPending)
                this.singleTaskPending.Enqueue(task);
        }

        /// <summary>
        /// Processes pending tasks in the queue.
        /// Updates the statistics counters based on task execution.
        /// </summary>
        public void Service()
        {
            SingleTask singleTask;

            lock (this._lockSingleTaskPending)
            {
                if (this.singleTaskPending.Count == 0) return;

                singleTask = this.singleTaskPending.Dequeue();
            }

            this.counter.ChangePending(-1);
            this.counter.ChangeCalled(1);

            if (!singleTask.Invoke()) this.counter.ChangeFailed(1);
        }

    }

}
