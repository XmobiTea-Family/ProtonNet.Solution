using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize.Writer
{
    /// <summary>
    /// Provides functionality to serialize a null or nil object into MessagePack binary format.
    /// </summary>
    class NilBinaryWriter : BinaryWriter<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NilBinaryWriter"/> class.
        /// </summary>
        /// <param name="binarySerializer">The binary serializer used by this writer.</param>
        public NilBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with nil objects.
        /// </summary>
        /// <returns>The binary type code for nil objects.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Nil;

        /// <summary>
        /// Gets the MessagePack type code for a nil object.
        /// </summary>
        /// <param name="value">The object value to determine the MessagePack type code for.</param>
        /// <returns>The MessagePack type code as a byte.</returns>
        public override byte GetMessagePackTypeCode(object value) => MessagePackTypeCode.Nil;

        /// <summary>
        /// Gets the total length of the serialized data for the nil object.
        /// </summary>
        /// <param name="value">The nil object for which to calculate the data length.</param>
        /// <returns>The total length of the serialized data.</returns>
        public override int GetDataLength(object value) => 1;

        /// <summary>
        /// Writes the nil object to the provided stream in MessagePack binary format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The nil object to serialize.</param>
        public override void Write(System.IO.Stream stream, object value) => this.WriteByte(stream, this.GetMessagePackTypeCode(value));

    }

}
