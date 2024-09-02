using System;

namespace XmobiTea.ProtonNet.Server.Handlers.Attributes
{
    /// <summary>
    /// Specifies that the attributed class EventHandler or RequestHandler, sender must is Server can handle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class OnlyServerAttribute : Attribute { }

}
