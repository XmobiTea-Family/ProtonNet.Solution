using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DoubleBinaryReader : BinaryReader<double>
    {
        public DoubleBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Double;

        public override double Read(Stream stream)
        {
            var bytes = this.ReadBytes(stream, 8);
            BinaryUtils.SwapIfLittleEndian(ref bytes);

            return BitConverter.ToDouble(bytes, 0);
        }

    }

}
