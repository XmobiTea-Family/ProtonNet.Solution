namespace XmobiTea.Bean.Support
{
    /// <summary>
    /// Defines a method that is called immediately after the constructor is invoked.
    /// </summary>
    public interface IConstructorInvoke
    {
        /// <summary>
        /// Called immediately after the constructor is invoked.
        /// </summary>
        void OnConstructorInvoke();

    }

}
