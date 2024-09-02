using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for accessing various statistics counters related to fibers.
    /// </summary>
    public interface IFiberStatisticsCounter
    {
        /// <summary>
        /// Gets the counter for tracking enqueued tasks.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for enqueued tasks.</returns>
        IStatisticsCounter GetEnqueueCounter();

        /// <summary>
        /// Gets the counter for tracking scheduled tasks.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for scheduled tasks.</returns>
        IStatisticsCounter GetScheduleCounter();

        /// <summary>
        /// Gets the counter for tracking tasks scheduled on intervals.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for interval scheduled tasks.</returns>
        IStatisticsCounter GetScheduleOnIntervalCounter();

    }

}
