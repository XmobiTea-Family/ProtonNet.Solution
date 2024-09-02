using System;
using System.Collections.Generic;
using System.Threading;
using XmobiTea.Threading.Agent;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading
{
    /// <summary>
    /// Represents a pool of fibers that can enqueue tasks, schedule tasks, and schedule tasks at regular intervals.
    /// </summary>
    public class PoolFiber : IFiber, IFiberControl, IFiberStatisticsCounter
    {
        private bool _disposed { get; set; }

        private IStatisticsCounter enqueueCounter { get; }
        private IStatisticsCounter scheduleCounter { get; }
        private IStatisticsCounter scheduleOnIntervalCounter { get; }

        private IEnqueue enqueue { get; }
        private ISchedule schedule { get; }
        private IScheduleOnInterval scheduleOnInterval { get; }

        private PoolFiberEnqueueAgent enqueueAgent { get; }
        private PoolFiberScheduleAgent scheduleAgent { get; }
        private PoolFiberScheduleOnIntervalAgent scheduleOnIntervalAgent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolFiber"/> class.
        /// </summary>
        public PoolFiber()
        {
            var enqueueCounterAgent = new StatisticsCounterAgent();
            this.enqueueAgent = new PoolFiberEnqueueAgent(enqueueCounterAgent);
            this.enqueueCounter = enqueueCounterAgent;
            this.enqueue = this.enqueueAgent;

            var scheduleCounterAgent = new StatisticsCounterAgent();
            this.scheduleAgent = new PoolFiberScheduleAgent(scheduleCounterAgent, this.enqueueAgent);
            this.scheduleCounter = scheduleCounterAgent;
            this.schedule = this.scheduleAgent;

            var scheduleOnIntervalCounterAgent = new StatisticsCounterAgent();
            this.scheduleOnIntervalAgent = new PoolFiberScheduleOnIntervalAgent(scheduleOnIntervalCounterAgent, this.enqueueAgent);
            this.scheduleOnIntervalCounter = scheduleOnIntervalCounterAgent;
            this.scheduleOnInterval = this.scheduleOnIntervalAgent;
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
        /// Starts the fiber.
        /// </summary>
        public void Start()
        {
            // Implementation for starting the fiber.
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

            this._disposed = true;

            this.scheduleAgent.Dispose();
            this.scheduleOnIntervalAgent.Dispose();
        }

        class PoolFiberEnqueueAgent : IEnqueueInternal, IEnqueue
        {
            private IStatisticsCounterChange counter { get; }

            public PoolFiberEnqueueAgent(IStatisticsCounterChange counter)
            {
                this.counter = counter;
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

            public void Enqueue(SingleTask task)
            {
                this.counter.ChangePending(1);

                ThreadPool.QueueUserWorkItem((obj) =>
                {
                    this.counter.ChangePending(-1);
                    this.counter.ChangeCalled(1);

                    if (!task.Invoke()) this.counter.ChangeFailed(1);
                });
            }
        }

        class TimerActions
        {
            private IList<TimerAction> timerActionLst;

            public TimerActions()
            {
                this.timerActionLst = new List<TimerAction>();
            }

            public void Add(TimerAction timerAction)
            {
                lock (this.timerActionLst)
                    this.timerActionLst.Add(timerAction);
            }

            public void Remove(TimerAction timerAction)
            {
                lock (this.timerActionLst)
                    this.timerActionLst.Remove(timerAction);
            }

            /// <summary>
            /// Disposes of all timer actions and releases any resources.
            /// </summary>
            public void Dispose()
            {
                IEnumerable<TimerAction> cloneTimerActionLst;

                lock (this.timerActionLst)
                {
                    if (this.timerActionLst == null) return;

                    cloneTimerActionLst = new List<TimerAction>(this.timerActionLst);
                }

                foreach (var timerAction in cloneTimerActionLst)
                {
                    timerAction.Dispose();
                }

                Interlocked.Exchange<IList<TimerAction>>(ref this.timerActionLst, null)?.Clear();
            }
        }

        class PoolFiberScheduleAgent : ISchedule
        {
            private IStatisticsCounterChange counter { get; }
            private IEnqueueInternal enqueueInternal { get; }
            private TimerActions timerActions;

            public PoolFiberScheduleAgent(IStatisticsCounterChange counter, IEnqueueInternal enqueueInternal)
            {
                this.counter = counter;
                this.enqueueInternal = enqueueInternal;
                this.timerActions = new TimerActions();
            }

            /// <summary>
            /// Schedules an action to be executed after a specified delay.
            /// </summary>
            /// <param name="action">The action to be scheduled.</param>
            /// <param name="firstInMs">The delay in milliseconds before the action is executed.</param>
            /// <returns>An instance of <see cref="IScheduleTask"/> representing the scheduled task.</returns>
            public IScheduleTask Schedule(Action action, long firstInMs)
            {
                this.counter.ChangePending(1);

                var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

                var answer = new ScheduleTask(action)
                {
                    nextInvokeInMs = currentMilli + firstInMs
                };

                var timerAction = new TimerAction(this.counter, this.enqueueInternal, this.timerActions, answer, firstInMs, Timeout.Infinite);
                this.timerActions.Add(timerAction);
                timerAction.Schedule();

                return answer;
            }

            public void Dispose()
            {
                Interlocked.Exchange<TimerActions>(ref this.timerActions, null)?.Dispose();
            }
        }

        class TimerAction
        {
            private IStatisticsCounterChange counter { get; }
            private IEnqueueInternal enqueueInternal { get; }
            private ScheduleTask task { get; }
            private long firstInMs { get; }
            private int regularInMs { get; }
            private TimerActions timerActions { get; }
            private bool isDispose { get; set; }

            private Timer timer;
            public TimerAction(IStatisticsCounterChange counter, IEnqueueInternal enqueueInternal, TimerActions timerActions, ScheduleTask task, long firstInMs, int regularInMs)
            {
                this.counter = counter;
                this.enqueueInternal = enqueueInternal;
                this.timerActions = timerActions;

                this.task = task;
                this.firstInMs = firstInMs;
                this.regularInMs = regularInMs;
            }

            public void Schedule()
            {
                this.timer = new Timer((obj) => this.ExecuteOnTimerThread(), null, this.firstInMs, this.regularInMs);
            }

            private void ExecuteOnTimerThread()
            {
                if (this.isDispose)
                {
                    return;
                }

                this.counter.ChangePending(-1);

                if (this.task.isDispose || this.regularInMs == -1)
                {
                    Interlocked.Exchange<Timer>(ref this.timer, null)?.Dispose();

                    this.timerActions.Remove(this);

                    if (this.task.isDispose) return;
                }

                this.counter.ChangeCalled(1);
                this.enqueueInternal.Enqueue(this.task);

                if (this.regularInMs != -1)
                {
                    this.Schedule();
                    this.task.nextInvokeInMs = System.DateTime.UtcNow.Ticks / 10000 + this.regularInMs;
                }
            }

            /// <summary>
            /// Disposes of the timer action, releasing any resources.
            /// </summary>
            public void Dispose()
            {
                this.isDispose = true;
                Interlocked.Exchange<Timer>(ref this.timer, null)?.Dispose();
            }
        }

        class PoolFiberScheduleOnIntervalAgent : IScheduleOnInterval
        {
            private IStatisticsCounterChange counter { get; }
            private IEnqueueInternal enqueueInternal { get; }
            private TimerActions timerActions;

            public PoolFiberScheduleOnIntervalAgent(IStatisticsCounterChange counter, IEnqueueInternal enqueueInternal)
            {
                this.counter = counter;
                this.enqueueInternal = enqueueInternal;
                this.timerActions = new TimerActions();
            }

            /// <summary>
            /// Schedules an action to be executed at regular intervals.
            /// </summary>
            /// <param name="action">The action to be scheduled.</param>
            /// <param name="firstInMs">The delay in milliseconds before the first execution of the action.</param>
            /// <param name="regularInMs">The interval in milliseconds between subsequent executions of the action.</param>
            /// <returns>An instance of <see cref="IScheduleOnIntervalTask"/> representing the scheduled task.</returns>
            public IScheduleOnIntervalTask ScheduleOnInterval(Action action, long firstInMs, int regularInMs)
            {
                this.counter.ChangePending(1);

                var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

                var answer = new ScheduleOnIntervalTask(action, regularInMs)
                {
                    nextInvokeInMs = currentMilli + firstInMs
                };

                var timerAction = new TimerAction(this.counter, this.enqueueInternal, this.timerActions, answer, firstInMs, regularInMs);
                this.timerActions.Add(timerAction);
                timerAction.Schedule();

                return answer;
            }

            public void Dispose()
            {
                Interlocked.Exchange<TimerActions>(ref this.timerActions, null)?.Dispose();
            }

        }

    }
}
