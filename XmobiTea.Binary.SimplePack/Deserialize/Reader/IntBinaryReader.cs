using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class IntBinaryReader : BinaryReader<int>
    {
        public IntBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Int;

        public override int Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 4);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

    }

}
