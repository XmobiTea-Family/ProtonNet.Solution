using System;
using XmobiTea.Threading.Models;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for scheduling tasks to run at regular intervals.
    /// </summary>
    public interface IScheduleOnInterval
    {
        /// <summary>
        /// Schedules an action to be executed at regular intervals.
        /// </summary>
        /// <param name="action">The action to be scheduled.</param>
        /// <param name="firstInMs">The delay in milliseconds before the first execution of the action.</param>
        /// <param name="regularInMs">The interval in milliseconds between subsequent executions of the action.</param>
        /// <returns>An instance of <see cref="IScheduleOnIntervalTask"/> representing the scheduled task.</returns>
        IScheduleOnIntervalTask ScheduleOnInterval(Action action, long firstInMs, int regularInMs);

    }

}
