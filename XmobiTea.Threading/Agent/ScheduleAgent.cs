using System;
using System.Collections.Generic;
using XmobiTea.Threading.Models;
using XmobiTea.Threading.StatisticsCounter;

namespace XmobiTea.Threading.Agent
{
    /// <summary>
    /// Manages and services tasks that are scheduled to run at a specific time.
    /// </summary>
    class ScheduleAgent : ISchedule, IService
    {
        /// <summary>
        /// Lock object for synchronizing access to the pending tasks queue.
        /// </summary>
        private object _lockScheduleTaskPending { get; }

        /// <summary>
        /// Queue of tasks that are scheduled to run at a specific time.
        /// </summary>
        private Queue<ScheduleTask> scheduleTaskPending { get; }

        /// <summary>
        /// Counter used for tracking statistics of tasks.
        /// </summary>
        private IStatisticsCounterChange counter { get; }

        /// <summary>
        /// Enqueue interface used to manage the execution of tasks.
        /// </summary>
        private IEnqueueInternal enqueueInternal { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleAgent"/> class.
        /// </summary>
        /// <param name="counter">The statistics counter for tracking task metrics.</param>
        /// <param name="enqueueInternal">The enqueue interface for managing task execution.</param>
        public ScheduleAgent(IStatisticsCounterChange counter, IEnqueueInternal enqueueInternal)
        {
            this._lockScheduleTaskPending = new object();
            this.scheduleTaskPending = new Queue<ScheduleTask>();

            this.counter = counter;
            this.enqueueInternal = enqueueInternal;
        }

        /// <summary>
        /// Schedules a task to run after a specified delay.
        /// </summary>
        /// <param name="action">The action to be executed by the task.</param>
        /// <param name="firstInMs">The delay in milliseconds before the task is first executed.</param>
        /// <returns>An instance of <see cref="IScheduleTask"/> representing the scheduled task.</returns>
        public IScheduleTask Schedule(Action action, long firstInMs)
        {
            this.counter.ChangePending(1);

            var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

            var answer = new ScheduleTask(action)
            {
                nextInvokeInMs = currentMilli + firstInMs
            };

            this.scheduleTaskPending.Enqueue(answer);

            return answer;
        }

        /// <summary>
        /// Services pending tasks by executing them when their scheduled time arrives.
        /// </summary>
        public void Service()
        {
            ScheduleTask scheduleTask;

            lock (this._lockScheduleTaskPending)
            {
                if (this.scheduleTaskPending.Count == 0) return;

                scheduleTask = this.scheduleTaskPending.Dequeue();
            }

            var currentMilli = System.DateTime.UtcNow.Ticks / 10000;

            if (scheduleTask.nextInvokeInMs > currentMilli)
            {
                lock (this._lockScheduleTaskPending)
                    this.scheduleTaskPending.Enqueue(scheduleTask);
            }
            else
            {
                this.counter.ChangePending(-1);
                this.counter.ChangeCalled(1);

                this.enqueueInternal.Enqueue(scheduleTask);
            }
        }

    }

}
