using System.IO;
using XmobiTea.Binary.SimplePack.Serialize.Writer.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DecimalBinaryWriter : BinaryWriter<decimal>
    {
        public DecimalBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Decimal;

        public override int GetDataLength(decimal value)
        {
            var stringBinaryWriter = (StringBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.String);

            return stringBinaryWriter.GetDataLength(value.ToString());
        }

        public override void Write(Stream stream, decimal value)
        {
            var stringBinaryWriter = (StringBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.String);

            stringBinaryWriter.Write(stream, value.ToString());
        }

    }

}
