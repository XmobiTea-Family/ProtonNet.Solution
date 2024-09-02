using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class ShortBinaryReader : BinaryReader<short>
    {
        public ShortBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Short;

        public override short Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 2);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToInt16(bytes, 0);
        }

    }

}
