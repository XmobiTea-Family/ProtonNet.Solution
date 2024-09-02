using System;

namespace XmobiTea.Threading.Models
{
    /// <summary>
    /// Represents a scheduled task that can be invoked and checked for its running status.
    /// </summary>
    public interface IScheduleTask : ISingleTask
    {
        /// <summary>
        /// Checks if the task is currently running.
        /// </summary>
        /// <returns>True if the task is running; otherwise, false.</returns>
        bool IsRunning();

        /// <summary>
        /// Releases all resources used by the task.
        /// </summary>
        void Dispose();

    }

    /// <summary>
    /// Provides an implementation of a scheduled task with an action and state management.
    /// </summary>
    class ScheduleTask : SingleTask, IScheduleTask
    {
        /// <summary>
        /// The time in milliseconds when the task should be invoked next.
        /// </summary>
        internal long nextInvokeInMs { get; set; }

        /// <summary>
        /// Indicates whether the task is currently running.
        /// </summary>
        internal bool isRunning { get; private set; }

        /// <summary>
        /// Indicates whether the task has been disposed.
        /// </summary>
        internal bool isDispose { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleTask"/> class.
        /// </summary>
        /// <param name="action">The action to be executed by the task.</param>
        public ScheduleTask(Action action) : base(action)
        {
            this.isRunning = true;
            this.isDispose = false;
        }

        /// <summary>
        /// Releases all resources used by the task.
        /// </summary>
        public void Dispose()
        {
            this.isDispose = true;
        }

        /// <summary>
        /// Checks if the task is currently running and not disposed.
        /// </summary>
        /// <returns>True if the task is running and not disposed; otherwise, false.</returns>
        public virtual bool IsRunning() => this.isRunning && !this.isDispose;

        /// <summary>
        /// Executes the task's action internally, with exception handling.
        /// </summary>
        /// <returns>True if the task was executed successfully; otherwise, false.</returns>
        protected bool InvokeInternal()
        {
            try
            {
                if (!this.isDispose)
                {
                    if (this.action == null) return false;

                    this.action.Invoke();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the task's action if it is running, and updates the running state.
        /// </summary>
        /// <returns>True if the task was executed successfully; otherwise, false.</returns>
        public override bool Invoke()
        {
            if (!this.isRunning) return false;
            this.isRunning = true;

            return this.InvokeInternal();
        }

    }

}
