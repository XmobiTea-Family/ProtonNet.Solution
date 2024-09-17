namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for view content, which includes the original
    /// content, processed content, session-related data, and layout override information.
    /// </summary>
    interface IViewContent
    {
        /// <summary>
        /// Gets the name of the view content.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the original content of the view without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        string OriginContent { get; } // no edit

        /// <summary>
        /// Gets the processed content of the view with partial renders and no session tags.
        /// </summary>
        string Content { get; } // Content with @renderPartial and no @session tag

        /// <summary>
        /// Gets the name of the layout to override the default layout, if applicable.
        /// </summary>
        string OverrideLayoutName { get; }

        /// <summary>
        /// Gets an array of session contents that are used in the view.
        /// </summary>
        ISessionContent[] SessionContents { get; }

    }

    /// <summary>
    /// Represents the implementation of <see cref="IViewContent"/>, which includes
    /// the name, original content, processed content, layout override, and session contents.
    /// </summary>
    class ViewContent : IViewContent
    {
        /// <summary>
        /// Gets the name of the view content.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the original content of the view without any modifications.
        /// This content is read-only and cannot be edited.
        /// </summary>
        public string OriginContent { get; }

        /// <summary>
        /// Gets or sets the processed content of the view with partial renders and no session tags.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the name of the layout to override the default layout, if applicable.
        /// </summary>
        public string OverrideLayoutName { get; set; }

        /// <summary>
        /// Gets or sets an array of session contents that are used in the view.
        /// </summary>
        public ISessionContent[] SessionContents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewContent"/> class with the specified name
        /// and original content.
        /// </summary>
        /// <param name="name">The name of the view content.</param>
        /// <param name="originContent">The original content of the view.</param>
        public ViewContent(string name, string originContent)
        {
            this.Name = name;
            this.OriginContent = originContent;
        }

    }

}
