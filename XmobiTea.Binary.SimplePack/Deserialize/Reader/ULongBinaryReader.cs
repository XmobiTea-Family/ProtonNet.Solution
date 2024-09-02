using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class ULongBinaryReader : BinaryReader<ulong>
    {
        public ULongBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.ULong;

        public override ulong Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 8);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

    }

}
