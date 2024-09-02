using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class SByteBinaryReader : BinaryReader<sbyte>
    {
        public SByteBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.SByte;

        public override sbyte Read(Stream stream)
        {
            short s = this.ReadByte(stream);

            if (s > 127) s -= 256;

            return Convert.ToSByte(s);
        }

    }

}
