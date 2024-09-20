using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class CollectionOfIdentifyBinaryWriter : AbstractCollectionBinaryWriter<ICollection>
    {
        public CollectionOfIdentifyBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.CollectionOfIdentify;

        public override int GetDataLength(ICollection value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : value.Count);
            answer += 1;

            if (value != null)
            {
                var type = value.GetType();
                var writer = this.binarySerializer.GetWriter(type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0]);

                foreach (var elementValue in value)
                    if (elementValue == null)
                        answer += 1;
                    else
                        answer += writer.GetDataLength(elementValue);
            }

            return answer;
        }

        public override void Write(Stream stream, ICollection value)
        {
            this.WriteCollectionLength(stream, value == null ? -1 : value.Count);

            if (value == null) this.WriteData(stream, value, BinaryTypeCode.Null);
            else
            {
                var type = value.GetType();
                this.WriteData(stream, value, BinaryUtils.GetBinaryTypeCode(type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0]));
            }
        }

        public void WriteData(Stream stream, ICollection value, byte elementBinaryTypeCode)
        {
            var writer = this.binarySerializer.GetWriter(elementBinaryTypeCode);
            this.WriteByte(stream, writer.GetBinaryTypeCode());

            if (value != null)
                foreach (var elementValue in value)
                    writer.Write(stream, elementValue);
        }

    }

}
