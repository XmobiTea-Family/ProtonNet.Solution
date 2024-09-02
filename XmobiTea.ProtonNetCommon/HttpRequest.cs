using System;
using System.Collections.Generic;
using System.Text;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Represents an HTTP request with properties for the method, URL, protocol, headers, cookies, and body.
    /// </summary>
    public class HttpRequest
    {
        private string method { get; set; }
        /// <summary>
        /// Gets the HTTP method of the request (e.g., GET, POST).
        /// </summary>
        public string Method => this.method;

        private string url { get; set; }
        /// <summary>
        /// Gets the URL of the request.
        /// </summary>
        public string Url => this.url;

        private string protocol { get; set; }
        /// <summary>
        /// Gets the protocol version (e.g., HTTP/1.1).
        /// </summary>
        public string Protocol => this.protocol;

        protected IList<Tuple<string, string>> headers { get; set; }
        /// <summary>
        /// Gets the list of headers in the request.
        /// </summary>
        public IList<Tuple<string, string>> Headers => this.headers;
        /// <summary>
        /// Gets header count in the request.
        /// </summary>
        public int HeaderCount => this.headers.Count;

        protected IList<Tuple<string, string>> cookies { get; set; }
        /// <summary>
        /// Gets the list of cookies in the request.
        /// </summary>
        public IList<Tuple<string, string>> Cookies => this.cookies;
        /// <summary>
        /// Gets cookie count in the request.
        /// </summary>
        public int CookieCount => this.cookies.Count;

        protected int bodyIndex { get; set; }
        /// <summary>
        /// Gets the index in the buffer where the body starts.
        /// </summary>
        public int BodyIndex => this.bodyIndex;

        protected int bodySize { get; set; }
        /// <summary>
        /// Gets the size of the body in bytes.
        /// </summary>
        public int BodySize => this.bodySize;

        protected int bodyLength { get; set; }
        /// <summary>
        /// Gets the total length of the body.
        /// </summary>
        public int BodyLength => this.bodyLength;

        protected bool bodyLengthProvided { get; set; }
        /// <summary>
        /// Indicates whether the body length was provided in the request.
        /// </summary>
        public bool BodyLengthProvided => this.bodyLengthProvided;

        protected IMemoryBuffer cache { get; set; }
        /// <summary>
        /// Gets the buffer that caches the request data.
        /// </summary>
        public IMemoryBuffer Cache => this.cache;

        protected int cacheSize { get; set; }
        /// <summary>
        /// Gets the size of the cache.
        /// </summary>
        public int CacheSize => this.cacheSize;

        /// <summary>
        /// Indicates whether the request is empty.
        /// </summary>
        public bool IsEmpty => this.cache.Length == 0;

        /// <summary>
        /// Indicates whether an error is set on the request.
        /// </summary>
        public bool IsErrorSet { get; protected set; }

        /// <summary>
        /// Gets the body of the request as a string.
        /// </summary>
        public string BodyAsString => this.cache.ExtractString(this.bodyIndex, this.bodySize);

        /// <summary>
        /// Gets the body of the request as a byte array.
        /// </summary>
        public byte[] BodyAsBytes => this.cache.Buffer.ToClone(this.bodyIndex, (this.bodyIndex + this.bodySize));

        /// <summary>
        /// Initializes a new instance of the HttpRequest class.
        /// </summary>
        public HttpRequest()
        {
            this.headers = new List<Tuple<string, string>>();
            this.cookies = new List<Tuple<string, string>>();
            this.cache = new MemoryBuffer();

            this.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the HttpRequest class with the specified method, URL, and protocol.
        /// </summary>
        /// <param name="method">The HTTP method (e.g., GET, POST).</param>
        /// <param name="url">The URL of the request.</param>
        /// <param name="protocol">The protocol version (default is HTTP/1.1).</param>
        public HttpRequest(string method, string url, string protocol = Constance.DefaultHttpProtocol) : this() => this.SetBegin(method, url, protocol);

        /// <summary>
        /// Gets the header at the specified index.
        /// </summary>
        /// <param name="i">The index of the header.</param>
        /// <returns>A tuple containing the header name and value.</returns>
        public Tuple<string, string> GetHeader(int i)
        {
            if (i >= this.headers.Count)
                return new Tuple<string, string>(string.Empty, null);

            return this.headers[i];
        }

        /// <summary>
        /// Gets the value of the header with the specified key.
        /// </summary>
        /// <param name="key">The name of the header.</param>
        /// <returns>The value of the header, or null if not found.</returns>
        public string GetHeader(string key)
        {
            key = key.ToLower();

            for (var i = 0; i < this.headers.Count; i++)
            {
                var header = this.headers[i];

                if (header.Item1.ToLower() == key) return header.Item2;
            }

            return null;
        }

        /// <summary>
        /// Gets the cookie at the specified index.
        /// </summary>
        /// <param name="i">The index of the cookie.</param>
        /// <returns>A tuple containing the cookie name and value.</returns>
        public Tuple<string, string> GetCookie(int i)
        {
            if (i >= this.cookies.Count)
                return new Tuple<string, string>(string.Empty, string.Empty);

            return this.cookies[i];
        }

        /// <summary>
        /// Gets the value of the cookie with the specified name.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <returns>The value of the cookie, or null if not found.</returns>
        public string GetCookie(string name)
        {
            name = name.ToLower();

            for (var i = 0; i < this.cookies.Count; i++)
            {
                var cookie = this.cookies[i];

                if (cookie.Item1.ToLower() == name) return cookie.Item2;
            }

            return null;
        }

        /// <summary>
        /// Clears the request, resetting all properties to their default values.
        /// </summary>
        public void Clear()
        {
            this.IsErrorSet = false;
            this.method = string.Empty;
            this.url = string.Empty;
            this.protocol = string.Empty;
            this.headers.Clear();
            this.cookies.Clear();
            this.bodyIndex = 0;
            this.bodySize = 0;
            this.bodyLength = 0;
            this.bodyLengthProvided = false;

            this.cache.Clear();
            this.cacheSize = 0;
        }

        /// <summary>
        /// Sets the initial request line (method, URL, protocol).
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="url">The URL of the request.</param>
        /// <param name="protocol">The protocol version (default is HTTP/1.1).</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest SetBegin(string method, string url, string protocol = Constance.DefaultHttpProtocol)
        {
            this.Clear();

            this.cache.Write(method);
            this.method = method;
            this.cache.Write(SpecialBytes.Spacebar);

            this.cache.Write(url);
            this.url = url;
            this.cache.Write(SpecialBytes.Spacebar);

            this.cache.Write(protocol);
            this.protocol = protocol;
            this.cache.Write(SpecialBytes.NewLine);

            return this;
        }

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="key">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest SetHeader(string key, string value)
        {
            this.cache.Write(key);

            this.cache.Write(SpecialBytes.ColonSpacebar);

            this.cache.Write(value);

            this.cache.Write(SpecialBytes.NewLine);

            this.headers.Add(new Tuple<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// Sets a cookie in the request.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest SetCookie(string name, string value)
        {
            var cookie = name + "=" + value;

            this.cache.Write(HeaderNames.Cookie);

            this.cache.Write(SpecialBytes.ColonSpacebar);

            this.cache.Write(cookie);

            this.cache.Write(SpecialBytes.NewLine);

            this.headers.Add(new Tuple<string, string>(HeaderNames.Cookie, cookie));
            this.cookies.Add(new Tuple<string, string>(name, value));

            return this;
        }

        /// <summary>
        /// Adds a cookie to the request.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest AddCookie(string name, string value)
        {
            this.cache.Write(SpecialBytes.SemiColonSpacebar);
            this.cache.Write(name);
            this.cache.Write(SpecialBytes.Equals);
            this.cache.Write(value);

            this.cookies.Add(new Tuple<string, string>(name, value));
            return this;
        }

        /// <summary>
        /// Sets the body of the request as a string.
        /// </summary>
        /// <param name="body">The body content as a string.</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest SetBody(string body = "")
        {
            var length = Encoding.UTF8.GetByteCount(body);

            this.SetHeader(HeaderNames.ContentLength, length.ToString());

            this.cache.Write(SpecialBytes.NewLine);

            var index = this.cache.Length;

            this.cache.Write(body);
            this.bodyIndex = index;
            this.bodySize = length;
            this.bodyLength = length;
            this.bodyLengthProvided = true;
            return this;
        }

        /// <summary>
        /// Sets the body of the request as a byte array.
        /// </summary>
        /// <param name="body">The body content as a byte array.</param>
        /// <returns>The current HttpRequest instance.</returns>
        public HttpRequest SetBody(byte[] body)
        {
            this.SetHeader(HeaderNames.ContentLength, body.Length.ToString());

            this.cache.Write(SpecialBytes.NewLine);

            var index = this.cache.Length;

            this.cache.Write(body);
            this.bodyIndex = index;
            this.bodySize = body.Length;
            this.bodyLength = body.Length;
            this.bodyLengthProvided = true;
            return this;
        }

        /// <summary>
        /// Determines whether the request is pending headers.
        /// </summary>
        /// <returns>True if headers are pending; otherwise, false.</returns>
        public bool IsPendingHeader() => !this.IsErrorSet && (this.bodyIndex == 0);

        /// <summary>
        /// Determines whether the request is pending body data.
        /// </summary>
        /// <returns>True if body data is pending; otherwise, false.</returns>
        public bool IsPendingBody() => !this.IsErrorSet && (this.bodyIndex > 0) && (this.bodySize > 0);

        /// <summary>
        /// Sets the header buffer from the provided byte array.
        /// </summary>
        /// <param name="buffer">The byte array containing the header data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        /// <returns>True if the headers were successfully set; otherwise, false.</returns>
        public bool SetHeaderBuffer(byte[] buffer, int position, int length)
        {
            this.cache.Write(buffer, position, length);

            for (int i = this.cacheSize; i < this.cache.Length; i++)
            {
                if (i + 3 >= this.cache.Length)
                    break;

                if (this.cache[i + 0] == SpecialChars.CarriageReturn && this.cache[i + 1] == SpecialChars.NewLine && this.cache[i + 2] == SpecialChars.CarriageReturn && this.cache[i + 3] == SpecialChars.NewLine)
                {
                    var index = 0;

                    this.IsErrorSet = true;

                    var methodIndex = index;
                    var methodSize = 0;
                    while (this.cache[index] != SpecialChars.Spacebar)
                    {
                        methodSize++;
                        index++;
                        if (index >= this.cache.Length)
                            return false;
                    }
                    index++;
                    if (index >= this.cache.Length)
                        return false;
                    this.method = this.cache.ExtractString(methodIndex, methodSize);

                    var urlIndex = index;
                    var urlSize = 0;
                    while (this.cache[index] != SpecialChars.Spacebar)
                    {
                        urlSize++;
                        index++;
                        if (index >= this.cache.Length)
                            return false;
                    }
                    index++;
                    if (index >= this.cache.Length)
                        return false;
                    this.url = this.cache.ExtractString(urlIndex, urlSize);

                    var protocolIndex = index;
                    var protocolSize = 0;
                    while (this.cache[index] != SpecialChars.CarriageReturn)
                    {
                        protocolSize++;
                        index++;
                        if (index >= this.cache.Length)
                            return false;
                    }
                    index++;
                    if (index >= this.cache.Length || this.cache[index] != SpecialChars.NewLine)
                        return false;
                    index++;
                    if (index >= this.cache.Length)
                        return false;
                    this.protocol = this.cache.ExtractString(protocolIndex, protocolSize);

                    while (index < this.cache.Length && index < i)
                    {
                        var headerNameIndex = index;
                        var headerNameSize = 0;
                        while (this.cache[index] != SpecialChars.Colon)
                        {
                            headerNameSize++;
                            index++;
                            if (index >= i)
                                break;
                            if (index >= this.cache.Length)
                                return false;
                        }
                        index++;
                        if (index >= i)
                            break;
                        if (index >= this.cache.Length)
                            return false;

                        while (char.IsWhiteSpace((char)this.cache[index]))
                        {
                            index++;
                            if (index >= i)
                                break;
                            if (index >= this.cache.Length)
                                return false;
                        }

                        var headerValueIndex = index;
                        var headerValueSize = 0;
                        while (this.cache[index] != SpecialChars.CarriageReturn)
                        {
                            headerValueSize++;
                            index++;
                            if (index >= i)
                                break;
                            if (index >= this.cache.Length)
                                return false;
                        }
                        index++;
                        if (index >= this.cache.Length || this.cache[index] != SpecialChars.NewLine)
                            return false;
                        index++;
                        if (index >= this.cache.Length)
                            return false;

                        if (headerNameSize == 0)
                            return false;

                        var headerName = this.cache.ExtractString(headerNameIndex, headerNameSize);
                        var headerValue = this.cache.ExtractString(headerValueIndex, headerValueSize);
                        this.headers.Add(new Tuple<string, string>(headerName, headerValue));

                        if (headerName.ToLower() == HeaderNames.ContentLength)
                        {
                            this.bodyLength = 0;
                            for (var j = headerValueIndex; j < (headerValueIndex + headerValueSize); j++)
                            {
                                if (this.cache[j] < SpecialChars.Number0 || this.cache[j] > SpecialChars.Number9)
                                    return false;
                                this.bodyLength *= 10;
                                this.bodyLength += this.cache[j] - SpecialChars.Number0;
                                this.bodyLengthProvided = true;
                            }
                        }

                        if (headerName.ToLower() == HeaderNames.Cookie)
                        {
                            var name = true;
                            var token = false;
                            var current = headerValueIndex;
                            var nameIndex = index;
                            var nameSize = 0;
                            var cookieIndex = index;
                            var cookieSize = 0;

                            for (var j = headerValueIndex; j < (headerValueIndex + headerValueSize); j++)
                            {
                                if (this.cache[j] == SpecialChars.Spacebar)
                                {
                                    if (token)
                                    {
                                        if (name)
                                        {
                                            nameIndex = current;
                                            nameSize = j - current;
                                        }
                                        else
                                        {
                                            cookieIndex = current;
                                            cookieSize = j - current;
                                        }
                                    }
                                    token = false;
                                    continue;
                                }
                                if (this.cache[j] == SpecialChars.Equals)
                                {
                                    if (token)
                                    {
                                        if (name)
                                        {
                                            nameIndex = current;
                                            nameSize = j - current;
                                        }
                                        else
                                        {
                                            cookieIndex = current;
                                            cookieSize = j - current;
                                        }
                                    }
                                    token = false;
                                    name = false;
                                    continue;
                                }
                                if (this.cache[j] == SpecialChars.Semicolon)
                                {
                                    if (token)
                                    {
                                        if (name)
                                        {
                                            nameIndex = current;
                                            nameSize = j - current;
                                        }
                                        else
                                        {
                                            cookieIndex = current;
                                            cookieSize = j - current;
                                        }

                                        if (nameSize > 0 && cookieSize > 0)
                                        {
                                            this.cookies.Add(new Tuple<string, string>(this.cache.ExtractString(nameIndex, nameSize), this.cache.ExtractString(cookieIndex, cookieSize)));

                                            nameIndex = j;
                                            nameSize = 0;
                                            cookieIndex = j;
                                            cookieSize = 0;
                                        }
                                    }
                                    token = false;
                                    name = true;
                                    continue;
                                }
                                if (!token)
                                {
                                    current = j;
                                    token = true;
                                }
                            }

                            if (token)
                            {
                                if (name)
                                {
                                    nameIndex = current;
                                    nameSize = headerValueIndex + headerValueSize - current;
                                }
                                else
                                {
                                    cookieIndex = current;
                                    cookieSize = headerValueIndex + headerValueSize - current;
                                }

                                if (nameSize > 0 && cookieSize > 0)
                                {
                                    this.cookies.Add(new Tuple<string, string>(this.cache.ExtractString(nameIndex, nameSize), this.cache.ExtractString(cookieIndex, cookieSize)));
                                }
                            }
                        }
                    }

                    this.IsErrorSet = false;

                    this.bodyIndex = i + 4;
                    this.bodySize = this.cache.Length - i - 4;

                    this.cacheSize = this.cache.Length;

                    return true;
                }
            }

            this.cacheSize = (this.cache.Length >= 3) ? (this.cache.Length - 3) : 0;

            return false;
        }

        /// <summary>
        /// Sets the body buffer from the provided byte array.
        /// </summary>
        /// <param name="buffer">The byte array containing the body data.</param>
        /// <param name="position">The position in the buffer where the data starts.</param>
        /// <param name="length">The length of the data in the buffer.</param>
        /// <returns>True if the body was successfully set; otherwise, false.</returns>
        public bool SetBodyBuffer(byte[] buffer, int position, int length)
        {
            this.cache.Write(buffer, position, length);

            this.cacheSize = this.cache.Length;

            this.bodySize += length;

            if (this.bodyLengthProvided)
            {
                if (this.bodySize >= this.bodyLength)
                {
                    this.bodySize = this.bodyLength;
                    return true;
                }
            }
            else
            {
                if (this.Method == MethodNames.Head || this.Method == MethodNames.Get || this.Method == MethodNames.Delete || this.Method == MethodNames.Options || this.Method == MethodNames.Trace)
                {
                    this.bodyLength = 0;
                    this.bodySize = 0;
                    return true;
                }

                if (this.bodySize >= 4)
                {
                    var index = this.bodyIndex + this.bodySize - 4;

                    if (this.cache[index + 0] == SpecialChars.CarriageReturn && this.cache[index + 1] == SpecialChars.NewLine && this.cache[index + 2] == SpecialChars.CarriageReturn && this.cache[index + 3] == SpecialChars.NewLine)
                    {
                        this.bodyLength = this.bodySize;
                        return true;
                    }
                }
            }

            return false;
        }

    }

}
