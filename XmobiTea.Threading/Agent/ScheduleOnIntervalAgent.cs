using System;
using System.Collections.Generic;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading.Agent
{
    /// <summary>
    /// Manages and services tasks that are scheduled to run at regular intervals.
    /// </summary>
    class ScheduleOnIntervalAgent : IScheduleOnInterval, IService
    {
        /// <summary>
        /// Lock object for synchronizing access to the pending tasks queue.
        /// </summary>
        private object _lockScheduleOnIntervalTaskPending { get; }

        /// <summary>
        /// Queue of tasks that are scheduled to run at regular intervals.
        /// </summary>
        private Queue<ScheduleOnIntervalTask> scheduleOnIntervalTaskPending { get; }

        /// <summary>
        /// Counter used for tracking statistics of tasks.
        /// </summary>
        private IStatisticsCounterChange counter { get; }

        /// <summary>
        /// Enqueue interface used to manage the execution of tasks.
        /// </summary>
        private IEnqueueInternal enqueueInternal { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleOnIntervalAgent"/> class.
        /// </summary>
        /// <param name="counter">The statistics counter for tracking task metrics.</param>
        /// <param name="enqueueInternal">The enqueue interface for managing task execution.</param>
        public ScheduleOnIntervalAgent(IStatisticsCounterChange counter, IEnqueueInternal enqueueInternal)
        {
            this._lockScheduleOnIntervalTaskPending = new object();
            this.scheduleOnIntervalTaskPending = new Queue<ScheduleOnIntervalTask>();

            this.counter = counter;
            this.enqueueInternal = enqueueInternal;
        }

        /// <summary>
        /// Schedules a task to run at regular intervals.
        /// </summary>
        /// <param name="action">The action to be executed by the task.</param>
        /// <param name="firstInMs">The delay in milliseconds before the task is first executed.</param>
        /// <param name="regularInMs">The interval in milliseconds between consecutive executions.</param>
        /// <returns>An instance of <see cref="IScheduleOnIntervalTask"/> representing the scheduled task.</returns>
        public IScheduleOnIntervalTask ScheduleOnInterval(Action action, long firstInMs, int regularInMs)
        {
            this.counter.ChangePending(1);

            var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

            var answer = new ScheduleOnIntervalTask(action, regularInMs)
            {
                nextInvokeInMs = currentMilli + firstInMs
            };

            lock (this._lockScheduleOnIntervalTaskPending)
                this.scheduleOnIntervalTaskPending.Enqueue(answer);

            return answer;
        }

        /// <summary>
        /// Services pending tasks by executing them when their scheduled time arrives.
        /// </summary>
        public void Service()
        {
            ScheduleOnIntervalTask scheduleOnIntervalTask;

            lock (this._lockScheduleOnIntervalTaskPending)
            {
                if (this.scheduleOnIntervalTaskPending.Count == 0) return;

                scheduleOnIntervalTask = this.scheduleOnIntervalTaskPending.Dequeue();
            }

            if (!scheduleOnIntervalTask.isDispose)
            {
                var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

                if (scheduleOnIntervalTask.nextInvokeInMs > currentMilli)
                {
                    lock (this._lockScheduleOnIntervalTaskPending)
                        this.scheduleOnIntervalTaskPending.Enqueue(scheduleOnIntervalTask);
                }
                else
                {
                    this.counter.ChangeCalled(1);

                    this.enqueueInternal.Enqueue(scheduleOnIntervalTask);

                    scheduleOnIntervalTask.nextInvokeInMs = currentMilli + scheduleOnIntervalTask.GetRegularInMs();

                    lock (this._lockScheduleOnIntervalTaskPending)
                        this.scheduleOnIntervalTaskPending.Enqueue(scheduleOnIntervalTask);
                }
            }
        }

    }

}
