using System;

namespace XmobiTea.Threading.Models
{
    /// <summary>
    /// Represents a single task that can be invoked.
    /// </summary>
    public interface ISingleTask
    {
        /// <summary>
        /// Executes the task and returns a boolean indicating success or failure.
        /// </summary>
        /// <returns>True if the task was executed successfully; otherwise, false.</returns>
        bool Invoke();

    }

    /// <summary>
    /// Provides an implementation of a single task with an action.
    /// </summary>
    class SingleTask : ISingleTask
    {
        /// <summary>
        /// The action to be executed by this task.
        /// </summary>
        protected Action action { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleTask"/> class.
        /// </summary>
        /// <param name="action">The action to be executed by the task.</param>
        public SingleTask(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Executes the task and returns a boolean indicating success or failure.
        /// </summary>
        /// <returns>True if the task was executed successfully; otherwise, false.</returns>
        public virtual bool Invoke()
        {
            if (this.action == null) return false;

            try
            {
                this.action.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
