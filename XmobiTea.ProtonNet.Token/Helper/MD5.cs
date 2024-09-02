using System.Text;

namespace XmobiTea.ProtonNet.Token.Helper
{
    /// <summary>
    /// Provides functionality to create an MD5 hash from a given input string.
    /// </summary>
    static class MD5
    {
        /// <summary>
        /// Generates an MD5 hash for the specified input string.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>A hexadecimal string representing the MD5 hash.</returns>
        public static string CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            using (var hash = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                var hashBytes = hash.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                    // To force the hex string to lower-case letters, use the following line instead:
                    // sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }

}
