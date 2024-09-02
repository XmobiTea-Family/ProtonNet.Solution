using System.Collections.Generic;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render
{
    /// <summary>
    /// Defines an interface for rendering layouts.
    /// </summary>
    public interface ILayoutRender
    {
        /// <summary>
        /// Retrieves the layout content for the specified layout key.
        /// </summary>
        /// <param name="layout">The key of the layout to retrieve.</param>
        /// <returns>The content of the layout.</returns>
        string GetLayout(string layout);

    }

    /// <summary>
    /// Implements the ILayoutRender interface to manage and retrieve layout content.
    /// </summary>
    class LayoutRender : ILayoutRender
    {
        private IDictionary<string, string> layoutDict { get; }

        public LayoutRender()
        {
            this.layoutDict = new Dictionary<string, string>();
        }

        /// <summary>
        /// Retrieves the layout content for the specified layout key.
        /// </summary>
        /// <param name="layout">The key of the layout to retrieve.</param>
        /// <returns>The content of the layout.</returns>
        public string GetLayout(string layout)
        {
            return this.layoutDict[layout];
        }

        /// <summary>
        /// Sets the content for a specified layout key.
        /// </summary>
        /// <param name="layout">The key of the layout to set.</param>
        /// <param name="content">The content to associate with the layout key.</param>
        public void SetLayout(string layout, string content)
        {
            this.layoutDict[layout] = content;
        }

    }

}
