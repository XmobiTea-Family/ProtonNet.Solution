using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class BooleanBinaryReader : BinaryReader<bool>
    {
        public BooleanBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Boolean;

        public override bool Read(Stream stream) => Convert.ToBoolean(this.ReadByte(stream));

    }

}
