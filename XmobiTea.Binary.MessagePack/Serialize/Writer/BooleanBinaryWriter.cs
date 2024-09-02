using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize boolean values into MessagePack binary format.
    /// </summary>
    class BooleanBinaryWriter : BinaryWriter<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public BooleanBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with boolean values.
        /// </summary>
        /// <returns>The binary type code for boolean values.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Boolean;

        /// <summary>
        /// Gets the MessagePack type code for the specified boolean value.
        /// </summary>
        /// <param name="value">The boolean value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(bool value) => value ? MessagePackTypeCode.True : MessagePackTypeCode.False;

        /// <summary>
        /// Gets the length of the serialized data for the boolean value.
        /// </summary>
        /// <param name="value">The boolean value to determine the data length for.</param>
        /// <returns>The length of the data as an integer.</returns>
        public override int GetDataLength(bool value) => 1;

        /// <summary>
        /// Writes the specified boolean value to the provided stream in binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The boolean value to write.</param>
        public override void Write(System.IO.Stream stream, bool value) => this.WriteByte(stream, this.GetMessagePackTypeCode(value));

    }

}
