namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for an element that is responsible for rendering 
    /// view data in a template, including its name and original content.
    /// </summary>
    interface IRenderViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element to be rendered.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the original content of the view data element before rendering.
        /// </summary>
        string OriginContent { get; }

    }

    /// <summary>
    /// Represents the implementation of <see cref="IRenderViewDataElement"/>, 
    /// holding the name and original content of the view data element to be rendered.
    /// </summary>
    class RenderViewDataElement : IRenderViewDataElement
    {
        /// <summary>
        /// Gets the name of the view data element to be rendered.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the original content of the view data element before rendering.
        /// </summary>
        public string OriginContent { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderViewDataElement"/> class
        /// with the specified name and original content.
        /// </summary>
        /// <param name="name">The name of the view data element to be rendered.</param>
        /// <param name="originContent">The original content of the view data element.</param>
        public RenderViewDataElement(string name, string originContent)
        {
            this.Name = name;
            this.OriginContent = originContent;
        }

    }

}
