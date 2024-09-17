namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models
{
    /// <summary>
    /// Defines the contract for a view that holds the HTML content 
    /// to be rendered.
    /// </summary>
    interface IView
    {
        /// <summary>
        /// Gets or sets the HTML content of the view.
        /// </summary>
        string Html { get; set; }

    }

    /// <summary>
    /// Represents a concrete implementation of the <see cref="IView"/> interface,
    /// which holds the HTML content to be rendered.
    /// </summary>
    class View : IView
    {
        /// <summary>
        /// Gets or sets the HTML content of the view.
        /// </summary>
        public string Html { get; set; }

    }

}
