using XmobiTea.Binary.MessagePack.Deserialize.Reader;
using XmobiTea.Binary.MessagePack.Deserialize.Reader.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Deserialize
{
    /// <summary>
    /// Defines the contract for a binary deserializer capable of deserializing data from binary streams and byte arrays.
    /// </summary>
    interface IBinaryDeserializer
    {
        /// <summary>Gets the appropriate binary reader for a given MessagePack type code.</summary>
        /// <param name="messagePackTypeCode">The type code representing the binary data type.</param>
        /// <returns>The binary reader capable of reading the specified data type.</returns>
        IBinaryReader GetReader(byte messagePackTypeCode);

        /// <summary>Deserializes binary data into a specified type from a stream.</summary>
        /// <typeparam name="T">The type into which the data will be deserialized.</typeparam>
        /// <param name="stream">The stream containing the binary data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(System.IO.Stream stream);

        /// <summary>Deserializes binary data into a specified type from a byte array.</summary>
        /// <typeparam name="T">The type into which the data will be deserialized.</typeparam>
        /// <param name="data">The byte array containing the binary data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        T Deserialize<T>(byte[] data);

    }

    /// <summary>
    /// Implements the <see cref="IBinaryDeserializer"/> interface, providing deserialization functionality for binary data.
    /// </summary>
    class BinaryDeserializer : IBinaryDeserializer
    {
        /// <summary>
        /// Gets the dictionary mapping MessagePack type codes to their corresponding binary readers.
        /// </summary>
        private System.Collections.Generic.IDictionary<byte, IBinaryReader> messagePackTypeCodeWithReaderDict { get; }

        /// <summary>
        /// Gets the dictionary mapping binary type codes to their corresponding binary readers.
        /// </summary>
        private System.Collections.Generic.IDictionary<byte, IBinaryReader> binaryTypeCodeWithReaderDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDeserializer"/> class and sets up the readers.
        /// </summary>
        public BinaryDeserializer()
        {
            this.messagePackTypeCodeWithReaderDict = new System.Collections.Generic.Dictionary<byte, IBinaryReader>();
            this.binaryTypeCodeWithReaderDict = new System.Collections.Generic.Dictionary<byte, IBinaryReader>();

            this.AddReaders();
        }

        /// <summary>
        /// Adds readers for both binary type codes and MessagePack type codes to their respective dictionaries.
        /// </summary>
        private void AddReaders()
        {
            this.AddBinaryTypeCodeReaders();
            this.AddMessagePackTypeCodeReaders();
        }

        /// <summary>
        /// Adds readers based on binary type codes.
        /// </summary>
        private void AddBinaryTypeCodeReaders()
        {
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer] = new IntegerBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Nil] = new NilBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Boolean] = new BooleanBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Float] = new FloatBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String] = new StringBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Binary] = new BinaryBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Array] = new ArrayBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Map] = new MapBinaryReader(this);
            this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension] = new ExtensionBinaryReader(this);
        }

        /// <summary>
        /// Adds readers based on MessagePack type codes.
        /// </summary>
        private void AddMessagePackTypeCodeReaders()
        {
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Nil, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Nil]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.True, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Boolean]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.False, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Boolean]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.NegativeIntFix, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.PositiveIntFix, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.UInt8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.UInt16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.UInt32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.UInt64, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Int8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Int16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Int32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Int64, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Float32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Float]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Float64, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Float]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.StrFix, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Str8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Str16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Str32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Bin8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Binary]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Bin16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Binary]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Bin32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Binary]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ArrayFix, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Array]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Array16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Array]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Array32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Array]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.MapFix, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Map]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Map16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Map]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Map32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Map]);

            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ExtFix1, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ExtFix2, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ExtFix4, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ExtFix8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.ExtFix16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Ext8, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Ext16, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
            this.messagePackTypeCodeWithReaderDict.Add(MessagePackTypeCode.Ext32, this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Extension]);
        }

        /// <summary>
        /// Gets the appropriate binary reader for a given MessagePack type code.
        /// </summary>
        /// <param name="messagePackTypeCode">The type code representing the binary data type.</param>
        /// <returns>The binary reader capable of reading the specified data type.</returns>
        public IBinaryReader GetReader(byte messagePackTypeCode)
        {
            if (this.messagePackTypeCodeWithReaderDict.TryGetValue(messagePackTypeCode, out var binaryReader))
                return binaryReader;

            if ((messagePackTypeCode & 0xE0) == 0xE0 || (messagePackTypeCode & 0x80) == 0)
                binaryReader = this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Integer];
            else if ((messagePackTypeCode & 0xA0) == 0xA0)
                binaryReader = this.binaryTypeCodeWithReaderDict[BinaryTypeCode.String];
            else if ((messagePackTypeCode & 0x90) == 0x90)
                binaryReader = this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Array];
            else if ((messagePackTypeCode & 0x80) == 0x80)
                binaryReader = this.binaryTypeCodeWithReaderDict[BinaryTypeCode.Map];

            return binaryReader;
        }

        /// <summary>
        /// Deserializes binary data into a specified type from a byte array.
        /// </summary>
        /// <typeparam name="T">The type into which the data will be deserialized.</typeparam>
        /// <param name="data">The byte array containing the binary data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(byte[] data)
        {
            using (var stream = new System.IO.MemoryStream(data))
                return this.Deserialize<T>(stream);
        }

        /// <summary>
        /// Deserializes binary data into a specified type from a stream.
        /// </summary>
        /// <typeparam name="T">The type into which the data will be deserialized.</typeparam>
        /// <param name="stream">The stream containing the binary data.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(System.IO.Stream stream)
        {
            var messagePackTypeCode = (byte)stream.ReadByte();

            var reader = this.GetReader(messagePackTypeCode);

            return (T)reader.Read(stream, messagePackTypeCode);
        }

    }

}
