using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class CharBinaryWriter : BinaryWriter<char>
    {
        public CharBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Char;

        public override int GetDataLength(char value) => 1;

        public override void Write(Stream stream, char value) => this.WriteByte(stream, Convert.ToByte(value));

    }

}
