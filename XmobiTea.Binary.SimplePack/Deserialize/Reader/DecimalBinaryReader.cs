using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DecimalBinaryReader : BinaryReader<decimal>
    {
        public DecimalBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.Decimal;

        public override decimal Read(Stream stream)
        {
            var stringBinaryReader = (StringBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.String);

            return Convert.ToDecimal(stringBinaryReader.Read(stream));
        }

    }

}
