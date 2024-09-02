namespace XmobiTea.ProtonNet.Server.WebApi.Types
{
    /// <summary>
    /// Provides constant header values used in the ProtonNet server.
    /// </summary>
    static class HeaderValues
    {
        /// <summary>
        /// Represents the header value for ProtonServerWebApi with the version from Constance.ProtonServerVersion.
        /// </summary>
        public static readonly string ProtonServerWebApi = $"ProtonServerWebApi.v{Constance.ProtonServerVersion}";

        /// <summary>
        /// Represents the header value for the RPC protocol.
        /// </summary>
        public static readonly string RpcProtocol = "rpcpr";

    }

}
