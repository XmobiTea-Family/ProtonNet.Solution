namespace XmobiTea.ProtonNet.Server.WebApi
{
    /// <summary>
    /// Interface for web API server entry.
    /// </summary>
    public interface IWebApiServerEntry
    {
        /// <summary>
        /// Gets the web API server instance.
        /// </summary>
        /// <returns>An instance of <see cref="IWebApiServer"/>.</returns>
        IWebApiServer GetServer();
    }

    /// <summary>
    /// Implementation of the <see cref="IWebApiServerEntry"/> interface.
    /// </summary>
    public class WebApiServerEntry : IWebApiServerEntry
    {
        /// <summary>
        /// Gets the startup settings.
        /// </summary>
        public StartupSettings StartupSettings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiServerEntry"/> class.
        /// </summary>
        /// <param name="builder">The builder instance used to construct this instance.</param>
        private WebApiServerEntry(Builder builder)
        {
            this.StartupSettings = builder.StartupSettings;
        }

        /// <summary>
        /// Creates and returns a new instance of <see cref="IWebApiServer"/>.
        /// </summary>
        /// <returns>A new <see cref="IWebApiServer"/> instance.</returns>
        public IWebApiServer GetServer() => new WebApiServer(this.StartupSettings);

        /// <summary>
        /// Creates a new instance of the <see cref="Builder"/> class.
        /// </summary>
        /// <returns>A new <see cref="Builder"/> instance.</returns>
        public static Builder NewBuilder() => new Builder();

        /// <summary>
        /// Builder for WebApiServerEntry
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Gets or sets the startup settings.
            /// </summary>
            public StartupSettings StartupSettings { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder() { }

            /// <summary>
            /// Sets the startup settings.
            /// </summary>
            /// <param name="startupSettings">The startup settings to set.</param>
            /// <returns>The current <see cref="Builder"/> instance.</returns>
            public Builder SetStartupSettings(StartupSettings startupSettings)
            {
                this.StartupSettings = startupSettings;
                return this;
            }

            /// <summary>
            /// Builds a new instance of the <see cref="WebApiServerEntry"/> class.
            /// </summary>
            /// <returns>A new <see cref="WebApiServerEntry"/> instance.</returns>
            public WebApiServerEntry Build() => new WebApiServerEntry(this);

        }

    }

}
