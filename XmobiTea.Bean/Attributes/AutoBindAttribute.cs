namespace XmobiTea.Bean.Attributes
{
    /// <summary>
    /// Attribute used to automatically bind a field or property to a specific type.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false)]
    public class AutoBindAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the type to which the field or property should be bound.
        /// </summary>
        public System.Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoBindAttribute"/> class with the specified type.
        /// </summary>
        /// <param name="type">The type to bind to.</param>
        public AutoBindAttribute(System.Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoBindAttribute"/> class with no specific type.
        /// </summary>
        public AutoBindAttribute() : this(null)
        {

        }

    }

}
