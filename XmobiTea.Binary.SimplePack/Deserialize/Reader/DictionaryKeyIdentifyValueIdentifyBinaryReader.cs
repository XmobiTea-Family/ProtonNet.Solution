using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DictionaryKeyIdentifyValueIdentifyBinaryReader : AbstractCollectionBinaryReader<IDictionary>
    {
        public DictionaryKeyIdentifyValueIdentifyBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyIdentifyValueIdentify;

        public override IDictionary Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            if (length < 0) return null;

            var collectionOfIdentifyBinaryReader = (CollectionOfIdentifyBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfIdentify);

            var keys = collectionOfIdentifyBinaryReader.ReadData(stream, length, out var keyBinaryTypeCode);
            var values = collectionOfIdentifyBinaryReader.ReadData(stream, length, out var valueBinaryTypeCode);

            var answer = this.GetDictionary(keyBinaryTypeCode, valueBinaryTypeCode);

            if (length != 0)
            {
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

        private IDictionary GetDictionary(byte keyBinaryTypeCode, byte valueBinaryTypeCode)
        {
            if (keyBinaryTypeCode == BinaryTypeCode.Byte)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<byte, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<byte, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<byte, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<byte, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<byte, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<byte, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<byte, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<byte, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<byte, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<byte, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<byte, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<byte, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<byte, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<byte, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<byte, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Boolean)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<bool, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<bool, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<bool, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<bool, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<bool, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<bool, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<bool, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<bool, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<bool, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<bool, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<bool, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<bool, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<bool, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<bool, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<bool, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Short)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<short, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<short, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<short, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<short, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<short, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<short, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<short, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<short, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<short, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<short, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<short, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<short, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<short, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<short, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<short, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Int)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<int, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<int, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<int, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<int, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<int, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<int, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<int, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<int, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<int, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<int, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<int, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<int, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<int, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<int, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<int, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Long)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<long, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<long, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<long, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<long, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<long, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<long, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<long, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<long, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<long, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<long, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<long, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<long, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<long, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<long, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<long, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Float)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<float, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<float, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<float, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<float, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<float, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<float, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<float, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<float, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<float, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<float, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<float, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<float, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<float, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<float, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<float, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Double)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<double, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<double, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<double, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<double, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<double, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<double, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<double, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<double, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<double, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<double, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<double, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<double, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<double, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<double, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<double, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.String)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<string, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<string, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<string, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<string, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<string, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<string, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<string, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<string, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<string, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<string, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<string, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<string, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<string, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<string, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<string, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Char)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<char, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<char, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<char, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<char, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<char, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<char, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<char, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<char, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<char, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<char, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<char, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<char, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<char, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<char, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<char, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.SByte)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<sbyte, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<sbyte, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<sbyte, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<sbyte, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<sbyte, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<sbyte, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<sbyte, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<sbyte, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<sbyte, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<sbyte, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<sbyte, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<sbyte, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<sbyte, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<sbyte, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<sbyte, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.UShort)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<ushort, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<ushort, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<ushort, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<ushort, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<ushort, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<ushort, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<ushort, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<ushort, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<ushort, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<ushort, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<ushort, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<ushort, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<ushort, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<ushort, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<ushort, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.UInt)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<uint, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<uint, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<uint, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<uint, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<uint, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<uint, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<uint, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<uint, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<uint, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<uint, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<uint, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<uint, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<uint, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<uint, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<uint, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.ULong)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<ulong, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<ulong, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<ulong, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<ulong, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<ulong, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<ulong, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<ulong, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<ulong, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<ulong, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<ulong, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<ulong, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<ulong, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<ulong, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<ulong, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<ulong, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.Decimal)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<decimal, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<decimal, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<decimal, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<decimal, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<decimal, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<decimal, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<decimal, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<decimal, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<decimal, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<decimal, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<decimal, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<decimal, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<decimal, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<decimal, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<decimal, DateTime>();
            }
            else if (keyBinaryTypeCode == BinaryTypeCode.DateTime)
            {
                if (valueBinaryTypeCode == BinaryTypeCode.Byte)
                    return new Dictionary<DateTime, byte>();

                if (valueBinaryTypeCode == BinaryTypeCode.Boolean)
                    return new Dictionary<DateTime, bool>();

                if (valueBinaryTypeCode == BinaryTypeCode.Short)
                    return new Dictionary<DateTime, short>();

                if (valueBinaryTypeCode == BinaryTypeCode.Int)
                    return new Dictionary<DateTime, int>();

                if (valueBinaryTypeCode == BinaryTypeCode.Long)
                    return new Dictionary<DateTime, long>();

                if (valueBinaryTypeCode == BinaryTypeCode.Float)
                    return new Dictionary<DateTime, float>();

                if (valueBinaryTypeCode == BinaryTypeCode.Double)
                    return new Dictionary<DateTime, double>();

                if (valueBinaryTypeCode == BinaryTypeCode.String)
                    return new Dictionary<DateTime, string>();

                if (valueBinaryTypeCode == BinaryTypeCode.Char)
                    return new Dictionary<DateTime, char>();

                if (valueBinaryTypeCode == BinaryTypeCode.SByte)
                    return new Dictionary<DateTime, sbyte>();

                if (valueBinaryTypeCode == BinaryTypeCode.UShort)
                    return new Dictionary<DateTime, ushort>();

                if (valueBinaryTypeCode == BinaryTypeCode.UInt)
                    return new Dictionary<DateTime, uint>();

                if (valueBinaryTypeCode == BinaryTypeCode.ULong)
                    return new Dictionary<DateTime, ulong>();

                if (valueBinaryTypeCode == BinaryTypeCode.Decimal)
                    return new Dictionary<DateTime, decimal>();

                if (valueBinaryTypeCode == BinaryTypeCode.DateTime)
                    return new Dictionary<DateTime, DateTime>();
            }

            return new Dictionary<object, object>();
        }

    }

}
