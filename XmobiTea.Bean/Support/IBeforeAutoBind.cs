namespace XmobiTea.Bean.Support
{
    /// <summary>
    /// Defines a method that is called before the auto-binding process begins.
    /// </summary>
    public interface IBeforeAutoBind
    {
        /// <summary>
        /// Called before the auto-binding process begins.
        /// </summary>
        void OnBeforeAutoBind();

    }

}
