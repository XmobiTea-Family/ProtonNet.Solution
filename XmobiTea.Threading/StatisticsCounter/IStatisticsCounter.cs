using System.Threading;

namespace XmobiTea.Threading.StatisticsCounter
{
    /// <summary>
    /// Defines methods for accessing statistics related to task execution.
    /// </summary>
    public interface IStatisticsCounter
    {
        /// <summary>
        /// Gets the count of pending tasks.
        /// </summary>
        /// <returns>The number of pending tasks.</returns>
        int GetPendingCount();

        /// <summary>
        /// Gets the count of called tasks.
        /// </summary>
        /// <returns>The number of called tasks.</returns>
        int GetCalledCount();

        /// <summary>
        /// Gets the count of failed tasks.
        /// </summary>
        /// <returns>The number of failed tasks.</returns>
        int GetFailedCount();

        /// <summary>
        /// Clears the statistics counters.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Defines methods for changing statistics counters.
    /// </summary>
    interface IStatisticsCounterChange
    {
        /// <summary>
        /// Updates the count of failed tasks.
        /// </summary>
        /// <param name="amount">The amount to change the failed count by.</param>
        void ChangeFailed(int amount);

        /// <summary>
        /// Updates the count of called tasks.
        /// </summary>
        /// <param name="amount">The amount to change the called count by.</param>
        void ChangeCalled(int amount);

        /// <summary>
        /// Updates the count of pending tasks.
        /// </summary>
        /// <param name="amount">The amount to change the pending count by.</param>
        void ChangePending(int amount);
    }

    /// <summary>
    /// Implements statistics counters with thread-safe operations.
    /// </summary>
    class StatisticsCounterAgent : IStatisticsCounterChange, IStatisticsCounter
    {
        private int failedCount;
        private int pendingCount;
        private int calledCount;

        /// <summary>
        /// Gets the count of failed tasks.
        /// </summary>
        /// <returns>The number of failed tasks.</returns>
        public int GetFailedCount() => this.failedCount;

        /// <summary>
        /// Gets the count of called tasks.
        /// </summary>
        /// <returns>The number of called tasks.</returns>
        public int GetCalledCount() => this.calledCount;

        /// <summary>
        /// Gets the count of pending tasks.
        /// </summary>
        /// <returns>The number of pending tasks.</returns>
        public int GetPendingCount() => this.pendingCount;

        /// <summary>
        /// Clears the statistics counters.
        /// </summary>
        public void Clear()
        {
            Interlocked.Exchange(ref this.failedCount, 0);
            Interlocked.Exchange(ref this.calledCount, 0);
            Interlocked.Exchange(ref this.pendingCount, 0);
        }

        /// <summary>
        /// Updates the count of failed tasks.
        /// </summary>
        /// <param name="amount">The amount to change the failed count by.</param>
        public void ChangeFailed(int amount) => Interlocked.Add(ref this.failedCount, amount);

        /// <summary>
        /// Updates the count of called tasks.
        /// </summary>
        /// <param name="amount">The amount to change the called count by.</param>
        public void ChangeCalled(int amount) => Interlocked.Add(ref this.calledCount, amount);

        /// <summary>
        /// Updates the count of pending tasks.
        /// </summary>
        /// <param name="amount">The amount to change the pending count by.</param>
        public void ChangePending(int amount) => Interlocked.Add(ref this.pendingCount, amount);
    }
}
