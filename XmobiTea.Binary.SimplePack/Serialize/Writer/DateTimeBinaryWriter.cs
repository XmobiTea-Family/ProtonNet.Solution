using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DateTimeBinaryWriter : BinaryWriter<DateTime>
    {
        public DateTimeBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DateTime;

        public override int GetDataLength(DateTime value) => 8;

        public override void Write(Stream stream, DateTime value)
        {
            var bytes = BitConverter.GetBytes(value.Ticks);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            this.WriteBytes(stream, bytes);
        }

    }

}
