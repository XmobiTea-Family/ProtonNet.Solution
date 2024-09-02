using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class CharBinaryReader : BinaryReader<char>
    {
        public CharBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Char;

        public override char Read(Stream stream) => Convert.ToChar(this.ReadByte(stream));

    }

}
