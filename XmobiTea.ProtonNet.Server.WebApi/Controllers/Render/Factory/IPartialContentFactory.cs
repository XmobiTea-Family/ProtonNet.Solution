using System;
using System.Text.RegularExpressions;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory
{
    /// <summary>
    /// Defines the contract for a partial content factory that manages and processes partial content.
    /// </summary>
    interface IPartialContentFactory : IContentFactory<IPartialContent>
    {
        /// <summary>
        /// Replaces the partial content tags within the provided content string with the actual partial content.
        /// </summary>
        /// <param name="content">The string containing partial render tags to be replaced.</param>
        /// <returns>The content string with all partials replaced by their corresponding content.</returns>
        string ReplacePartial(string content);

    }

    /// <summary>
    /// Provides the implementation for handling partial content, including loading, processing, 
    /// and replacing partial tags within content.
    /// </summary>
    class PartialContentFactory : AbstractContentFactory<IPartialContent>, IPartialContentFactory
    {
        /// <summary>
        /// Sets up the partial content by loading all partial files from the specified path, 
        /// processing them, and storing them in the content dictionary.
        /// </summary>
        /// <param name="path">The path to the directory containing the partial files.</param>
        public override void SetupContent(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                this.logger.Warn("could not found web partials path " + path);
                return;
            }

            path = path.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

            var files = System.IO.Directory.GetFiles(path, "*.phtml", System.IO.SearchOption.AllDirectories);

            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                filePath = filePath.Replace(SpecialChars.Backslash, SpecialChars.Slash).RemoveSuffix(SpecialChars.Slash);

                var partial = filePath
                    .Replace(path, string.Empty)
                    .Replace(".phtml", string.Empty)
                    .Replace("\\", "/")
                    .Replace("//", "/").ToLower();

                if (partial.StartsWith("/")) partial = partial.Substring(1);

                var originContent = System.IO.File.ReadAllText(filePath);

                var partialContent = new PartialContent(partial, originContent);
                partialContent.Content = originContent;

                this.contentDict[partial.ToLower()] = partialContent;
            }

            // Recursive partial content replacement to process nested partials
            for (var i = 0; i < 5; i++)
            {
                foreach (var c in this.contentDict)
                {
                    ((PartialContent)c.Value).Content = this.ReplacePartial(c.Value.Content);
                }
            }
        }

        /// <summary>
        /// Retrieves the partial content associated with the specified partial name.
        /// </summary>
        /// <param name="partial">The name of the partial content to retrieve.</param>
        /// <returns>The partial content if found; otherwise, an exception is thrown.</returns>
        /// <exception cref="Exception">Thrown when the partial content is not found.</exception>
        public override IPartialContent GetContent(string partial)
        {
            if (!this.contentDict.TryGetValue(partial.ToLower(), out var content))
                throw new Exception($"Partial '{partial}' not found in partials path.");

            return content;
        }

        /// <summary>
        /// Replaces the partial content tags within the provided content string with the actual partial content.
        /// </summary>
        /// <param name="input">The string containing partial render tags to be replaced.</param>
        /// <returns>The content string with all partials replaced by their corresponding content.</returns>
        public string ReplacePartial(string input)
        {
            var matches = Regex.Matches(input, Constance.RenderPartialPattern, RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var partialContent = this.GetContent(match.Groups[1].Value);
                    input = input.Replace(match.Value, partialContent.Content);
                }
            }

            return input;
        }

    }

}
