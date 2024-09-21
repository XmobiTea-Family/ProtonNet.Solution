using System;
using System.Text.RegularExpressions;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory
{
    /// <summary>
    /// Defines the contract for a factory that manages and retrieves view content.
    /// </summary>
    interface IViewContentFactory : IContentFactory<IViewContent>
    {
    }

    /// <summary>
    /// Provides an implementation for handling view content, including loading and processing views, 
    /// managing layout overrides, and handling session content.
    /// </summary>
    class ViewContentFactory : AbstractContentFactory<IViewContent>, IViewContentFactory
    {
        /// <summary>
        /// Gets the factory responsible for handling partial content within views.
        /// </summary>
        private IPartialContentFactory partialContentFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewContentFactory"/> class with the specified partial content factory.
        /// </summary>
        /// <param name="partialContentFactory">The factory responsible for managing partial content.</param>
        public ViewContentFactory(IPartialContentFactory partialContentFactory) => this.partialContentFactory = partialContentFactory;

        /// <summary>
        /// Sets up the content by loading view files from the specified path, processes them, 
        /// and stores them in the content dictionary. Handles layout overrides and session content.
        /// </summary>
        /// <param name="path">The path to the directory containing the view files.</param>
        public override void SetupContent(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                this.logger.Warn("could not found web views path " + path);
                return;
            }

            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            var files = System.IO.Directory.GetFiles(path, "*.phtml", System.IO.SearchOption.AllDirectories);

            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

                var view = filePath
                    .Replace(path, string.Empty)
                    .Replace(".phtml", string.Empty)
                    .Replace("\\", "/")
                    .Replace("//", "/").ToLower();

                if (view.StartsWith("/")) view = view.Substring(1);

                var originContent = System.IO.File.ReadAllText(filePath);

                var viewContent = new ViewContent(view, originContent);

                // Handle layout override in the view
                {
                    var match = Regex.Match(originContent, Constance.RenderLayoutPattern, RegexOptions.Singleline);

                    if (match.Success)
                    {
                        viewContent.OverrideLayoutName = match.Groups[1].Value;
                        originContent = originContent.Replace(match.Value, string.Empty);
                    }
                }

                // Handle session content in the view
                {
                    var matches = Regex.Matches(originContent, Constance.SessionPattern, RegexOptions.Singleline);

                    viewContent.SessionContents = new ISessionContent[matches.Count];
                    var id = 0;

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            var sessionContent = new SessionContent(match.Groups[1].Value, match.Groups[2].Value.Trim());
                            sessionContent.Content = this.partialContentFactory.ReplacePartial(sessionContent.OriginContent);
                            viewContent.SessionContents[id] = sessionContent;

                            originContent = originContent.Replace(match.Value, string.Empty);
                            id += 1;
                        }
                    }
                }

                // Store the processed view content
                viewContent.Content = originContent;
                this.contentDict[view.ToLower()] = viewContent;
            }
        }

        /// <summary>
        /// Retrieves the view content associated with the specified view name.
        /// </summary>
        /// <param name="view">The name of the view to retrieve.</param>
        /// <returns>The view content if found; otherwise, an exception is thrown.</returns>
        /// <exception cref="ArgumentException">Thrown when the view content is not found.</exception>
        public override IViewContent GetContent(string view)
        {
            if (!this.contentDict.TryGetValue(view.ToLower(), out var content))
                throw new ArgumentException($"View '{view}' not found in views path.");

            return content;
        }

    }

}
