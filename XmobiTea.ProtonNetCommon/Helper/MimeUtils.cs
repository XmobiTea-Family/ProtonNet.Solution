using System.Collections.Generic;

namespace XmobiTea.ProtonNetCommon.Helper
{
    /// <summary>
    /// Provides utility methods for handling MIME types based on file extensions.
    /// </summary>
    static class MimeUtils
    {
        /// <summary>
        /// A dictionary mapping file extensions to their corresponding MIME types.
        /// </summary>
        private static IDictionary<string, string> extensionWithMimeDict { get; }

        /// <summary>
        /// Static constructor to initialize the dictionary and populate it with common MIME types.
        /// </summary>
        static MimeUtils()
        {
            extensionWithMimeDict = new Dictionary<string, string>();

            AddExtensionWithMimes();
        }

        /// <summary>
        /// Populates the dictionary with common file extensions and their associated MIME types.
        /// </summary>
        private static void AddExtensionWithMimes()
        {
            extensionWithMimeDict[".html"] = "text/html";
            extensionWithMimeDict[".css"] = "text/css";
            extensionWithMimeDict[".js"] = "text/javascript";
            extensionWithMimeDict[".vue"] = "text/html";
            extensionWithMimeDict[".xml"] = "text/xml";

            extensionWithMimeDict[".atom"] = "application/atom+xml";
            extensionWithMimeDict[".fastsoap"] = "application/fastsoap";
            extensionWithMimeDict[".gzip"] = "application/gzip";
            extensionWithMimeDict[".json"] = "application/json";
            extensionWithMimeDict[".map"] = "application/json";
            extensionWithMimeDict[".pdf"] = "application/pdf";
            extensionWithMimeDict[".ps"] = "application/postscript";
            extensionWithMimeDict[".soap"] = "application/soap+xml";
            extensionWithMimeDict[".sql"] = "application/sql";
            extensionWithMimeDict[".xslt"] = "application/xslt+xml";
            extensionWithMimeDict[".zip"] = "application/zip";
            extensionWithMimeDict[".zlib"] = "application/zlib";

            extensionWithMimeDict[".aac"] = "audio/aac";
            extensionWithMimeDict[".ac3"] = "audio/ac3";
            extensionWithMimeDict[".mp3"] = "audio/mpeg";
            extensionWithMimeDict[".ogg"] = "audio/ogg";

            extensionWithMimeDict[".ttf"] = "font/ttf";

            extensionWithMimeDict[".bmp"] = "image/bmp";
            extensionWithMimeDict[".emf"] = "image/emf";
            extensionWithMimeDict[".gif"] = "image/gif";
            extensionWithMimeDict[".jpg"] = "image/jpeg";
            extensionWithMimeDict[".jpm"] = "image/jpm";
            extensionWithMimeDict[".jpx"] = "image/jpx";
            extensionWithMimeDict[".jrx"] = "image/jrx";
            extensionWithMimeDict[".png"] = "image/png";
            extensionWithMimeDict[".svg"] = "image/svg+xml";
            extensionWithMimeDict[".tiff"] = "image/tiff";
            extensionWithMimeDict[".wmf"] = "image/wmf";

            extensionWithMimeDict[".http"] = "message/http";
            extensionWithMimeDict[".s-http"] = "message/s-http";

            extensionWithMimeDict[".mesh"] = "model/mesh";
            extensionWithMimeDict[".vrml"] = "model/vrml";

            extensionWithMimeDict[".csv"] = "text/csv";
            extensionWithMimeDict[".plain"] = "text/plain";
            extensionWithMimeDict[".richtext"] = "text/richtext";
            extensionWithMimeDict[".rtf"] = "text/rtf";
            extensionWithMimeDict[".rtx"] = "text/rtx";
            extensionWithMimeDict[".sgml"] = "text/sgml";
            extensionWithMimeDict[".strings"] = "text/strings";
            extensionWithMimeDict[".url"] = "text/uri-list";

            extensionWithMimeDict[".H264"] = "video/H264";
            extensionWithMimeDict[".H265"] = "video/H265";
            extensionWithMimeDict[".mp4"] = "video/mp4";
            extensionWithMimeDict[".mpeg"] = "video/mpeg";
            extensionWithMimeDict[".raw"] = "video/raw";
        }

        /// <summary>
        /// Gets the MIME type associated with the given file extension.
        /// </summary>
        /// <param name="extension">The file extension for which to retrieve the MIME type.</param>
        /// <returns>
        /// The corresponding MIME type if the extension is found; otherwise, null.
        /// </returns>
        public static string GetMimeName(string extension) => extensionWithMimeDict.TryGetValue(extension, out var answer) ? answer : null;

    }

}
