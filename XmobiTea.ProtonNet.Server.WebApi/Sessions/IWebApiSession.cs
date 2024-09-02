using XmobiTea.ProtonNet.Server.Models;
using XmobiTea.ProtonNet.Server.WebApi.Types;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi.Sessions
{
    /// <summary>
    /// Defines the methods and properties for a Web API session.
    /// </summary>
    public interface IWebApiSession : ISession, ISessionIdSetter, IEncryptKeySetter
    {
        /// <summary>
        /// Gets the Hypertext Transfer Protocol used by this session.
        /// </summary>
        /// <returns>The <see cref="HypertextTransferProtocol"/> used by this session.</returns>
        HypertextTransferProtocol GetHypertextTransferProtocol();

        /// <summary>
        /// Sends a byte array to the client.
        /// </summary>
        /// <param name="buffer">The byte array to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Asynchronously sends a byte array to the client.
        /// </summary>
        /// <param name="buffer">The byte array to send.</param>
        /// <returns><c>true</c> if the send operation was initiated successfully; otherwise, <c>false</c>.</returns>
        bool SendAsync(byte[] buffer);

        /// <summary>
        /// Sends an HTTP response to the client.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to send.</param>
        /// <returns>The number of bytes sent.</returns>
        int SendResponse(HttpResponse response);

        /// <summary>
        /// Asynchronously sends an HTTP response to the client.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to send.</param>
        /// <returns><c>true</c> if the send operation was initiated successfully; otherwise, <c>false</c>.</returns>
        bool SendResponseAsync(HttpResponse response);

    }

}
