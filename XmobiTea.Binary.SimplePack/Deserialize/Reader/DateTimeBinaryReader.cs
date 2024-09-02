using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DateTimeBinaryReader : BinaryReader<DateTime>
    {
        public DateTimeBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DateTime;

        public override DateTime Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 8);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return new DateTime(BitConverter.ToInt64(bytes, 0));
        }

    }

}
