using System;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize
{
    interface IBinarySerializer
    {
        IBinaryWriter GetWriter(byte binaryTypeCode);
        IBinaryWriter GetWriter(Type type);
        byte[] Serialize<T>(T tObj);
        void Serialize<T>(Stream stream, T tObj);

    }

    class BinarySerializer : IBinarySerializer
    {
        private IDictionary<byte, IBinaryWriter> writerDict { get; }

        public BinarySerializer()
        {
            this.writerDict = new Dictionary<byte, IBinaryWriter>();

            this.AddWriters();
        }

        private void AddWriters()
        {
            this.writerDict.Add(BinaryTypeCode.Byte, new ByteBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Boolean, new BooleanBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Short, new ShortBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Int, new IntBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Long, new LongBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Float, new FloatBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Double, new DoubleBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.String, new StringBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Char, new CharBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.Null, new NullBinaryWriter(this));

            this.writerDict.Add(BinaryTypeCode.CollectionOfIdentify, new CollectionOfIdentifyBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.CollectionOfObject, new CollectionOfObjectBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.DictionaryKeyIdentifyValueIdentify, new DictionaryKeyIdentifyValueIdentifyBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.DictionaryKeyObjectValueIdentify, new DictionaryKeyObjectValueIdentifyBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.DictionaryKeyIdentifyValueObject, new DictionaryKeyIdentifyValueObjectBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.DictionaryKeyObjectValueObject, new DictionaryKeyObjectValueObjectBinaryWriter(this));

            this.writerDict.Add(BinaryTypeCode.SByte, new SByteBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.UShort, new UShortBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.UInt, new UIntBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.ULong, new ULongBinaryWriter(this));

            this.writerDict.Add(BinaryTypeCode.Decimal, new DecimalBinaryWriter(this));
            this.writerDict.Add(BinaryTypeCode.DateTime, new DateTimeBinaryWriter(this));

            //this.writerDict.Add(BinaryTypeCode.UnknownType, new UnknownType(this));

        }

        public IBinaryWriter GetWriter(byte binaryTypeCode) => this.writerDict[binaryTypeCode];

        public IBinaryWriter GetWriter(Type type) => this.GetWriter(BinaryUtils.GetBinaryTypeCode(type));

        public byte[] Serialize<T>(T tObj)
        {
            if (tObj == null) return new byte[] { BinaryTypeCode.Null };

            var writer = this.GetWriter(tObj.GetType());

            using (var stream = new MemoryStream())
            {
                stream.WriteByte(writer.GetBinaryTypeCode());
                writer.Write(stream, tObj);

                return stream.ToArray();
            }
        }

        public void Serialize<T>(Stream stream, T tObj)
        {
            if (tObj == null)
            {
                stream.WriteByte(BinaryTypeCode.Null);
                return;
            }

            var writer = this.GetWriter(tObj.GetType());

            stream.WriteByte(writer.GetBinaryTypeCode());
            writer.Write(stream, tObj);
        }

    }

}
