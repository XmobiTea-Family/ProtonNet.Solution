using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DoubleBinaryWriter : BinaryWriter<double>
    {
        public DoubleBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Double;

        public override int GetDataLength(double value) => 8;

        public override void Write(Stream stream, double value)
        {
            var bytes = BitConverter.GetBytes(value);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            this.WriteBytes(stream, bytes);
        }

    }

}
