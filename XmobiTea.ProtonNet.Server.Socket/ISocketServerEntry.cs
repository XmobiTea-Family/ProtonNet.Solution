namespace XmobiTea.ProtonNet.Server.Socket
{
    /// <summary>
    /// Represents an entry point for obtaining an instance of <see cref="ISocketServer"/>.
    /// </summary>
    public interface ISocketServerEntry
    {
        /// <summary>
        /// Gets an instance of <see cref="ISocketServer"/> configured with the specified startup settings.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketServer"/>.</returns>
        ISocketServer GetServer();

    }

    /// <summary>
    /// Provides an implementation of <see cref="ISocketServerEntry"/> for creating and configuring socket servers.
    /// </summary>
    public class SocketServerEntry : ISocketServerEntry
    {
        /// <summary>
        /// Gets the startup settings used for configuring the socket server.
        /// </summary>
        public StartupSettings StartupSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServerEntry"/> class using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the socket server entry.</param>
        private SocketServerEntry(Builder builder)
        {
            this.StartupSettings = builder.StartupSettings;
        }

        /// <summary>
        /// Gets an instance of <see cref="ISocketServer"/> configured with the specified startup settings.
        /// </summary>
        /// <returns>An instance of <see cref="ISocketServer"/>.</returns>
        public ISocketServer GetServer() => new SocketServer(this.StartupSettings);

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class for constructing 
        /// <see cref="SocketServerEntry"/> instances.
        /// </summary>
        /// <returns>A new instance of the <see cref="Builder"/> class.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for constructing instances of <see cref="SocketServerEntry"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the startup settings used to configure the socket server.
            /// </summary>
            public StartupSettings StartupSettings { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the startup settings used to configure the socket server.
            /// </summary>
            /// <param name="startupSettings">The startup settings.</param>
            /// <returns>The current instance of the <see cref="Builder"/>.</returns>
            public Builder SetStartupSettings(StartupSettings startupSettings)
            {
                this.StartupSettings = startupSettings;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="SocketServerEntry"/> using the configured settings.
            /// </summary>
            /// <returns>A new instance of <see cref="SocketServerEntry"/>.</returns>
            public SocketServerEntry Build() => new SocketServerEntry(this);

        }

    }

}
