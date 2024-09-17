namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for partial content, including the name, 
    /// original content, and processed content with partial renders.
    /// </summary>
    interface IPartialContent
    {
        /// <summary>
        /// Gets the name of the partial content.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the original content of the partial without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        string OriginContent { get; } // no edit

        /// <summary>
        /// Gets the processed content of the partial, which may include other partial renders.
        /// </summary>
        string Content { get; } // Content with @renderPartial

    }

    /// <summary>
    /// Represents the implementation of <see cref="IPartialContent"/>, holding 
    /// both the original and processed content of a partial, including partial renders.
    /// </summary>
    class PartialContent : IPartialContent
    {
        /// <summary>
        /// Gets the name of the partial content.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the original content of the partial without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        public string OriginContent { get; }

        /// <summary>
        /// Gets or sets the processed content of the partial, which may include other partial renders.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialContent"/> class with the specified name and original content.
        /// </summary>
        /// <param name="name">The name of the partial content.</param>
        /// <param name="originContent">The original content of the partial.</param>
        public PartialContent(string name, string originContent)
        {
            this.Name = name;
            this.OriginContent = originContent;
        }

    }

}
