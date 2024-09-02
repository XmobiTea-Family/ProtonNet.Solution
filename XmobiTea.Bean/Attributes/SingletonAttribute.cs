namespace XmobiTea.Bean.Attributes
{
    /// <summary>
    /// Attribute used to mark a class as a singleton.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class SingletonAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonAttribute"/> class.
        /// </summary>
        public SingletonAttribute()
        {

        }

    }

}
