using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render
{
    /// <summary>
    /// Interface for rendering views.
    /// </summary>
    public interface IViewRender
    {
        /// <summary>
        /// Retrieves the content of a specified view.
        /// </summary>
        /// <param name="view">The name of the view to retrieve.</param>
        /// <returns>The content of the specified view.</returns>
        string GetView(string view);

    }

    /// <summary>
    /// Implementation of IViewRender for managing and retrieving view content.
    /// </summary>
    class ViewRender : IViewRender
    {
        /// <summary>
        /// Dictionary to store view names and their associated content.
        /// </summary>
        private IDictionary<string, string> viewDict { get; }

        /// <summary>
        /// Initializes a new instance of the ViewRender class.
        /// </summary>
        public ViewRender()
        {
            this.viewDict = new Dictionary<string, string>();
        }

        /// <summary>
        /// Retrieves the content of a specified view from the dictionary.
        /// </summary>
        /// <param name="view">The name of the view to retrieve.</param>
        /// <returns>The content of the specified view.</returns>
        public string GetView(string view)
        {
            return this.viewDict[view];
        }

        /// <summary>
        /// Sets the content for a specified view in the dictionary.
        /// </summary>
        /// <param name="view">The name of the view to set content for.</param>
        /// <param name="content">The content to set for the view.</param>
        public void SetView(string view, string content)
        {
            this.viewDict[view] = content;
        }

    }

}
