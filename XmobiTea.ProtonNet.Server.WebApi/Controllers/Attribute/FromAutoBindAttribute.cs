using System;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute
{
    /// <summary>
    /// Attribute used to automatically bind a method parameter 
    /// to a specific type or use default auto-binding.
    /// </summary>
    public sealed class FromAutoBindAttribute : FromHttpParamAttribute
    {
        /// <summary>
        /// Gets the type used for auto-binding. If null, the default binding will be applied.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromAutoBindAttribute"/> class
        /// with a specific type to bind.
        /// </summary>
        /// <param name="type">
        /// The specific type to bind to the parameter. 
        /// If not provided, the default auto-binding will be used.
        /// </param>
        public FromAutoBindAttribute(Type type) : base(null) => this.Type = type;

        /// <summary>
        /// Initializes a new instance of the <see cref="FromAutoBindAttribute"/> class
        /// with the default auto-binding.
        /// </summary>
        public FromAutoBindAttribute() : base(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FromAutoBindAttribute"/> class
        /// with the default auto-binding.
        /// </summary>
        private FromAutoBindAttribute(string name) : base(name) { }

    }

}
