using System;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize
{
    interface IBinaryDeserializer
    {
        IBinaryReader GetReader(byte binaryTypeCode);
        IBinaryReader GetReader(Type type);
        T Deserialize<T>(Stream stream);
        T Deserialize<T>(byte[] data);

    }

    class BinaryDeserializer : IBinaryDeserializer
    {
        private IDictionary<byte, IBinaryReader> readerDict { get; }

        public BinaryDeserializer()
        {
            this.readerDict = new Dictionary<byte, IBinaryReader>();

            this.AddReaders();
        }

        private void AddReaders()
        {
            this.readerDict.Add(BinaryTypeCode.Byte, new ByteBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Boolean, new BooleanBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Short, new ShortBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Int, new IntBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Long, new LongBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Float, new FloatBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Double, new DoubleBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.String, new StringBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Char, new CharBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.Null, new NullBinaryReader(this));

            this.readerDict.Add(BinaryTypeCode.CollectionOfIdentify, new CollectionOfIdentifyBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.CollectionOfObject, new CollectionOfObjectBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.DictionaryKeyIdentifyValueIdentify, new DictionaryKeyIdentifyValueIdentifyBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.DictionaryKeyObjectValueIdentify, new DictionaryKeyObjectValueIdentifyBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.DictionaryKeyIdentifyValueObject, new DictionaryKeyIdentifyValueObjectBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.DictionaryKeyObjectValueObject, new DictionaryKeyObjectValueObjectBinaryReader(this));

            this.readerDict.Add(BinaryTypeCode.SByte, new SByteBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.UShort, new UShortBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.UInt, new UIntBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.ULong, new ULongBinaryReader(this));

            this.readerDict.Add(BinaryTypeCode.Decimal, new DecimalBinaryReader(this));
            this.readerDict.Add(BinaryTypeCode.DateTime, new DateTimeBinaryReader(this));
        }

        public IBinaryReader GetReader(byte binaryTypeCode) => this.readerDict[binaryTypeCode];

        public IBinaryReader GetReader(Type type) => this.GetReader(BinaryUtils.GetBinaryTypeCode(type));

        public T Deserialize<T>(byte[] data)
        {
            using (var stream = new MemoryStream(data))
                return this.Deserialize<T>(stream);
        }

        public T Deserialize<T>(Stream stream)
        {
            var binaryTypeCode = (byte)stream.ReadByte();

            var reader = this.GetReader(binaryTypeCode);

            return (T)reader.Read(stream);
        }

    }

}
