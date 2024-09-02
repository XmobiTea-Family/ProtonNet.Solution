using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class SByteBinaryWriter : BinaryWriter<sbyte>
    {
        public SByteBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.SByte;

        public override int GetDataLength(sbyte value) => 1;

        public override void Write(Stream stream, sbyte value) => this.WriteByte(stream, (byte)(value < 0 ? value + 256 : value));

    }

}
