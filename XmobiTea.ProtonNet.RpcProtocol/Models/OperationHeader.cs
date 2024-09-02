using XmobiTea.ProtonNet.Networking;
using XmobiTea.ProtonNet.RpcProtocol.Types;

namespace XmobiTea.ProtonNet.RpcProtocol.Models
{
    /// <summary>
    /// Represents the header for an operation in the RPC protocol.
    /// </summary>
    public class OperationHeader
    {
        /// <summary>
        /// Gets or sets the length of the payload in bytes.
        /// </summary>
        public int PayloadLength { get; set; }

        /// <summary>
        /// Gets or sets the parameters related to sending the operation.
        /// </summary>
        public SendParameters SendParameters { get; set; }

        /// <summary>
        /// Gets or sets the type of operation.
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// Gets or sets the type of protocol provider.
        /// </summary>
        public ProtocolProviderType ProtocolProviderType { get; set; }

        /// <summary>
        /// Gets or sets the type of crypto provider used for encryption, if applicable.
        /// </summary>
        public CryptoProviderType? CryptoProviderType { get; set; }

    }

}
