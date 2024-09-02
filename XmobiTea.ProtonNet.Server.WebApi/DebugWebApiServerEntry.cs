namespace XmobiTea.ProtonNet.Server.WebApi
{
    /// <summary>
    /// Represents an entry point for a debug web API server.
    /// Implements <see cref="IWebApiServerEntry"/> to provide server creation.
    /// </summary>
    public class DebugWebApiServerEntry : IWebApiServerEntry
    {
        /// <summary>
        /// Gets the startup settings for the server.
        /// </summary>
        public StartupSettings StartupSettings { get; }

        private DebugWebApiServerEntry(Builder builder)
        {
            this.StartupSettings = builder.StartupSettings;
        }

        /// <summary>
        /// Creates an instance of <see cref="WebApiServer"/> using the current startup settings.
        /// </summary>
        /// <returns>An instance of <see cref="IWebApiServer"/>.</returns>
        public IWebApiServer GetServer() => new WebApiServer(this.StartupSettings);

        /// <summary>
        /// Creates a new builder for <see cref="DebugWebApiServerEntry"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="Builder"/>.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder class for creating instances of <see cref="DebugWebApiServerEntry"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the startup settings for the server.
            /// </summary>
            public StartupSettings StartupSettings { get; set; }

            internal Builder() { }

            /// <summary>
            /// Sets the startup settings for the server.
            /// </summary>
            /// <param name="startupSettings">The startup settings to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetStartupSettings(StartupSettings startupSettings)
            {
                this.StartupSettings = startupSettings;
                return this;
            }

            /// <summary>
            /// Builds an instance of <see cref="DebugWebApiServerEntry"/> with the specified settings.
            /// </summary>
            /// <returns>A new instance of <see cref="DebugWebApiServerEntry"/>.</returns>
            public DebugWebApiServerEntry Build() => new DebugWebApiServerEntry(this);

        }

    }

}
