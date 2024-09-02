using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class NullBinaryWriter : BinaryWriter<object>
    {
        public NullBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Null;

        public override int GetDataLength(object value) => 0;

        public override void Write(Stream stream, object value)
        {
            // null no need write here
        }

    }

}
