using System;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory
{
    /// <summary>
    /// Defines the contract for a layout content factory that manages layout content.
    /// </summary>
    interface ILayoutContentFactory : IContentFactory<ILayoutContent>
    {
    }

    /// <summary>
    /// Provides the implementation for handling layout content, including loading and retrieving layouts.
    /// </summary>
    class LayoutContentFactory : AbstractContentFactory<ILayoutContent>, ILayoutContentFactory
    {
        /// <summary>
        /// Gets the factory responsible for handling partial content within layouts.
        /// </summary>
        private IPartialContentFactory partialContentFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutContentFactory"/> class
        /// with the specified partial content factory.
        /// </summary>
        /// <param name="partialContentFactory">The factory responsible for managing partial content.</param>
        public LayoutContentFactory(IPartialContentFactory partialContentFactory) => this.partialContentFactory = partialContentFactory;

        /// <summary>
        /// Sets up the content by loading layout files from the specified path, 
        /// processes them, and stores them in the content dictionary.
        /// </summary>
        /// <param name="path">The path to the directory containing the layout files.</param>
        public override void SetupContent(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                this.logger.Warn("could not found web layouts path " + path);
                return;
            }

            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            var files = System.IO.Directory.GetFiles(path, "*.phtml", System.IO.SearchOption.AllDirectories);

            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

                var layout = filePath
                    .Replace(path, string.Empty)
                    .Replace(".phtml", string.Empty)
                    .Replace("\\", "/")
                    .Replace("//", "/").ToLower();

                if (layout.StartsWith("/")) layout = layout.Substring(1);

                var originContent = System.IO.File.ReadAllText(filePath);

                var layoutContent = new LayoutContent(layout, originContent);
                layoutContent.Content = this.partialContentFactory.ReplacePartial(originContent);

                this.contentDict[layout.ToLower()] = layoutContent;
            }
        }

        /// <summary>
        /// Retrieves the layout content associated with the specified layout name.
        /// </summary>
        /// <param name="layout">The name of the layout to retrieve.</param>
        /// <returns>The layout content if found; otherwise, an exception is thrown.</returns>
        /// <exception cref="ArgumentException">Thrown when the layout content is not found.</exception>
        public override ILayoutContent GetContent(string layout)
        {
            if (!this.contentDict.TryGetValue(layout.ToLower(), out var content))
                throw new ArgumentException($"Layout '{layout}' not found in layouts path.");

            return content;
        }

    }

}
