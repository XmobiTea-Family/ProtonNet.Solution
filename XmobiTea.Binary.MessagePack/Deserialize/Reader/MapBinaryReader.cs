using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize.Reader
{
    /// <summary>
    /// Implements a binary reader for deserializing map (dictionary) data from a binary stream.
    /// </summary>
    class MapBinaryReader : AbstractCollectionBinaryReader<System.Collections.IDictionary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBinaryReader"/> class.
        /// </summary>
        /// <param name="binaryDeserializer">The binary deserializer used for interpreting binary data.</param>
        public MapBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        /// <summary>
        /// Gets the binary type code associated with maps (dictionaries).
        /// </summary>
        /// <returns>The binary type code for maps as a byte.</returns>
        public override byte GetBinaryTypeCode() => BinaryTypeCode.Map;

        /// <summary>
        /// Reads and deserializes a map (dictionary) from the specified binary stream based on the provided type code.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="messagePackTypeCode">The type code used to interpret the binary data as a map.</param>
        /// <returns>A dictionary representing the deserialized map data.</returns>
        public override System.Collections.IDictionary Read(System.IO.Stream stream, byte messagePackTypeCode)
        {
            if (!this.IsMasked(MessagePackTypeCode.MapFix, messagePackTypeCode, 0x0F, out var length))
                if (messagePackTypeCode == MessagePackTypeCode.Map16) length = this.GetCollectionLength(stream, 2);
                else if (messagePackTypeCode == MessagePackTypeCode.Map32) length = this.GetCollectionLength(stream, 4);

            var answer = new System.Collections.Generic.Dictionary<object, object>();

            for (var i = 0; i < length; i++)
            {
                var elementKeyMessagePackTypeCode = this.ReadByte(stream);
                var elementKey = this.binaryDeserializer.GetReader(elementKeyMessagePackTypeCode).Read(stream, elementKeyMessagePackTypeCode);

                var elementValueMessagePackTypeCode = this.ReadByte(stream);
                var elementValue = this.binaryDeserializer.GetReader(elementValueMessagePackTypeCode).Read(stream, elementValueMessagePackTypeCode);

                answer[elementKey] = elementValue;
            }

            return answer;
        }

    }

}
