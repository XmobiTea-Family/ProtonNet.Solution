namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for layout content, including the name, 
    /// original content, and processed content with partial renders.
    /// </summary>
    interface ILayoutContent
    {
        /// <summary>
        /// Gets the name of the layout content.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the original content of the layout without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        string OriginContent { get; } // no edit

        /// <summary>
        /// Gets the processed content of the layout, which may include partial renders.
        /// </summary>
        string Content { get; } // Content with @renderPartial

    }

    /// <summary>
    /// Represents the implementation of <see cref="ILayoutContent"/>, holding 
    /// both the original and processed content of a layout, including partial renders.
    /// </summary>
    class LayoutContent : ILayoutContent
    {
        /// <summary>
        /// Gets the name of the layout content.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the original content of the layout without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        public string OriginContent { get; }

        /// <summary>
        /// Gets or sets the processed content of the layout, which may include partial renders.
        /// </summary>
        public string Content { get; set; } // Content with @renderPartial

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutContent"/> class with the specified name and original content.
        /// </summary>
        /// <param name="name">The name of the layout content.</param>
        /// <param name="originContent">The original content of the layout.</param>
        public LayoutContent(string name, string originContent)
        {
            this.Name = name;
            this.OriginContent = originContent;
        }

    }

}
