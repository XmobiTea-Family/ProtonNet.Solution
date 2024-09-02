using System;
using XmobiTea.Threading.Models;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for scheduling tasks.
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// Schedules an action to be executed after a specified delay.
        /// </summary>
        /// <param name="action">The action to be scheduled.</param>
        /// <param name="firstInMs">The delay in milliseconds before the action is executed.</param>
        /// <returns>An instance of <see cref="IScheduleTask"/> representing the scheduled task.</returns>
        IScheduleTask Schedule(Action action, long firstInMs);

    }

}
