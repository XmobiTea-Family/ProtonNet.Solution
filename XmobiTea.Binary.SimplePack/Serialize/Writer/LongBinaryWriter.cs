using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class LongBinaryWriter : BinaryWriter<long>
    {
        public LongBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Long;

        public override int GetDataLength(long value) => 8;

        public override void Write(Stream stream, long value)
        {
            var bytes = BitConverter.GetBytes(value);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            this.WriteBytes(stream, bytes);
        }

    }

}
