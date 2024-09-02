using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class LongBinaryReader : BinaryReader<long>
    {
        public LongBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Long;

        public override long Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 8);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToInt64(bytes, 0);
        }

    }

}
