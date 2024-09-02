using System;
using System.Collections.Generic;
using System.Text;
using XmobiTea.ProtonNetCommon.Extensions;
using XmobiTea.ProtonNetCommon.Helper;
using XmobiTea.ProtonNetCommon.Types;

namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Represents an HTTP response, encapsulating the protocol, status, headers, cookies, and body of the response.
    /// </summary>
    public class HttpResponse
    {
        private string protocol { get; set; }
        /// <summary>
        /// Gets the HTTP protocol used in the response.
        /// </summary>
        public string Protocol => this.protocol;

        private IList<Tuple<string, string>> headers { get; }
        /// <summary>
        /// Gets the count of headers in the response.
        /// </summary>
        public long HeaderCount => this.headers.Count;

        private IList<Tuple<string, string>> cookies { get; set; }
        /// <summary>
        /// Gets the count of cookies in the response.
        /// </summary>
        public int CookieCount => this.cookies.Count;

        private int bodyIndex { get; set; }
        private int bodySize { get; set; }
        private int bodyLength { get; set; }
        private bool bodyLengthProvided { get; set; }

        private IMemoryBuffer cache { get; }
        /// <summary>
        /// Gets the memory buffer that stores the response data.
        /// </summary>
        public IMemoryBuffer Cache => this.cache;

        private int cacheSize { get; set; }

        /// <summary>
        /// Indicates whether the response body is empty.
        /// </summary>
        public bool IsEmpty => this.cache.Length > 0;

        /// <summary>
        /// Indicates whether an error has been set in the response.
        /// </summary>
        public bool IsErrorSet { get; private set; }

        private string statusPhrase { get; set; }
        /// <summary>
        /// Gets the HTTP status code of the response.
        /// </summary>
        public StatusCode Status { get; private set; }

        /// <summary>
        /// Gets the body of the response as a string.
        /// </summary>
        public string BodyAsString => this.cache.ExtractString(this.bodyIndex, this.bodySize);

        /// <summary>
        /// Gets the body of the response as a byte array.
        /// </summary>
        public byte[] BodyAsBytes => this.cache.Buffer.ToClone(this.bodyIndex, (this.bodyIndex + this.bodySize));

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        public HttpResponse()
        {
            this.headers = new List<Tuple<string, string>>();
            this.cookies = new List<Tuple<string, string>>();
            this.cache = new MemoryBuffer();

            this.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class with the specified status and protocol.
        /// </summary>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="protocol">The HTTP protocol (default is <see cref="Constance.DefaultHttpProtocol"/>).</param>
        public HttpResponse(StatusCode status, string protocol = Constance.DefaultHttpProtocol) : this() => this.SetBegin(status, protocol);

        /// <summary>
        /// Gets the header at the specified index.
        /// </summary>
        /// <param name="i">The index of the header.</param>
        /// <returns>A tuple containing the header name and value.</returns>
        public Tuple<string, string> GetHeader(int i)
        {
            if (i >= this.headers.Count)
                return new Tuple<string, string>(string.Empty, string.Empty);

            return this.headers[i];
        }

        /// <summary>
        /// Gets the value of the specified header.
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
        /// Gets the value of the specified cookie.
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
        /// Clears the current response, resetting all properties.
        /// </summary>
        public void Clear()
        {
            this.IsErrorSet = false;
            this.Status = 0;
            this.statusPhrase = string.Empty;
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
        /// Sets the beginning of the response with the specified status and protocol.
        /// </summary>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="protocol">The HTTP protocol (default is <see cref="Constance.DefaultHttpProtocol"/>).</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetBegin(StatusCode status, string protocol = Constance.DefaultHttpProtocol)
        {
            this.Clear();

            this.cache.Write(protocol);
            this.protocol = protocol;
            this.cache.Write(SpecialBytes.Spacebar);

            this.cache.Write(((int)status).ToString());
            this.Status = status;
            this.cache.Write(SpecialBytes.Spacebar);

            this.statusPhrase = status.ToString();
            this.cache.Write(this.statusPhrase);
            this.cache.Write(SpecialBytes.NewLine);

            return this;
        }

        /// <summary>
        /// Sets the Content-Type header for the response based on the file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetContentType(string extension)
        {
            var mime = MimeUtils.GetMimeName(extension);
            if (!string.IsNullOrEmpty(mime))
                return this.SetHeader(HeaderNames.ContentType, mime);

            return this;
        }

        /// <summary>
        /// Sets a header with the specified key and value.
        /// </summary>
        /// <param name="key">The name of the header.</param>
        /// <param name="value">The value of the header.</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetHeader(string key, string value)
        {
            this.cache.Write(key);

            this.cache.Write(SpecialBytes.ColonSpacebar);

            this.cache.Write(value);

            this.cache.Write(SpecialBytes.NewLine);

            this.headers.Add(new Tuple<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// Sets a cookie with the specified name, value, and optional parameters.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="maxAge">The maximum age of the cookie in seconds (default is 86400 seconds).</param>
        /// <param name="path">The path where the cookie is valid.</param>
        /// <param name="domain">The domain where the cookie is valid.</param>
        /// <param name="secure">Indicates whether the cookie is secure (default is true).</param>
        /// <param name="strict">Indicates whether the cookie should be SameSite=Strict (default is true).</param>
        /// <param name="httpOnly">Indicates whether the cookie is HttpOnly (default is true).</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetCookie(string name, string value, int maxAge = 86400, string path = "", string domain = "", bool secure = true, bool strict = true, bool httpOnly = true)
        {
            this.cache.Write(HeaderNames.SetCookie);

            this.cache.Write(SpecialBytes.ColonSpacebar);

            var valueIndex = this.cache.Length;

            this.cache.Write(name);
            this.cache.Write(SpecialBytes.Equals);
            this.cache.Write(value);
            this.cache.Write("; Max-Age=");
            this.cache.Write(maxAge.ToString());
            if (!string.IsNullOrEmpty(domain))
            {
                this.cache.Write("; Domain=");
                this.cache.Write(domain);
            }
            if (!string.IsNullOrEmpty(path))
            {
                this.cache.Write("; Path=");
                this.cache.Write(path);
            }
            if (secure)
                this.cache.Write("; Secure");
            if (strict)
                this.cache.Write("; SameSite=Strict");
            if (httpOnly)
                this.cache.Write("; HttpOnly");

            var valueSize = this.cache.Length - valueIndex;

            var cookie = this.cache.ExtractString(valueIndex, valueSize);

            this.cache.Write(SpecialBytes.NewLine);

            this.headers.Add(new Tuple<string, string>(HeaderNames.SetCookie, cookie));
            this.cookies.Add(new Tuple<string, string>(name, value));

            return this;
        }

        /// <summary>
        /// Sets the body of the response with the specified string content.
        /// </summary>
        /// <param name="body">The body content as a string.</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetBody(string body = "")
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
        /// Sets the body of the response with the specified byte array content.
        /// </summary>
        /// <param name="body">The body content as a byte array.</param>
        /// <returns>The current <see cref="HttpResponse"/> instance.</returns>
        public HttpResponse SetBody(byte[] body)
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
        /// Indicates whether the response is pending header processing.
        /// </summary>
        /// <returns>True if the response is pending header processing, otherwise false.</returns>
        public bool IsPendingHeader() => !this.IsErrorSet && (this.bodyIndex == 0);

        /// <summary>
        /// Indicates whether the response is pending body processing.
        /// </summary>
        /// <returns>True if the response is pending body processing, otherwise false.</returns>
        public bool IsPendingBody() => !this.IsErrorSet && (this.bodyIndex > 0) && (this.bodySize > 0);

        /// <summary>
        /// Sets the header buffer with the specified byte array data.
        /// </summary>
        /// <param name="buffer">The byte array containing header data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to write.</param>
        /// <returns>True if the header processing is complete, otherwise false.</returns>
        public bool SetHeaderBuffer(byte[] buffer, int position, int length)
        {
            this.cache.Write(buffer, position, length);

            for (var i = this.cacheSize; i < this.cache.Length; i++)
            {
                if (i + 3 >= this.cache.Length)
                    break;

                if (this.cache[i + 0] == SpecialChars.CarriageReturn && this.cache[i + 1] == SpecialChars.NewLine && this.cache[i + 2] == SpecialChars.CarriageReturn && this.cache[i + 3] == SpecialChars.NewLine)
                {
                    var index = 0;

                    this.IsErrorSet = true;

                    var protocolIndex = index;
                    var protocolSize = 0;
                    while (this.cache[index] != SpecialChars.Spacebar)
                    {
                        protocolSize++;
                        index++;
                        if (index >= this.cache.Length)
                            return false;
                    }
                    index++;
                    if (index >= this.cache.Length)
                        return false;
                    this.protocol = this.cache.ExtractString(protocolIndex, protocolSize);

                    var statusIndex = index;
                    var statusSize = 0;
                    while (this.cache[index] != SpecialChars.Spacebar)
                    {
                        if (this.cache[index] < SpecialChars.Number0 || this.cache[index] > SpecialChars.Number9)
                            return false;
                        statusSize++;
                        index++;
                        if (index >= this.cache.Length)
                            return false;
                    }

                    var tempStatus = 0;

                    for (var j = statusIndex; j < (statusIndex + statusSize); j++)
                    {
                        tempStatus *= 10;
                        tempStatus += this.cache[j] - SpecialChars.Number0;
                    }

                    this.Status = (StatusCode)tempStatus;

                    index++;
                    if (index >= this.cache.Length)
                        return false;

                    var statusPhraseIndex = index;
                    var statusPhraseSize = 0;
                    while (this.cache[index] != SpecialChars.CarriageReturn)
                    {
                        statusPhraseSize++;
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
                    this.statusPhrase = this.cache.ExtractString(statusPhraseIndex, statusPhraseSize);

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

                        if (headerName.ToLower() == HeaderNames.SetCookie)
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
        /// Sets the body buffer with the specified byte array data.
        /// </summary>
        /// <param name="buffer">The byte array containing body data.</param>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the data to write.</param>
        /// <returns>True if the body processing is complete, otherwise false.</returns>
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
                if (this.bodySize >= 4)
                {
                    var index = this.bodyIndex + this.bodySize - 4;

                    if ((this.cache[index + 0] == SpecialChars.CarriageReturn) && (this.cache[index + 1] == SpecialChars.NewLine) && (this.cache[index + 2] == SpecialChars.CarriageReturn) &&
                        (this.cache[index + 3] == SpecialChars.NewLine))
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
