using System;

namespace XmobiTea.Threading.Models
{
    /// <summary>
    /// Represents a scheduled task that repeats at regular intervals.
    /// </summary>
    public interface IScheduleOnIntervalTask : IScheduleTask
    {
        /// <summary>
        /// Gets the interval in milliseconds between consecutive executions.
        /// </summary>
        /// <returns>The interval in milliseconds.</returns>
        int GetRegularInMs();

    }

    /// <summary>
    /// Provides an implementation of a scheduled task that repeats at regular intervals.
    /// </summary>
    class ScheduleOnIntervalTask : ScheduleTask, IScheduleOnIntervalTask
    {
        /// <summary>
        /// The interval in milliseconds between consecutive executions.
        /// </summary>
        private int regularInMs { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleOnIntervalTask"/> class.
        /// </summary>
        /// <param name="action">The action to be executed by the task.</param>
        /// <param name="regularInMs">The interval in milliseconds between consecutive executions.</param>
        public ScheduleOnIntervalTask(Action action, int regularInMs) : base(action)
        {
            this.regularInMs = regularInMs;
        }

        /// <summary>
        /// Gets the interval in milliseconds between consecutive executions.
        /// </summary>
        /// <returns>The interval in milliseconds.</returns>
        public int GetRegularInMs() => this.regularInMs;

        /// <summary>
        /// Executes the task's action and handles the task's running state.
        /// </summary>
        /// <returns>True if the task was executed successfully; otherwise, false.</returns>
        public override bool Invoke()
        {
            return this.InvokeInternal();
        }

    }

}
