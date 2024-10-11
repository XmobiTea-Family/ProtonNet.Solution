using System;
using System.Collections.Generic;
using XmobiTea.ProtonNetCommon;

namespace XmobiTea.ProtonNet.Server.WebApi.Models
{
    /// <summary>
    /// Represents an HTTP request, inheriting from <see cref="ProtonNetCommon.HttpRequest"/>.
    /// </summary>
    class HttpRequest : ProtonNetCommon.HttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class based on an existing <see cref="ProtonNetCommon.HttpRequest"/>.
        /// </summary>
        /// <param name="originRequest">The original HTTP request to copy from.</param>
        public HttpRequest(ProtonNetCommon.HttpRequest originRequest) : base(originRequest.Method, originRequest.Url, originRequest.Protocol)
        {
            this.IsErrorSet = originRequest.IsErrorSet;
            this.headers = new List<Tuple<string, string>>(originRequest.Headers);
            this.cookies = new List<Tuple<string, string>>(originRequest.Cookies);
            this.bodyIndex = originRequest.BodyIndex;
            this.bodySize = originRequest.BodySize;
            this.bodyLength = originRequest.BodyLength;
            this.bodyLengthProvided = originRequest.BodyLengthProvided;

            var buffer = new byte[originRequest.Cache.Buffer.Length];
            Array.Copy(originRequest.Cache.Buffer, 0, buffer, 0, originRequest.Cache.Buffer.Length);
            this.cache = new MemoryBuffer(buffer);
            this.cacheSize = originRequest.CacheSize;
        }

    }

}
