using System;
using System.Collections.Generic;
using System.Threading;
using XmobiTea.Threading.Agent;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Represents a fiber that uses a round-robin scheduling strategy for task execution.
    /// </summary>
    public class RoundRobinFiber : IFiber, IFiberControl, IFiberStatisticsCounter
    {
        private static Thread loopThread { get; }
        private static IList<RoundRobinFiber> roundRobinFiberLst { get; }

        static RoundRobinFiber()
        {
            roundRobinFiberLst = new List<RoundRobinFiber>();

            loopThread = new Thread(() =>
            {
                while (true)
                {
                    IEnumerable<RoundRobinFiber> cloneRoundRobinFiberLst;

                    lock (roundRobinFiberLst)
                    {
                        if (roundRobinFiberLst.Count == 0) continue;

                        cloneRoundRobinFiberLst = new List<RoundRobinFiber>(roundRobinFiberLst);
                    }

                    foreach (var roundRobinFiber in cloneRoundRobinFiberLst)
                    {
                        if (roundRobinFiber != null)
                        {
                            try
                            {
                                if (roundRobinFiber._disposed) continue;

                                roundRobinFiber.scheduleAgent.Service();
                                roundRobinFiber.scheduleOnIntervalAgent.Service();
                            }
                            catch
                            { }
                        }
                    }

                    Thread.Sleep(10);
                }
            });

            loopThread.Start();
            loopThread.Name = "RoundRobinFiber_LoopThread";
        }

        private bool _disposed { get; set; }

        private IStatisticsCounter enqueueCounter { get; }
        private IStatisticsCounter scheduleCounter { get; }
        private IStatisticsCounter scheduleOnIntervalCounter { get; }

        private IEnqueue enqueue { get; }
        private ISchedule schedule { get; }
        private IScheduleOnInterval scheduleOnInterval { get; }

        private RoundRobinEnqueueAgent enqueueAgent { get; }
        private ScheduleAgent scheduleAgent { get; }
        private ScheduleOnIntervalAgent scheduleOnIntervalAgent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRobinFiber"/> class.
        /// </summary>
        /// <param name="name">The name of the fiber.</param>
        /// <param name="count">The number of child threads to use.</param>
        public RoundRobinFiber(string name, int count)
        {
            var enqueueCounterAgent = new StatisticsCounterAgent();
            this.enqueueAgent = new RoundRobinEnqueueAgent(enqueueCounterAgent, name + "_child", count);

            this.enqueueCounter = enqueueCounterAgent;
            this.enqueue = this.enqueueAgent;

            var scheduleCounterAgent = new StatisticsCounterAgent();
            this.scheduleAgent = new ScheduleAgent(scheduleCounterAgent, this.enqueueAgent);

            this.scheduleCounter = scheduleCounterAgent;
            this.schedule = this.scheduleAgent;

            var scheduleOnIntervalCounterAgent = new StatisticsCounterAgent();
            this.scheduleOnIntervalAgent = new ScheduleOnIntervalAgent(scheduleOnIntervalCounterAgent, this.enqueueAgent);

            this.scheduleOnIntervalCounter = scheduleOnIntervalCounterAgent;
            this.scheduleOnInterval = this.scheduleOnIntervalAgent;
        }

        /// <summary>
        /// Starts the fiber and adds it to the list of round-robin fibers.
        /// </summary>
        public void Start()
        {
            lock (roundRobinFiberLst)
                roundRobinFiberLst.Add(this);

            this.enqueueAgent.Start();
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
                lock (roundRobinFiberLst)
                    roundRobinFiberLst.Remove(this);

                this.enqueueAgent.Dispose();
            }

            this._disposed = true;
        }

    }

    /// <summary>
    /// Represents an enqueue agent that uses a round-robin scheduling strategy for task execution.
    /// </summary>
    class RoundRobinEnqueueAgent : IEnqueueInternal, IEnqueue
    {
        private object _lockQueueAgents { get; }
        private Queue<EnqueueAgent> queueAgents { get; }

        private EnqueueAgent[] enqueueAgents { get; }
        private Thread[] childThreads { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundRobinEnqueueAgent"/> class.
        /// </summary>
        /// <param name="counter">The counter for tracking enqueue statistics.</param>
        /// <param name="name">The name of the agent.</param>
        /// <param name="count">The number of child threads to use.</param>
        public RoundRobinEnqueueAgent(IStatisticsCounterChange counter, string name, int count)
        {
            this._lockQueueAgents = new object();
            this.queueAgents = new Queue<EnqueueAgent>();

            this.enqueueAgents = new EnqueueAgent[count];
            this.childThreads = new Thread[count];

            for (var i = 0; i < count; i++)
            {
                var enqueueAgent = new EnqueueAgent(counter);
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                        enqueueAgent.Service();

                        Thread.Sleep(10);
                    }
                });

                thread.Name = name + "_" + i;

                this.enqueueAgents[i] = enqueueAgent;
                this.childThreads[i] = thread;

                this.queueAgents.Enqueue(enqueueAgent);
            }
        }

        /// <summary>
        /// Enqueues an action to be executed.
        /// </summary>
        /// <param name="action">The action to be enqueued.</param>
        /// <returns>An instance of <see cref="ISingleTask"/> representing the enqueued task.</returns>
        public ISingleTask Enqueue(Action action)
        {
            var answer = new SingleTask(action);

            this.Enqueue(answer);

            return answer;
        }

        /// <summary>
        /// Enqueues a single task to be executed using round-robin scheduling.
        /// </summary>
        /// <param name="task">The task to be enqueued.</param>
        public void Enqueue(SingleTask task)
        {
            lock (this._lockQueueAgents)
            {
                var enqueueAgent = this.queueAgents.Dequeue();

                enqueueAgent.Enqueue(task);

                this.queueAgents.Enqueue(enqueueAgent);
            }
        }

        /// <summary>
        /// Starts all child threads for processing enqueued tasks.
        /// </summary>
        public void Start()
        {
            foreach (var thread in this.childThreads)
                thread.Start();
        }

        /// <summary>
        /// Disposes of all child threads and releases any resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var thread in this.childThreads)
            {
                try
                {
                    thread.Interrupt();
                }
                catch { }
            }
        }

    }

}
