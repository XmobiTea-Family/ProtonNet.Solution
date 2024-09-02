using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class UIntBinaryReader : BinaryReader<uint>
    {
        public UIntBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.UInt;

        public override uint Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 4);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

    }

}
