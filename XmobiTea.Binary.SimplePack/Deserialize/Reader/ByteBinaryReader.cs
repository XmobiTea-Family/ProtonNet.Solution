using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class ByteBinaryReader : BinaryReader<byte>
    {
        public ByteBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Byte;

        public override byte Read(Stream stream) => this.ReadByte(stream);

    }

}
