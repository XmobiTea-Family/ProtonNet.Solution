using System;
using System.Threading;
using XmobiTea.Threading.Agent;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Represents a fiber that uses a dedicated thread for task execution and scheduling.
    /// </summary>
    public class ThreadFiber : IFiber, IFiberControl, IFiberStatisticsCounter
    {
        private bool _disposed { get; set; }

        private IStatisticsCounter enqueueCounter { get; }
        private IStatisticsCounter scheduleCounter { get; }
        private IStatisticsCounter scheduleOnIntervalCounter { get; }

        private IEnqueue enqueue { get; }
        private ISchedule schedule { get; }
        private IScheduleOnInterval scheduleOnInterval { get; }

        private Thread thread { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadFiber"/> class.
        /// </summary>
        /// <param name="name">The name of the thread.</param>
        public ThreadFiber(string name)
        {
            var enqueueCounterAgent = new StatisticsCounterAgent();
            var enqueueAgent = new EnqueueAgent(enqueueCounterAgent);

            this.enqueueCounter = enqueueCounterAgent;
            this.enqueue = enqueueAgent;

            var scheduleCounterAgent = new StatisticsCounterAgent();
            var scheduleAgent = new ScheduleAgent(scheduleCounterAgent, enqueueAgent);

            this.scheduleCounter = scheduleCounterAgent;
            this.schedule = scheduleAgent;

            var scheduleOnIntervalCounterAgent = new StatisticsCounterAgent();
            var scheduleOnIntervalAgent = new ScheduleOnIntervalAgent(scheduleOnIntervalCounterAgent, enqueueAgent);

            this.scheduleOnIntervalCounter = scheduleOnIntervalCounterAgent;
            this.scheduleOnInterval = scheduleOnIntervalAgent;

            this.thread = new Thread(() =>
            {
                while (true)
                {
                    if (this._disposed) break;

                    scheduleAgent.Service();
                    scheduleOnIntervalAgent.Service();
                    enqueueAgent.Service();

                    Thread.Sleep(10);
                }
            });

            this.thread.Name = name;
        }

        /// <summary>
        /// Gets the counter for tracking enqueued tasks.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for enqueued tasks.</returns>
        public IStatisticsCounter GetEnqueueCounter() => this.enqueueCounter;

        /// <summary>
        /// Gets the counter for tracking scheduled tasks.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for scheduled tasks.</returns>
        public IStatisticsCounter GetScheduleCounter() => this.scheduleCounter;

        /// <summary>
        /// Gets the counter for tracking tasks scheduled on intervals.
        /// </summary>
        /// <returns>An instance of <see cref="IStatisticsCounter"/> for interval scheduled tasks.</returns>
        public IStatisticsCounter GetScheduleOnIntervalCounter() => this.scheduleOnIntervalCounter;

        /// <summary>
        /// Enqueues an action to be executed.
        /// </summary>
        /// <param name="action">The action to be enqueued.</param>
        /// <returns>An instance of <see cref="ISingleTask"/> representing the enqueued task.</returns>
        public ISingleTask Enqueue(Action action) => this.enqueue.Enqueue(action);

        /// <summary>
        /// Schedules an action to be executed after a specified delay.
        /// </summary>
        /// <param name="action">The action to be scheduled.</param>
        /// <param name="firstInMs">The delay in milliseconds before the action is executed.</param>
        /// <returns>An instance of <see cref="IScheduleTask"/> representing the scheduled task.</returns>
        public IScheduleTask Schedule(Action action, long firstInMs) => this.schedule.Schedule(action, firstInMs);

        /// <summary>
        /// Schedules an action to be executed at regular intervals.
        /// </summary>
        /// <param name="action">The action to be scheduled.</param>
        /// <param name="firstInMs">The delay in milliseconds before the first execution of the action.</param>
        /// <param name="regularInMs">The interval in milliseconds between subsequent executions of the action.</param>
        /// <returns>An instance of <see cref="IScheduleOnIntervalTask"/> representing the scheduled task.</returns>
        public IScheduleOnIntervalTask ScheduleOnInterval(Action action, long firstInMs, int regularInMs) => this.scheduleOnInterval.ScheduleOnInterval(action, firstInMs, regularInMs);

        /// <summary>
        /// Starts the thread for processing tasks.
        /// </summary>
        public void Start()
        {
            this.thread.Start();
        }

        /// <summary>
        /// Disposes of the fiber, releasing any resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this.thread?.Interrupt();
            }

            this._disposed = true;
        }

    }

}
