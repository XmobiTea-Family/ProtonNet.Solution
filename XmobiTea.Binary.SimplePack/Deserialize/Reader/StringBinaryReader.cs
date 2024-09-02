using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class StringBinaryReader : AbstractCollectionBinaryReader<string>
    {
        public StringBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.String;

        public override string Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            if (length < 0) return null;

            if (length == 0) return string.Empty;

            return System.Text.Encoding.UTF8.GetString(this.ReadBytes(stream, length));
        }

        private byte[] ReadBytes(Stream stream, int length)
        {
            var answer = new byte[length];

            stream.Read(answer, 0, length);

            return answer;
        }
    }

}
