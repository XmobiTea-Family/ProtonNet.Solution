namespace XmobiTea.Threading
{
    /// <summary>
    /// Defines the contract for controlling a fiber, including starting and disposing of it.
    /// </summary>
    public interface IFiberControl
    {
        /// <summary>
        /// Starts the fiber.
        /// </summary>
        void Start();

        /// <summary>
        /// Disposes of the fiber, releasing any resources.
        /// </summary>
        void Dispose();

    }

}
