using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class FloatBinaryReader : BinaryReader<float>
    {
        public FloatBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Float;

        public override float Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 4);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToSingle(bytes, 0);
        }

    }

}
