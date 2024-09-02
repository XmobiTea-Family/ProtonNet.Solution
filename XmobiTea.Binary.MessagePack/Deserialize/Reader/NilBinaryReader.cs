using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing 'nil' values (representing null) from a binary stream.
    /// </summary>
    class NilBinaryReader : BinaryReader<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NilBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public NilBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with 'nil' values.
        /// </summary>
        /// <returns>The binary type code for 'nil' as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Nil;

        /// <summary>
        /// Reads and deserializes a 'nil' value from the specified binary stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as 'nil'.</param>
        /// <returns>Always returns null, as 'nil' represents a null value.</returns>
        public override object Read(System.IO.Stream stream, byte messagePackTypeCode) => null;

    }

}
