using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class ByteBinaryWriter : BinaryWriter<byte>
    {
        public ByteBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Byte;

        public override int GetDataLength(byte value) => 1;

        public override void Write(Stream stream, byte value) => this.WriteByte(stream, value);

    }

}
