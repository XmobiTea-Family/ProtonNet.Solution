using System;

namespace XmobiTea.ProtonNet.Server.Handlers.Attributes
{
    /// <summary>
    /// Indicates that the attributed class allows anonymous access.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AllowAnonymousAttribute : Attribute { }

}
