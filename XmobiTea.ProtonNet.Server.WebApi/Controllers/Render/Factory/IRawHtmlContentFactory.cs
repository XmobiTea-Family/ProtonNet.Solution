using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XmobiTea.Linq;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory
{
    /// <summary>
    /// Defines the contract for a factory that handles raw HTML templates, 
    /// including generating and retrieving them based on views and layouts.
    /// </summary>
    interface IRawHtmlContentFactory : IContentFactory<IRawHtmlTemplate>
    {
        /// <summary>
        /// Retrieves the raw HTML template by combining the specified view and layout.
        /// </summary>
        /// <param name="view">The name of the view to render.</param>
        /// <param name="layout">The name of the layout to wrap the view.</param>
        /// <returns>The raw HTML template for the specified view and layout.</returns>
        IRawHtmlTemplate GetTemplate(string view, string layout);

    }

    /// <summary>
    /// Provides an implementation for handling raw HTML content templates, including
    /// generating new templates based on views and layouts, and managing session and view data.
    /// </summary>
    class RawHtmlContentFactory : AbstractContentFactory<IRawHtmlTemplate>, IRawHtmlContentFactory
    {
        /// <summary>
        /// Factory for handling partial content within the raw HTML template.
        /// </summary>
        private IPartialContentFactory partialContentFactory { get; }

        /// <summary>
        /// Factory for handling view content within the raw HTML template.
        /// </summary>
        private IViewContentFactory viewContentFactory { get; }

        /// <summary>
        /// Factory for handling layout content within the raw HTML template.
        /// </summary>
        private ILayoutContentFactory layoutContentFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawHtmlContentFactory"/> class with the specified factories.
        /// </summary>
        /// <param name="partialContentFactory">The factory for managing partial content.</param>
        /// <param name="viewContentFactory">The factory for managing view content.</param>
        /// <param name="layoutContentFactory">The factory for managing layout content.</param>
        public RawHtmlContentFactory(IPartialContentFactory partialContentFactory, IViewContentFactory viewContentFactory, ILayoutContentFactory layoutContentFactory)
        {
            this.partialContentFactory = partialContentFactory;
            this.viewContentFactory = viewContentFactory;
            this.layoutContentFactory = layoutContentFactory;
        }

        /// <summary>
        /// Do not use this method on RawHtmlContentFactory.
        /// </summary>
        /// <param name="name">Not applicable.</param>
        /// <returns>Not applicable.</returns>
        /// <exception cref="NotImplementedException">This method is not implemented for RawHtmlContentFactory.</exception>
        public override IRawHtmlTemplate GetContent(string name) => throw new NotImplementedException();

        /// <summary>
        /// Do not use this method on RawHtmlContentFactory.
        /// </summary>
        /// <param name="path">Not applicable.</param>
        /// <exception cref="NotImplementedException">This method is not implemented for RawHtmlContentFactory.</exception>
        public override void SetupContent(string path) => throw new NotImplementedException();

        /// <summary>
        /// Retrieves the raw HTML template by combining the specified view and layout.
        /// </summary>
        /// <param name="view">The name of the view to render.</param>
        /// <param name="layout">The name of the layout to wrap the view.</param>
        /// <returns>The raw HTML template for the specified view and layout.</returns>
        public IRawHtmlTemplate GetTemplate(string view, string layout)
        {
            var key = view + "_" + layout;

            if (!this.contentDict.TryGetValue(key, out var content))
            {
                content = this.GenerateNewRawHtmlContent(view, layout);
                this.contentDict[key] = content;
            }

            return content;
        }

        /// <summary>
        /// Generates a new raw HTML template by processing the specified view and layout, 
        /// handling session, initialization, and view data replacement.
        /// </summary>
        /// <param name="view">The name of the view to render.</param>
        /// <param name="layout">The name of the layout to wrap the view.</param>
        /// <returns>The generated raw HTML template.</returns>
        private IRawHtmlTemplate GenerateNewRawHtmlContent(string view, string layout)
        {
            var viewContent = this.viewContentFactory.GetContent(view);

            if (viewContent.OverrideLayoutName != null) layout = viewContent.OverrideLayoutName;
            var layoutContent = this.layoutContentFactory.GetContent(layout);

            var answer = new RawHtmlTemplate(view + "_" + layout);

            this.ReplaceSession(viewContent, layoutContent, answer);
            this.ReplaceInitElement(answer);

            answer.Content = answer.Content.Trim();
            this.ReplaceViewData(answer);

            return answer;
        }

        /// <summary>
        /// Replaces session placeholders in the layout with the corresponding session content 
        /// from the view and updates the raw HTML template.
        /// </summary>
        /// <param name="viewContent">The content of the view.</param>
        /// <param name="layoutContent">The content of the layout.</param>
        /// <param name="rawHtmlContent">The raw HTML template to update.</param>
        private void ReplaceSession(IViewContent viewContent, ILayoutContent layoutContent, RawHtmlTemplate rawHtmlContent)
        {
            if (layoutContent != null)
            {
                var content = layoutContent.Content;

                var matches = Regex.Matches(content, Constance.RenderSessionPattern, RegexOptions.Singleline);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var sessionContent = viewContent.SessionContents.Find(x => x.Name == match.Groups[1].Value);

                        if (sessionContent != null) content = content.Replace(match.Value, sessionContent.OriginContent);
                    }
                }

                rawHtmlContent.Content = content.Replace("@renderBody();", viewContent.Content);
            }
            else
            {
                rawHtmlContent.Content = viewContent.Content;
            }
        }

        /// <summary>
        /// Processes initialization elements in the raw HTML template by replacing the 
        /// initialization content and updating view data elements.
        /// </summary>
        /// <param name="rawHtmlContent">The raw HTML template to process.</param>
        private void ReplaceInitElement(RawHtmlTemplate rawHtmlContent)
        {
            var matches = Regex.Matches(rawHtmlContent.Content, Constance.InitPattern, RegexOptions.Singleline);

            var setViewInitElementLst = new List<ISetViewDataElement>();

            var pElemenetStr = string.Empty;
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var initElementContent = match.Groups[1].Value.Trim();

                    pElemenetStr += initElementContent + "\n";
                    rawHtmlContent.Content = rawHtmlContent.Content.Replace(match.Value, string.Empty);

                    this.AddSetViewInitElement(initElementContent, setViewInitElementLst);
                }
            }

            rawHtmlContent.PinitElement = new PinitElement(setViewInitElementLst.ToArray());
        }

        /// <summary>
        /// Adds view data initialization elements from the content to the provided list.
        /// </summary>
        /// <param name="content">The content containing initialization elements.</param>
        /// <param name="setViewInitElementLst">The list to store the set view data elements.</param>
        private void AddSetViewInitElement(string content, List<ISetViewDataElement> setViewInitElementLst)
        {
            var matches = Regex.Matches(content, Constance.SetViewDataPattern, RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    object value;

                    var originValue = match.Groups[2].Value;
                    if (originValue.StartsWith("\"") && originValue.EndsWith("\""))
                    {
                        originValue = originValue.Substring(1, originValue.Length - 2);
                        value = originValue;
                    }
                    else
                    {
                        var originValueToLower = originValue.ToLower();
                        if (originValueToLower == "null") value = null;
                        else if (originValueToLower == "false") value = false;
                        else if (originValueToLower == "true") value = true;
                        else if (double.TryParse(originValueToLower, out var doubleValue)) value = doubleValue;
                        else value = originValue;
                    }

                    setViewInitElementLst.Add(new SetViewDataElement(match.Groups[1].Value, value));
                }
            }
        }

        /// <summary>
        /// Processes view data placeholders in the raw HTML template and replaces them 
        /// with the corresponding view data elements.
        /// </summary>
        /// <param name="rawHtmlContent">The raw HTML template to process.</param>
        private void ReplaceViewData(RawHtmlTemplate rawHtmlContent)
        {
            var matches = Regex.Matches(rawHtmlContent.Content, Constance.RenderViewDataPattern, RegexOptions.Singleline);

            rawHtmlContent.RenderViewDataElements = new IRenderViewDataElement[matches.Count];
            var id = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    rawHtmlContent.RenderViewDataElements[id] = new RenderViewDataElement(match.Groups[1].Value, match.Value);
                    id += 1;
                }
            }
        }

    }

}
