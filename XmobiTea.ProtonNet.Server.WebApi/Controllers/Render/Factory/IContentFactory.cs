using System.Collections.Generic;
using XmobiTea.Logging;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory
{
    /// <summary>
    /// Defines the contract for a content factory that can set up content 
    /// and retrieve it by name.
    /// </summary>
    /// <typeparam name="TContent">The type of content this factory handles.</typeparam>
    interface IContentFactory<TContent>
    {
        /// <summary>
        /// Sets up the content directory or source by specifying the path.
        /// </summary>
        /// <param name="path">The path where the content is located.</param>
        void SetupContent(string path);

        /// <summary>
        /// Retrieves the content by its name.
        /// </summary>
        /// <param name="name">The name of the content to retrieve.</param>
        /// <returns>The content associated with the given name.</returns>
        TContent GetContent(string name);

    }

    /// <summary>
    /// Provides a base implementation for a content factory that manages content.
    /// </summary>
    /// <typeparam name="TContent">The type of content this factory manages.</typeparam>
    abstract class AbstractContentFactory<TContent> : IContentFactory<TContent>
    {
        /// <summary>
        /// Gets the logger instance for logging operations related to the content factory.
        /// </summary>
        protected ILogger logger { get; }

        /// <summary>
        /// Gets the dictionary used to store content by name.
        /// </summary>
        protected IDictionary<string, TContent> contentDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractContentFactory{TContent}"/> class
        /// and sets up the logger and the content dictionary.
        /// </summary>
        public AbstractContentFactory()
        {
            this.logger = LogManager.GetLogger(this);
            this.contentDict = new Dictionary<string, TContent>();
        }

        /// <summary>
        /// Sets up the content directory or source by specifying the path.
        /// Abstract method to be implemented by subclasses.
        /// </summary>
        /// <param name="path">The path where the content is located.</param>
        public abstract void SetupContent(string path);

        /// <summary>
        /// Retrieves the content by its name.
        /// Abstract method to be implemented by subclasses.
        /// </summary>
        /// <param name="name">The name of the content to retrieve.</param>
        /// <returns>The content associated with the given name.</returns>
        public abstract TContent GetContent(string name);

    }

}
