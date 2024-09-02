using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    abstract class AbstractCollectionBinaryWriter<T> : BinaryWriter<T>
    {
        protected AbstractCollectionBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }


        protected int GetLengthByte(int length) => length < 64 ? 1 : length < 16384 ? 2 : length < 2097152 ? 3 : length < 536870912 ? 4 : 0;

        protected void WriteCollectionLength(Stream stream, int length)
        {
            if (length < 0)
            {
                this.WriteCollectionNull(stream, length);
                return;
            }

            // 2^6 (muon 2 bit) (1 bit dau cho biet con du lieu length hay khong, 1 bit tiep theo cho biet day la du lieu null hay khong)
            if (length < 64)
            {
                this.WriteCollectionLengthLt64(stream, length);
                return;
            }

            // 2 ^ 14 (muon 2 bit)
            if (length < 16384)
            {
                this.WriteCollectionLengthLt16384(stream, length);
                return;
            }

            // 2 ^ 21 (muon 3 bit)
            if (length < 2097152)
            {
                this.WriteCollectionLengthLt2097152(stream, length);
                return;
            }

            // 2 ^ 29 (chi muon 3 bit)
            if (length < 268435456)
            {
                this.WriteCollectionLengthLt268435456(stream, length);
                return;
            }

            throw new Exception("Length too long > 268435456, length is " + length);
        }

        private void WriteCollectionNull(Stream stream, int length)
        {
            byte byte0 = 0;

            byte0 |= 1 << 6;

            this.WriteByte(stream, byte0);
        }

        private void WriteCollectionLengthLt64(Stream stream, int length)
        {
            this.WriteByte(stream, (byte)length);
        }

        private void WriteCollectionLengthLt16384(Stream stream, int length)
        {
            byte byte0 = 0;
            byte0 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte1 = 0;

            if ((length & 1 << 0) != 0) byte0 |= 1 << 0;
            if ((length & 1 << 1) != 0) byte0 |= 1 << 1;
            if ((length & 1 << 2) != 0) byte0 |= 1 << 2;
            if ((length & 1 << 3) != 0) byte0 |= 1 << 3;
            if ((length & 1 << 4) != 0) byte0 |= 1 << 4;
            if ((length & 1 << 5) != 0) byte0 |= 1 << 5;
            if ((length & 1 << 6) != 0) byte0 |= 1 << 6;

            if ((length & 1 << 7) != 0) byte1 |= 1 << 0;
            if ((length & 1 << 8) != 0) byte1 |= 1 << 1;
            if ((length & 1 << 9) != 0) byte1 |= 1 << 2;
            if ((length & 1 << 10) != 0) byte1 |= 1 << 3;
            if ((length & 1 << 11) != 0) byte1 |= 1 << 4;
            if ((length & 1 << 12) != 0) byte1 |= 1 << 5;
            if ((length & 1 << 13) != 0) byte1 |= 1 << 6;

            this.WriteByte(stream, byte0);
            this.WriteByte(stream, byte1);
        }

        private void WriteCollectionLengthLt2097152(Stream stream, int length)
        {
            byte byte0 = 0;
            byte0 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte1 = 0;
            byte1 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte2 = 0;

            if ((length & 1 << 0) != 0) byte0 |= 1 << 0;
            if ((length & 1 << 1) != 0) byte0 |= 1 << 1;
            if ((length & 1 << 2) != 0) byte0 |= 1 << 2;
            if ((length & 1 << 3) != 0) byte0 |= 1 << 3;
            if ((length & 1 << 4) != 0) byte0 |= 1 << 4;
            if ((length & 1 << 5) != 0) byte0 |= 1 << 5;
            if ((length & 1 << 6) != 0) byte0 |= 1 << 6;

            if ((length & 1 << 7) != 0) byte1 |= 1 << 0;
            if ((length & 1 << 8) != 0) byte1 |= 1 << 1;
            if ((length & 1 << 9) != 0) byte1 |= 1 << 2;
            if ((length & 1 << 10) != 0) byte1 |= 1 << 3;
            if ((length & 1 << 11) != 0) byte1 |= 1 << 4;
            if ((length & 1 << 12) != 0) byte1 |= 1 << 5;
            if ((length & 1 << 13) != 0) byte1 |= 1 << 6;

            if ((length & 1 << 14) != 0) byte2 |= 1 << 0;
            if ((length & 1 << 15) != 0) byte2 |= 1 << 1;
            if ((length & 1 << 16) != 0) byte2 |= 1 << 2;
            if ((length & 1 << 17) != 0) byte2 |= 1 << 3;
            if ((length & 1 << 18) != 0) byte2 |= 1 << 4;
            if ((length & 1 << 19) != 0) byte2 |= 1 << 5;
            if ((length & 1 << 20) != 0) byte2 |= 1 << 6;

            this.WriteByte(stream, byte0);
            this.WriteByte(stream, byte1);
            this.WriteByte(stream, byte2);
        }

        private void WriteCollectionLengthLt268435456(Stream stream, int length)
        {
            byte byte0 = 0;
            byte0 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte1 = 0;
            byte1 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte2 = 0;
            byte2 |= 1 << 7; // cho biet con 1 byte nua de tinh cho length

            byte byte3 = 0;

            if ((length & 1 << 0) != 0) byte0 |= 1 << 0;
            if ((length & 1 << 1) != 0) byte0 |= 1 << 1;
            if ((length & 1 << 2) != 0) byte0 |= 1 << 2;
            if ((length & 1 << 3) != 0) byte0 |= 1 << 3;
            if ((length & 1 << 4) != 0) byte0 |= 1 << 4;
            if ((length & 1 << 5) != 0) byte0 |= 1 << 5;
            if ((length & 1 << 6) != 0) byte0 |= 1 << 6;

            if ((length & 1 << 7) != 0) byte1 |= 1 << 0;
            if ((length & 1 << 8) != 0) byte1 |= 1 << 1;
            if ((length & 1 << 9) != 0) byte1 |= 1 << 2;
            if ((length & 1 << 10) != 0) byte1 |= 1 << 3;
            if ((length & 1 << 11) != 0) byte1 |= 1 << 4;
            if ((length & 1 << 12) != 0) byte1 |= 1 << 5;
            if ((length & 1 << 13) != 0) byte1 |= 1 << 6;

            if ((length & 1 << 14) != 0) byte2 |= 1 << 0;
            if ((length & 1 << 15) != 0) byte2 |= 1 << 1;
            if ((length & 1 << 16) != 0) byte2 |= 1 << 2;
            if ((length & 1 << 17) != 0) byte2 |= 1 << 3;
            if ((length & 1 << 18) != 0) byte2 |= 1 << 4;
            if ((length & 1 << 19) != 0) byte2 |= 1 << 5;
            if ((length & 1 << 20) != 0) byte2 |= 1 << 6;

            if ((length & 1 << 21) != 0) byte3 |= 1 << 0;
            if ((length & 1 << 22) != 0) byte3 |= 1 << 1;
            if ((length & 1 << 23) != 0) byte3 |= 1 << 2;
            if ((length & 1 << 24) != 0) byte3 |= 1 << 3;
            if ((length & 1 << 25) != 0) byte3 |= 1 << 4;
            if ((length & 1 << 26) != 0) byte3 |= 1 << 5;
            if ((length & 1 << 27) != 0) byte3 |= 1 << 6;
            if ((length & 1 << 28) != 0) byte3 |= 1 << 7;

            this.WriteByte(stream, byte0);
            this.WriteByte(stream, byte1);
            this.WriteByte(stream, byte2);
            this.WriteByte(stream, byte3);
        }

    }

}
