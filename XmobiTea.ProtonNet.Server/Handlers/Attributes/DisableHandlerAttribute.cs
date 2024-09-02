using System;

namespace XmobiTea.ProtonNet.Server.Handlers.Attributes
{
    /// <summary>
    /// Indicates that the attributed class should have its handler functionality disabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DisableHandlerAttribute : Attribute { }

}
