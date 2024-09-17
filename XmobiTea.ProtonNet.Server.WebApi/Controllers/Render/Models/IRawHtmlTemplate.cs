namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for a raw HTML template, including the name, content, 
    /// renderable view data elements, and initialization elements.
    /// </summary>
    interface IRawHtmlTemplate
    {
        /// <summary>
        /// Gets the name of the HTML template.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the final content of the HTML template.
        /// </summary>
        string Content { get; set; }    // Final Content

        /// <summary>
        /// Gets or sets the elements that render view data within the template.
        /// </summary>
        IRenderViewDataElement[] RenderViewDataElements { get; set; }

        /// <summary>
        /// Gets or sets the initialization element used to set view data in the template.
        /// </summary>
        IPinitElement PinitElement { get; set; }

    }

    /// <summary>
    /// Represents the implementation of <see cref="IRawHtmlTemplate"/>, holding
    /// the name, content, renderable view data elements, and initialization elements for the template.
    /// </summary>
    class RawHtmlTemplate : IRawHtmlTemplate
    {
        /// <summary>
        /// Gets the name of the HTML template.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the final content of the HTML template.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the elements that render view data within the template.
        /// </summary>
        public IRenderViewDataElement[] RenderViewDataElements { get; set; }

        /// <summary>
        /// Gets or sets the initialization element used to set view data in the template.
        /// </summary>
        public IPinitElement PinitElement { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawHtmlTemplate"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the HTML template.</param>
        public RawHtmlTemplate(string name) => this.Name = name;

    }

}
