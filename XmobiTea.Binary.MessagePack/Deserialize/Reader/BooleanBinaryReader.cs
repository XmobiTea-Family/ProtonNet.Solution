using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing boolean values from a binary stream.
    /// </summary>
    class BooleanBinaryReader : BinaryReader<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public BooleanBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with boolean values.
        /// </summary>
        /// <returns>The binary type code for boolean values as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Boolean;

        /// <summary>
        /// Reads and deserializes a boolean value from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as a boolean.</param>
        /// <returns>A boolean value representing the deserialized data.</returns>
        public override bool Read(System.IO.Stream stream, byte messagePackTypeCode) => messagePackTypeCode == MessagePackTypeCode.True;

    }

}
