using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class ULongBinaryWriter : BinaryWriter<ulong>
    {
        public ULongBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.ULong;

        public override int GetDataLength(ulong value) => 8;

        public override void Write(Stream stream, ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            this.WriteBytes(stream, bytes);
        }

    }

}
