namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for a fiber that supports task enqueueing, scheduling, and interval scheduling.
    /// </summary>
    public interface IFiber : IEnqueue, ISchedule, IScheduleOnInterval
    {

    }

}
