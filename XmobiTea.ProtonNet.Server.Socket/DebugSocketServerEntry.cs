namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Represents a debug implementation of <see cref="ISocketServerEntry"/> for creating and configuring 
    /// a socket server specifically for debugging purposes.
    /// </summary>
    public class DebugSocketServerEntry : ISocketServerEntry
    {
        /// <summary>
        /// Gets the startup settings used for configuring the debug socket server.
        /// </summary>
        public StartupSettings StartupSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugSocketServerEntry"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the debug socket server entry.</param>
        private DebugSocketServerEntry(Builder builder)
        {
            this.StartupSettings = builder.StartupSettings;
        }

        /// <summary>
        /// Gets an instance of <see cref="ISocketServer"/> configured with the specified startup settings 
        /// for debugging purposes.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketServer"/>.</returns>
        public ISocketServer GetServer() => new SocketServer(this.StartupSettings);

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="DebugSocketServerEntry"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="DebugSocketServerEntry"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the startup settings used to configure the debug socket server.
            /// </summary>
            public StartupSettings StartupSettings { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the startup settings used to configure the debug socket server.
            /// </summary>
            /// <param name="startupSettings">The startup settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetStartupSettings(StartupSettings startupSettings)
            {
                this.StartupSettings = startupSettings;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="DebugSocketServerEntry"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="DebugSocketServerEntry"/>.</returns>
            public DebugSocketServerEntry Build() => new DebugSocketServerEntry(this);

        }

    }

}
