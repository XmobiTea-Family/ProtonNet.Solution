using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class UShortBinaryReader : BinaryReader<ushort>
    {
        public UShortBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.UShort;

        public override ushort Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 2);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToUInt16(bytes, 0);
        }

    }

}
