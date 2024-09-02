using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class BooleanBinaryWriter : BinaryWriter<bool>
    {
        public BooleanBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Boolean;

        public override int GetDataLength(bool value) => 1;

        public override void Write(Stream stream, bool value) => this.WriteByte(stream, Convert.ToByte(value));

    }

}
