using System.IO;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader.Core
{
    interface IBinaryReader
    {
        byte GetBinaryTypeCode();

        object Read(Stream stream);

    }

    interface IBinaryReader<T>
    {
        T Read(Stream stream);

    }

    abstract class BinaryReader<T> : IBinaryReader<T>, IBinaryReader
    {
        protected IBinaryDeserializer binaryDeserializer { get; }

        public BinaryReader(IBinaryDeserializer binaryDeserializer)
        {
            this.binaryDeserializer = binaryDeserializer;
        }

        public abstract byte GetBinaryTypeCode();

        public abstract T Read(Stream stream);

        object IBinaryReader.Read(Stream stream) => this.Read(stream);

        protected byte[] ReadBytes(Stream stream, ushort length)
        {
            var answer = new byte[length];

            stream.Read(answer, 0, length);

            return answer;
        }

        protected byte ReadByte(Stream stream) => (byte)stream.ReadByte();

    }

}
