using System;
using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class CollectionOfIdentifyBinaryReader : AbstractCollectionBinaryReader<ICollection>
    {
        public CollectionOfIdentifyBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.CollectionOfIdentify;

        public override ICollection Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            return this.ReadData(stream, length, out var _);
        }

        public ICollection ReadData(Stream stream, int length, out byte elementBinaryTypeCode)
        {
            if (length < 0)
            {
                elementBinaryTypeCode = BinaryTypeCode.Null;
                return null;
            }

            elementBinaryTypeCode = this.ReadByte(stream);

            if (elementBinaryTypeCode == BinaryTypeCode.Byte)
                return this.ReadByteCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Boolean)
                return this.ReadBooleanCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Short)
                return this.ReadShortCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Int)
                return this.ReadIntCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Long)
                return this.ReadLongCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Float)
                return this.ReadFloatCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Double)
                return this.ReadDoubleCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.String)
                return this.ReadStringCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Char)
                return this.ReadCharCollection(stream, length);

            //if (binaryTypeCode == BinaryTypeCode.Null)
            //    return this.ReadNullCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.SByte)
                return this.ReadSByteCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.UShort)
                return this.ReadUShortCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.UInt)
                return this.ReadUIntCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.ULong)
                return this.ReadULongCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.Decimal)
                return this.ReadDecimalCollection(stream, length);

            if (elementBinaryTypeCode == BinaryTypeCode.DateTime)
                return this.ReadDateTimeCollection(stream, length);

            return null;
        }

        //private ICollection ReadNullCollection(Stream stream, int length)
        //{
        //    var answer = new object[length];

        //    for (var i = 0; i < length; i++)
        //        answer[i] = null;

        //    return answer;
        //}

        private ICollection ReadBooleanCollection(Stream stream, int length)
        {
            var binaryReader = (BooleanBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Boolean);

            var answer = new bool[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadCharCollection(Stream stream, int length)
        {
            var binaryReader = (CharBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Char);

            var answer = new char[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadSByteCollection(Stream stream, int length)
        {
            var binaryReader = (SByteBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.SByte);

            var answer = new sbyte[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadByteCollection(Stream stream, int length)
        {
            var binaryReader = (ByteBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Byte);

            var answer = new byte[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadShortCollection(Stream stream, int length)
        {
            var binaryReader = (ShortBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Short);

            var answer = new short[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadUShortCollection(Stream stream, int length)
        {
            var binaryReader = (UShortBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.UShort);

            var answer = new ushort[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadIntCollection(Stream stream, int length)
        {
            var binaryReader = (IntBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Int);

            var answer = new int[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadUIntCollection(Stream stream, int length)
        {
            var binaryReader = (UIntBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.UInt);

            var answer = new uint[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadLongCollection(Stream stream, int length)
        {
            var binaryReader = (LongBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Long);

            var answer = new long[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadULongCollection(Stream stream, int length)
        {
            var binaryReader = (ULongBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.ULong);

            var answer = new ulong[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadFloatCollection(Stream stream, int length)
        {
            var binaryReader = (FloatBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Float);

            var answer = new float[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadDoubleCollection(Stream stream, int length)
        {
            var binaryReader = (DoubleBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Double);

            var answer = new double[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadDecimalCollection(Stream stream, int length)
        {
            var binaryReader = (DecimalBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.Decimal);

            var answer = new decimal[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadDateTimeCollection(Stream stream, int length)
        {
            var binaryReader = (DateTimeBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.DateTime);

            var answer = new DateTime[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

        private ICollection ReadStringCollection(Stream stream, int length)
        {
            var binaryReader = (StringBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.String);

            var answer = new string[length];

            for (var i = 0; i < length; i++)
                answer[i] = binaryReader.Read(stream);

            return answer;
        }

    }

}
