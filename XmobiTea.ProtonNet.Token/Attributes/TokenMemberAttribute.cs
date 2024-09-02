using System;

namespace XmobiTea.ProtonNet.Token.Attributes
{
    /// <summary>
    /// Specifies that a property is a member of a token and associates it with a code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TokenMemberAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the code associated with the token member.
        /// </summary>
        public byte Code { get; }

        /// <summary>
        /// Initializes a new instance of the TokenMemberAttribute class.
        /// </summary>
        /// <param name="code">The code associated with the token member.</param>
        public TokenMemberAttribute(byte code)
        {
            this.Code = code;
        }

    }

}
