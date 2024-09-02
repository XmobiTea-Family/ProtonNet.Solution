using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DictionaryKeyIdentifyValueObjectBinaryReader : AbstractCollectionBinaryReader<IDictionary>
    {
        public DictionaryKeyIdentifyValueObjectBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyIdentifyValueObject;

        public override IDictionary Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            if (length < 0) return null;

            var collectionOfIdentifyBinaryReader = (CollectionOfIdentifyBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfIdentify);
            var collectionOfObjectBinaryReader = (CollectionOfObjectBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfObject);

            var keys = collectionOfIdentifyBinaryReader.ReadData(stream, length, out var keyBinaryTypeCode);

            var answer = this.GetDictionary(keyBinaryTypeCode);

            if (length != 0)
            {
                var values = collectionOfObjectBinaryReader.ReadData(stream, length);

                var i = 0;
                var keyObjs = new object[length];
                foreach (var key in keys)
                    keyObjs[i++] = key;

                i = 0;
                var valueObjs = new object[length];
                foreach (var value in values)
                    valueObjs[i++] = value;

                for (i = 0; i < length; i++)
                    answer[keyObjs[i]] = valueObjs[i];
            }

            return answer;
        }

        private IDictionary GetDictionary(byte keyBinaryTypeCode)
        {
            if (keyBinaryTypeCode == BinaryTypeCode.Byte)
                return new Dictionary<byte, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Boolean)
                return new Dictionary<bool, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Short)
                return new Dictionary<short, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Int)
                return new Dictionary<int, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Long)
                return new Dictionary<long, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Float)
                return new Dictionary<float, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Double)
                return new Dictionary<double, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.String)
                return new Dictionary<string, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Char)
                return new Dictionary<char, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.SByte)
                return new Dictionary<sbyte, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.UShort)
                return new Dictionary<ushort, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.UInt)
                return new Dictionary<uint, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.ULong)
                return new Dictionary<ulong, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.Decimal)
                return new Dictionary<decimal, object>();
            else if (keyBinaryTypeCode == BinaryTypeCode.DateTime)
                return new Dictionary<DateTime, object>();

            return new Dictionary<object, object>();
        }

    }

}
