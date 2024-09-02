using XmobiTea.Binary.MessagePack.Helper;
using XmobiTea.Binary.MessagePack.Serialize.Writer;
using XmobiTea.Binary.MessagePack.Serialize.Writer.Core;
using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Serialize
{
    /// <summary>
    /// Provides methods for serializing objects into MessagePack binary format.
    /// </summary>
    interface IBinarySerializer
    {
        /// <summary>
        /// Gets a binary writer based on the binary type code.
        /// </summary>
        /// <param name="binaryTypeCode">The binary type code.</param>
        /// <returns>An instance of <see cref="IBinaryWriter"/> corresponding to the type code.</returns>
        IBinaryWriter GetWriter(byte binaryTypeCode);

        /// <summary>
        /// Gets a binary writer based on the type of the object.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <returns>An instance of <see cref="IBinaryWriter"/> corresponding to the type.</returns>
        IBinaryWriter GetWriter(System.Type type);

        /// <summary>
        /// Serializes an object to a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="tObj">The object to serialize.</param>
        /// <returns>A byte array representing the serialized object.</returns>
        byte[] Serialize<T>(T tObj);

        /// <summary>
        /// Serializes an object and writes it to a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="stream">The stream to write the serialized data to.</param>
        /// <param name="tObj">The object to serialize.</param>
        void Serialize<T>(System.IO.Stream stream, T tObj);
    }

    /// <summary>
    /// Implements the <see cref="IBinarySerializer"/> interface to provide serialization capabilities for various data types.
    /// </summary>
    class BinarySerializer : IBinarySerializer
    {
        /// <summary>
        /// A dictionary mapping binary type codes to their corresponding writers.
        /// </summary>
        private System.Collections.Generic.IDictionary<byte, IBinaryWriter> writerDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializer"/> class and sets up the writers.
        /// </summary>
        public BinarySerializer()
        {
            this.writerDict = new System.Collections.Generic.Dictionary<byte, IBinaryWriter>();

            this.AddWriters();
        }

        /// <summary>
        /// Adds all the supported writers to the dictionary.
        /// </summary>
        private void AddWriters()
        {
            this.writerDict.Add(BinaryTypeCode.Nil, new NilBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Boolean, new BooleanBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.String, new StringBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Integer, new IntegerBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Float, new FloatBinaryWriter(this));

            this.writerDict.Add(BinaryTypeCode.Array, new ArrayBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Map, new MapBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Binary, new BinaryBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Extension, new ExtensionBinaryWriter(this));
        }

        /// <summary>
        /// Gets a binary writer based on the binary type code.
        /// </summary>
        /// <param name="binaryTypeCode">The binary type code.</param>
        /// <returns>An instance of <see cref="IBinaryWriter"/> corresponding to the type code.</returns>
        public IBinaryWriter GetWriter(byte binaryTypeCode) => this.writerDict[binaryTypeCode];

        /// <summary>
        /// Gets a binary writer based on the type of the object.
        /// </summary>
        /// <param name="type">The type of the object.</param>
        /// <returns>An instance of <see cref="IBinaryWriter"/> corresponding to the type.</returns>
        public IBinaryWriter GetWriter(System.Type type) => this.GetWriter(BinaryUtils.GetBinaryTypeCode(type));

        /// <summary>
        /// Serializes an object to a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="tObj">The object to serialize.</param>
        /// <returns>A byte array representing the serialized object.</returns>
        public byte[] Serialize<T>(T tObj)
        {
            if (tObj == null) return new byte[] { MessagePackTypeCode.Nil };

            var writer = this.GetWriter(tObj.GetType());

            using (var stream = new System.IO.MemoryStream())
            {
                writer.Write(stream, tObj);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Serializes an object and writes it to a stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="stream">The stream to write the serialized data to.</param>
        /// <param name="tObj">The object to serialize.</param>
        public void Serialize<T>(System.IO.Stream stream, T tObj)
        {
            if (tObj == null)
            {
                stream.WriteByte(MessagePackTypeCode.Nil);
                return;
            }

            var writer = this.GetWriter(tObj.GetType());

            writer.Write(stream, tObj);
        }

    }

}
