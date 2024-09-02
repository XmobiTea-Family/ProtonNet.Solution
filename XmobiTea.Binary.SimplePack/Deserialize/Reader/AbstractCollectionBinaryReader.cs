using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    abstract class AbstractCollectionBinaryReader<T> : BinaryReader<T>
    {
        protected AbstractCollectionBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        protected int GetCollectionLength(Stream stream)
        {
            var byte0 = this.ReadByte(stream);

            var bit07 = (byte0 & 1 << 7) != 0;
            var bit06 = (byte0 & 1 << 6) != 0;

            if (!bit07) // khong con them data nao nua
            {
                if (bit06) return -1;   // bit thu 6 ma 1 thi day la data null

                return byte0;
            }

            var byte1 = this.ReadByte(stream);
            var bit17 = (byte1 & 1 << 7) != 0;
            if (!bit17)
            {
                return this.GetCollectionLengthLt16384(byte0, byte1);
            }

            var byte2 = this.ReadByte(stream);
            var bit27 = (byte2 & 1 << 7) != 0;
            if (!bit27)
            {
                return this.GetCollectionLengthLt2097152(byte0, byte1, byte2);
            }

            var byte3 = this.ReadByte(stream);

            return this.GetCollectionLengthLt268435456(byte0, byte1, byte2, byte3);
        }

        private int GetCollectionLengthLt16384(byte byte0, byte byte1)
        {
            var answer = 0;

            if ((byte0 & 1 << 0) != 0) answer |= 1 << 0;
            if ((byte0 & 1 << 1) != 0) answer |= 1 << 1;
            if ((byte0 & 1 << 2) != 0) answer |= 1 << 2;
            if ((byte0 & 1 << 3) != 0) answer |= 1 << 3;
            if ((byte0 & 1 << 4) != 0) answer |= 1 << 4;
            if ((byte0 & 1 << 5) != 0) answer |= 1 << 5;
            if ((byte0 & 1 << 6) != 0) answer |= 1 << 6;

            if ((byte1 & 1 << 0) != 0) answer |= 1 << 7;
            if ((byte1 & 1 << 1) != 0) answer |= 1 << 8;
            if ((byte1 & 1 << 2) != 0) answer |= 1 << 9;
            if ((byte1 & 1 << 3) != 0) answer |= 1 << 10;
            if ((byte1 & 1 << 4) != 0) answer |= 1 << 11;
            if ((byte1 & 1 << 5) != 0) answer |= 1 << 12;
            if ((byte1 & 1 << 6) != 0) answer |= 1 << 13;

            return answer;
        }

        private int GetCollectionLengthLt2097152(byte byte0, byte byte1, byte byte2)
        {
            var answer = 0;

            if ((byte0 & 1 << 0) != 0) answer |= 1 << 0;
            if ((byte0 & 1 << 1) != 0) answer |= 1 << 1;
            if ((byte0 & 1 << 2) != 0) answer |= 1 << 2;
            if ((byte0 & 1 << 3) != 0) answer |= 1 << 3;
            if ((byte0 & 1 << 4) != 0) answer |= 1 << 4;
            if ((byte0 & 1 << 5) != 0) answer |= 1 << 5;
            if ((byte0 & 1 << 6) != 0) answer |= 1 << 6;

            if ((byte1 & 1 << 0) != 0) answer |= 1 << 7;
            if ((byte1 & 1 << 1) != 0) answer |= 1 << 8;
            if ((byte1 & 1 << 2) != 0) answer |= 1 << 9;
            if ((byte1 & 1 << 3) != 0) answer |= 1 << 10;
            if ((byte1 & 1 << 4) != 0) answer |= 1 << 11;
            if ((byte1 & 1 << 5) != 0) answer |= 1 << 12;
            if ((byte1 & 1 << 6) != 0) answer |= 1 << 13;

            if ((byte2 & 1 << 0) != 0) answer |= 1 << 14;
            if ((byte2 & 1 << 1) != 0) answer |= 1 << 15;
            if ((byte2 & 1 << 2) != 0) answer |= 1 << 16;
            if ((byte2 & 1 << 3) != 0) answer |= 1 << 17;
            if ((byte2 & 1 << 4) != 0) answer |= 1 << 18;
            if ((byte2 & 1 << 5) != 0) answer |= 1 << 19;
            if ((byte2 & 1 << 6) != 0) answer |= 1 << 20;

            return answer;
        }

        private int GetCollectionLengthLt268435456(byte byte0, byte byte1, byte byte2, byte byte3)
        {
            var answer = 0;

            if ((byte0 & 1 << 0) != 0) answer |= 1 << 0;
            if ((byte0 & 1 << 1) != 0) answer |= 1 << 1;
            if ((byte0 & 1 << 2) != 0) answer |= 1 << 2;
            if ((byte0 & 1 << 3) != 0) answer |= 1 << 3;
            if ((byte0 & 1 << 4) != 0) answer |= 1 << 4;
            if ((byte0 & 1 << 5) != 0) answer |= 1 << 5;
            if ((byte0 & 1 << 6) != 0) answer |= 1 << 6;

            if ((byte1 & 1 << 0) != 0) answer |= 1 << 7;
            if ((byte1 & 1 << 1) != 0) answer |= 1 << 8;
            if ((byte1 & 1 << 2) != 0) answer |= 1 << 9;
            if ((byte1 & 1 << 3) != 0) answer |= 1 << 10;
            if ((byte1 & 1 << 4) != 0) answer |= 1 << 11;
            if ((byte1 & 1 << 5) != 0) answer |= 1 << 12;
            if ((byte1 & 1 << 6) != 0) answer |= 1 << 13;

            if ((byte2 & 1 << 0) != 0) answer |= 1 << 14;
            if ((byte2 & 1 << 1) != 0) answer |= 1 << 15;
            if ((byte2 & 1 << 2) != 0) answer |= 1 << 16;
            if ((byte2 & 1 << 3) != 0) answer |= 1 << 17;
            if ((byte2 & 1 << 4) != 0) answer |= 1 << 18;
            if ((byte2 & 1 << 5) != 0) answer |= 1 << 19;
            if ((byte2 & 1 << 6) != 0) answer |= 1 << 20;

            if ((byte3 & 1 << 0) != 0) answer |= 1 << 21;
            if ((byte3 & 1 << 1) != 0) answer |= 1 << 22;
            if ((byte3 & 1 << 2) != 0) answer |= 1 << 23;
            if ((byte3 & 1 << 3) != 0) answer |= 1 << 24;
            if ((byte3 & 1 << 4) != 0) answer |= 1 << 25;
            if ((byte3 & 1 << 5) != 0) answer |= 1 << 26;
            if ((byte3 & 1 << 6) != 0) answer |= 1 << 27;
            if ((byte3 & 1 << 7) != 0) answer |= 1 << 28;

            return answer;
        }

    }

}
