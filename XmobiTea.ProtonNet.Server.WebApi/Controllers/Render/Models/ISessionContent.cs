namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for session content, including the name, 
    /// original content, and processed content with partial renders.
    /// </summary>
    interface ISessionContent
    {
        /// <summary>
        /// Gets the name of the session content.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the original content of the session without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        string OriginContent { get; } // no edit

        /// <summary>
        /// Gets the processed content of the session, which may include partial renders.
        /// </summary>
        string Content { get; } // Content with @renderPartial

    }

    /// <summary>
    /// Represents the implementation of <see cref="ISessionContent"/>, holding 
    /// the name, original content, and processed content of a session, 
    /// including partial renders.
    /// </summary>
    class SessionContent : ISessionContent
    {
        /// <summary>
        /// Gets the name of the session content.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the original content of the session without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        public string OriginContent { get; }

        /// <summary>
        /// Gets or sets the processed content of the session, which may include partial renders.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContent"/> class with the specified
        /// name and original content.
        /// </summary>
        /// <param name="name">The name of the session content.</param>
        /// <param name="originContent">The original content of the session.</param>
        public SessionContent(string name, string originContent)
        {
            this.Name = name;
            this.OriginContent = originContent;
        }

    }

}
