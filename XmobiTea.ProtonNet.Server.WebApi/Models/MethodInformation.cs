using System.Collections.Generic;
using System.Reflection;
using XmobiTea.ProtonNet.Server.WebApi.Controllers.Attribute;
using XmobiTea.ProtonNet.Server.WebApi.Sessions;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents the data used for generating parameters for method invocation.
    /// </summary>
    class GenerateParameterData
    {
        /// <summary>
        /// The HTTP request associated with the parameter data.
        /// </summary>
        public ProtonNetCommon.HttpRequest HttpRequest { get; set; }

        /// <summary>
        /// The HTTP response associated with the parameter data.
        /// </summary>
        public ProtonNetCommon.HttpResponse HttpResponse { get; set; }

        /// <summary>
        /// The session associated with the parameter data.
        /// </summary>
        public IWebApiSession Session { get; set; }

        /// <summary>
        /// The middleware context associated with the parameter data.
        /// </summary>
        public MiddlewareContext MiddlewareContext { get; set; }

        /// <summary>
        /// The attribute indicating how the parameter is bound from HTTP.
        /// </summary>
        public FromHttpParamAttribute FromHttpParam { get; set; }

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public System.Type ParameterType { get; set; }

        /// <summary>
        /// The method controller associated with the parameter data.
        /// </summary>
        public MethodController MethodController { get; set; }

        /// <summary>
        /// The list of query items for the parameter data.
        /// </summary>
        public List<QueryItem> QueryLst { get; set; }
    }

    /// <summary>
    /// Delegate for a function that generates a parameter object based on the given parameter data.
    /// </summary>
    /// <param name="parameterData">The data used to generate the parameter object.</param>
    /// <param name="obj">The generated parameter object.</param>
    /// <returns><c>true</c> if the parameter object was successfully generated; otherwise, <c>false</c>.</returns>
    delegate bool TryGenerateParameterDataDelegate(GenerateParameterData parameterData, out object obj);

    /// <summary>
    /// Represents information about a method, including its parameters and how to generate those parameters.
    /// </summary>
    class MethodInformation
    {
        /// <summary>
        /// The object that contains the method.
        /// </summary>
        public object originObject { get; internal set; }

        /// <summary>
        /// The method info for the method.
        /// </summary>
        public MethodInfo methodInfo { get; internal set; }

        /// <summary>
        /// The parameter info for the method.
        /// </summary>
        public ParameterInfo[] parameterInfos { get; internal set; }

        /// <summary>
        /// Delegates used to generate parameter data for the method.
        /// </summary>
        public TryGenerateParameterDataDelegate[] tryGenerateParameterDatas { get; internal set; }
    }

    /// <summary>
    /// Represents information about a method invocation, including its prefix.
    /// </summary>
    class MethodInvokeInformation : MethodInformation
    {
        /// <summary>
        /// The full prefix associated with the method invocation.
        /// </summary>
        public string fullPrefix { get; internal set; }
    }

    /// <summary>
    /// Represents a controller for handling methods, including method information and controllers for different prefixes.
    /// </summary>
    class MethodController
    {
        /// <summary>
        /// The key prefix for the method controller.
        /// </summary>
        public string keyPrefix { get; }

        /// <summary>
        /// The full prefix for the method controller.
        /// </summary>
        public string fullPrefix { get; }

        /// <summary>
        /// The list of method information associated with the controller.
        /// </summary>
        public List<MethodInformation> methodInformationLst { get; internal set; }

        /// <summary>
        /// The object that contains the method controller.
        /// </summary>
        public object originObject { get; internal set; }

        /// <summary>
        /// Indicates whether the prefix is parameterized.
        /// </summary>
        public bool isPrefixWithParam { get; internal set; }

        /// <summary>
        /// The dictionary of method controllers for different prefixes.
        /// </summary>
        public IDictionary<string, MethodController> methodControllerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodController"/> class.
        /// </summary>
        /// <param name="keyPrefix">The key prefix for the controller.</param>
        /// <param name="fullPrefix">The full prefix for the controller.</param>
        public MethodController(string keyPrefix, string fullPrefix)
        {
            this.methodInformationLst = new List<MethodInformation>();
            this.methodControllerDict = new Dictionary<string, MethodController>();

            this.keyPrefix = keyPrefix;
            this.fullPrefix = fullPrefix;

        }

    }

}
