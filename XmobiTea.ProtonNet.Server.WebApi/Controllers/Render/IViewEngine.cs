using System.Collections.Generic;
using System.Text.RegularExpressions;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Factory;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Render.Models;
using XmobiTea.ProtonNet.Server.WebApi.Models;

namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render
{
    /// <summary>
    /// Defines the contract for a view engine that processes templates 
    /// and renders views with data.
    /// </summary>
    interface IViewEngine
    {
        /// <summary>
        /// Configures the path where the view engine will look for web templates.
        /// </summary>
        /// <param name="websPath">The path to the directory containing web templates.</param>
        void SetupWebsPath(string websPath);

        /// <summary>
        /// Retrieves the raw HTML template for the specified view and layout.
        /// </summary>
        /// <param name="view">The name of the view to be rendered.</param>
        /// <param name="layout">The name of the layout that wraps the view.</param>
        /// <returns>The raw HTML template for the view and layout combination.</returns>
        IRawHtmlTemplate GetTemplate(string view, string layout);

        /// <summary>
        /// Renders the specified template with the provided view data.
        /// </summary>
        /// <param name="template">The raw HTML template to be rendered.</param>
        /// <param name="viewData">The data that will be passed to the view for rendering.</param>
        /// <returns>The rendered view.</returns>
        IView Render(IRawHtmlTemplate template, IViewData viewData);

    }

    /// <summary>
    /// Implements the IViewEngine interface to process templates and render views.
    /// </summary>
    class ViewEngine : IViewEngine
    {
        /// <summary>
        /// Gets the factory responsible for creating partial content.
        /// </summary>
        private IPartialContentFactory partialContentFactory { get; }

        /// <summary>
        /// Gets the factory responsible for creating view content.
        /// </summary>
        private IViewContentFactory viewContentFactory { get; }

        /// <summary>
        /// Gets the factory responsible for creating layout content.
        /// </summary>
        private ILayoutContentFactory layoutContentFactory { get; }

        /// <summary>
        /// Gets the factory responsible for creating raw HTML content.
        /// </summary>
        private IRawHtmlContentFactory rawHtmlContentFactory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewEngine"/> class.
        /// Sets up the content factories required for rendering views.
        /// </summary>
        public ViewEngine()
        {
            this.partialContentFactory = new PartialContentFactory();
            this.viewContentFactory = new ViewContentFactory(this.partialContentFactory);
            this.layoutContentFactory = new LayoutContentFactory(this.partialContentFactory);
            this.rawHtmlContentFactory = new RawHtmlContentFactory(this.partialContentFactory, this.viewContentFactory, this.layoutContentFactory);
        }

        /// <summary>
        /// Configures the paths for partials, views, and layouts based on the specified web path.
        /// </summary>
        /// <param name="websPath">The base path to the web content directories.</param>
        public void SetupWebsPath(string websPath)
        {
            this.partialContentFactory.SetupContent(websPath + "/partials");
            this.viewContentFactory.SetupContent(websPath + "/views");
            this.layoutContentFactory.SetupContent(websPath + "/layouts");
        }

        /// <summary>
        /// Retrieves the raw HTML template for the specified view and layout.
        /// </summary>
        /// <param name="view">The name of the view to render.</param>
        /// <param name="layout">The name of the layout to use. If null, an empty string is used.</param>
        /// <returns>The raw HTML template combining the view and layout.</returns>
        public IRawHtmlTemplate GetTemplate(string view, string layout)
        {
            if (layout == null) layout = string.Empty;

            return this.rawHtmlContentFactory.GetTemplate(view, layout);
        }

        /// <summary>
        /// Renders the specified template using the provided view data.
        /// </summary>
        /// <param name="template">The raw HTML template to render.</param>
        /// <param name="viewData">The data to be used during rendering.</param>
        /// <returns>An <see cref="IView"/> object containing the rendered HTML.</returns>
        public IView Render(IRawHtmlTemplate template, IViewData viewData)
        {
            var view = new View() { Html = template.Content, };

            if (viewData == null) viewData = new ViewData();

            // Set initial view data elements from the template
            if (template.PinitElement.SetViewDataElements.Length != 0)
                foreach (var setViewDataElement in template.PinitElement.SetViewDataElements)
                    viewData.GetOriginDict()[setViewDataElement.Name] = setViewDataElement.Value;

            // Process conditional statements in the template
            this.ProcessIfElseStatements(view, viewData);

            // Process conditional statements in the template
            this.ProcessIfStatements(view, viewData);

            // Process foreach statements in the template
            this.ProcessForEachStatements(view, viewData);

            // Process render directives in the template
            this.ProcessRenderViewData(view, template, viewData);

            return view;
        }

        /// <summary>
        /// Replaces placeholders in the view with corresponding data from <see cref="IViewData"/>.
        /// </summary>
        /// <param name="view">The view object containing HTML to be rendered.</param>
        /// <param name="template">The raw HTML template.</param>
        /// <param name="viewData">The data used for rendering.</param>
        private void ProcessRenderViewData(IView view, IRawHtmlTemplate template, IViewData viewData)
        {
            if (template.RenderViewDataElements.Length != 0)
                foreach (var renderViewDataElement in template.RenderViewDataElements)
                    if (viewData.Contains(renderViewDataElement.Name))
                        view.Html = view.Html.Replace(renderViewDataElement.OriginContent, viewData.GetData(renderViewDataElement.Name).ToString());
        }

        /// <summary>
        /// Processes and renders conditional (if else) statements in the template.
        /// </summary>
        /// <param name="view">The view object being rendered.</param>
        /// <param name="viewData">The data used for evaluating conditions.</param>
        private void ProcessIfElseStatements(IView view, IViewData viewData)
        {
            view.Html = Regex.Replace(view.Html, Constance.IfElsePattern, match =>
            {
                var condition = match.Groups[1].Value.Trim();
                var content = match.Groups[2].Value;
                var elseContent = match.Groups[3].Value;

                if (this.EvaluateCondition(condition, viewData))
                    return this.EvaluateContent(content, viewData);

                return this.EvaluateContent(elseContent, viewData); // If condition is false, exclude the content
            });
        }

        /// <summary>
        /// Processes and renders conditional (if) statements in the template.
        /// </summary>
        /// <param name="view">The view object being rendered.</param>
        /// <param name="viewData">The data used for evaluating conditions.</param>
        private void ProcessIfStatements(IView view, IViewData viewData)
        {
            view.Html = Regex.Replace(view.Html, Constance.IfPattern, match =>
            {
                var condition = match.Groups[1].Value.Trim();
                var content = match.Groups[2].Value;

                if (this.EvaluateCondition(condition, viewData))
                    return this.EvaluateContent(content, viewData);

                return string.Empty; // If condition is false, exclude the content
            });
        }

        /// <summary>
        /// Processes and renders foreach loops in the template.
        /// </summary>
        /// <param name="view">The view object being rendered.</param>
        /// <param name="viewData">The data used for iterating collections.</param>
        private void ProcessForEachStatements(IView view, IViewData viewData)
        {
            view.Html = Regex.Replace(view.Html, Constance.ForeachPattern, match =>
            {
                var variable = match.Groups[1].Value.Trim();
                var collectionName = match.Groups[2].Value.Trim();
                var content = match.Groups[3].Value;

                if (viewData.TryGetValue(collectionName, out var collection) && collection is IEnumerable<object> items)
                {
                    var result = new System.Text.StringBuilder();

                    foreach (var item in items)
                    {
                        //var itemModel = new Dictionary<string, object>(viewData.GetOriginDict()) { [variable] = item };
                        //result.Append(this.EvaluateContent(this.ReplaceRender(content, itemModel, variable), viewData));

                        var cloneContent = this.ReplaceRender(content, item, variable);
                        result.Append(this.EvaluateContent(cloneContent, viewData));
                    }

                    return result.ToString();
                }

                return string.Empty;
            });
        }

        /// <summary>
        /// Replaces placeholders within a template fragment using the provided model.
        /// </summary>
        /// <param name="template">The template fragment containing placeholders.</param>
        /// <param name="item">The item used for replacement.</param>
        /// <param name="variable">The variable name to replace in the template.</param>
        /// <returns>The template fragment with placeholders replaced by actual data.</returns>
        private string ReplaceRender(string template, object item, string variable)
        {
            var renderPattern = $@"@render\s*\(\s*{variable}\s*\)\s*;";

            return Regex.Replace(template, renderPattern, match => item?.ToString() ?? string.Empty);
        }

        private bool EvaluateCondition(string condition, IViewData viewData)
        {
            var pattern = @"@getViewData\(\s*""(\w+)""\s*\)\s*(==|!=|<=|>=|<|>)\s*(.+)";
            var match = Regex.Match(condition, pattern);

            if (match.Success)
            {
                var key = match.Groups[1].Value;
                var operatorSign = match.Groups[2].Value;
                var comparisonValue = match.Groups[3].Value.Trim();

                // Kiểm tra nếu so sánh với null
                if (comparisonValue == "null")
                {
                    if (viewData.TryGetValue(key, out var modelValue1))
                    {
                        return this.CompareWithNull(modelValue1, operatorSign);
                    }
                    else
                    {
                        // Nếu không tìm thấy giá trị trong model, xử lý như null
                        return this.CompareWithNull(null, operatorSign);
                    }
                }

                // Lấy giá trị từ model
                if (viewData.TryGetValue(key, out var modelValue))
                {
                    // Chuyển đổi comparisonValue và modelValue thành kiểu phù hợp để so sánh
                    if (double.TryParse(modelValue.ToString(), out var modelNumber) && double.TryParse(comparisonValue, out var comparisonNumber))
                    {
                        // So sánh số học
                        return this.CompareNumbers(modelNumber, comparisonNumber, operatorSign);
                    }
                    else if (bool.TryParse(modelValue.ToString(), out var modelBool) && bool.TryParse(comparisonValue, out var comparisonBool))
                    {
                        // So sánh boolean
                        return this.CompareBooleans(modelBool, comparisonBool, operatorSign);
                    }
                    else
                    {
                        // So sánh chuỗi
                        return this.CompareStrings(modelValue.ToString(), comparisonValue, operatorSign);
                    }
                }
            }

            // Nếu không phải so sánh phức tạp, kiểm tra điều kiện đơn giản (boolean từ model)
            if (viewData.TryGetValue(condition, out var value) && value is bool boolValue)
            {
                return boolValue;
            }

            return false;
        }

        private bool CompareWithNull(object modelValue, string operatorSign)
        {
            switch (operatorSign)
            {
                case "==":
                    return modelValue == null;
                case "!=":
                    return modelValue != null;
                default:
                    return false;
            }
        }

        private bool CompareNumbers(double modelValue, double comparisonValue, string operatorSign)
        {
            switch (operatorSign)
            {
                case "==":
                    return modelValue == comparisonValue;
                case "!=":
                    return modelValue != comparisonValue;
                case "<=":
                    return modelValue <= comparisonValue;
                case ">=":
                    return modelValue >= comparisonValue;
                case "<":
                    return modelValue < comparisonValue;
                case ">":
                    return modelValue > comparisonValue;
                default:
                    return false;
            }
        }

        private bool CompareBooleans(bool modelValue, bool comparisonValue, string operatorSign)
        {
            switch (operatorSign)
            {
                case "==":
                    return modelValue == comparisonValue;
                case "!=":
                    return modelValue != comparisonValue;
                default:
                    return false;
            }
        }

        private bool CompareStrings(string modelValue, string comparisonValue, string operatorSign)
        {
            switch (operatorSign)
            {
                case "==":
                    return modelValue == comparisonValue;
                case "!=":
                    return modelValue != comparisonValue;
                default:
                    return false;
            }
        }

        private string EvaluateContent(string content, IViewData viewData)
        {
            if (string.IsNullOrEmpty(content)) return content;

            var view = new View() { Html = content, };

            // Process set view data
            this.ProcessSetViewData(view, viewData);

            // Process conditional statements in the template
            this.ProcessIfElseStatements(view, viewData);

            // Process conditional statements in the template
            this.ProcessIfStatements(view, viewData);

            // Process foreach statements in the template
            this.ProcessForEachStatements(view, viewData);

            return view.Html;
        }

        private void ProcessSetViewData(IView view, IViewData viewData)
        {
            view.Html = Regex.Replace(view.Html, Constance.SetViewDataPattern, match =>
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

                viewData.GetOriginDict()[match.Groups[1].Value] = value;

                return string.Empty;
            });
        }

    }

}
