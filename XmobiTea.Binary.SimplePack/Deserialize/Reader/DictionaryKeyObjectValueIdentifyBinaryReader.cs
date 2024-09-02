using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DictionaryKeyObjectValueIdentifyBinaryReader : AbstractCollectionBinaryReader<IDictionary>
    {
        public DictionaryKeyObjectValueIdentifyBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyObjectValueIdentify;

        public override IDictionary Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            if (length < 0) return null;

            var collectionOfIdentifyBinaryReader = (CollectionOfIdentifyBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfIdentify);
            var collectionOfObjectBinaryReader = (CollectionOfObjectBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfObject);

            var values = collectionOfIdentifyBinaryReader.ReadData(stream, length, out var valueBinaryTypeCode);

            var answer = this.GetDictionary(valueBinaryTypeCode);

            if (length != 0)
            {
                var keys = collectionOfObjectBinaryReader.ReadData(stream, length);

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

        private IDictionary GetDictionary(byte valueBinaryTypeCode)
        {
            if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                return new Dictionary<object, byte>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                return new Dictionary<object, bool>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Short)
                return new Dictionary<object, short>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Int)
                return new Dictionary<object, int>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Long)
                return new Dictionary<object, long>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Float)
                return new Dictionary<object, float>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Double)
                return new Dictionary<object, double>();
            else if (valueBinaryTypeCode == BinaryTypeCode.String)
                return new Dictionary<object, string>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Char)
                return new Dictionary<object, char>();
            else if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                return new Dictionary<object, sbyte>();
            else if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                return new Dictionary<object, ushort>();
            else if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                return new Dictionary<object, uint>();
            else if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                return new Dictionary<object, ulong>();
            else if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                return new Dictionary<object, decimal>();
            else if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                return new Dictionary<object, DateTime>();

            return new Dictionary<object, object>();
        }

    }

}
