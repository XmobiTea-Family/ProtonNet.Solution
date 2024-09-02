using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class IntBinaryWriter : BinaryWriter<int>
    {
        public IntBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Int;

        public override int GetDataLength(int value) => 4;

        public override void Write(Stream stream, int value)
        {
            var bytes = BitConverter.GetBytes(value);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            this.WriteBytes(stream, bytes);
        }

    }

}
