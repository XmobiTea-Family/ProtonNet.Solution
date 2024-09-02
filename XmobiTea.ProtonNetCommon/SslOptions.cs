using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Represents the SSL/TLS context used for secure communications.
    /// </summary>
    public class SslOptions
    {
        /// <summary>
        /// Gets the SSL/TLS protocols to be used.
        /// </summary>
        public SslProtocols SslProtocols { get; }

        /// <summary>
        /// Gets the certificate used for SSL/TLS.
        /// </summary>
        public X509Certificate X509Certificate { get; }

        /// <summary>
        /// Gets the collection of certificates used for SSL/TLS.
        /// </summary>
        public X509Certificate2Collection Certificates { get; }

        /// <summary>
        /// Initializes a new instance of the SslOptions class with the specified protocols and certificate.
        /// </summary>
        /// <param name="protocols">The SSL/TLS protocols to be used.</param>
        /// <param name="certificate">The certificate used for SSL/TLS.</param>
        public SslOptions(SslProtocols protocols, X509Certificate certificate)
        {
            this.SslProtocols = protocols;
            this.X509Certificate = certificate;
        }

    }

}
