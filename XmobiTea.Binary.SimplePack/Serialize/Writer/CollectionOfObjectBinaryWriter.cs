using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class CollectionOfObjectBinaryWriter : AbstractCollectionBinaryWriter<IList>
    {
        public CollectionOfObjectBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.CollectionOfObject;

        public override int GetDataLength(IList value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : value.Count);

            if (value != null)
                foreach (var elementValue in value)
                    if (elementValue == null)
                        answer += 1;
                    else
                    {
                        var writer = this.binarySerializer.GetWriter(elementValue.GetType());
                        answer += writer.GetDataLength(elementValue);
                    }

            return answer;
        }

        public override void Write(Stream stream, IList value)
        {
            this.WriteCollectionLength(stream, value == null ? -1 : value.Count);

            this.WriteData(stream, value);
        }

        public void WriteData(Stream stream, IList value)
        {
            if (value != null)
                foreach (var elementValue in value)
                    if (elementValue == null)
                        this.WriteByte(stream, BinaryTypeCode.Null);
                    else
                    {
                        var writer = this.binarySerializer.GetWriter(elementValue.GetType());
                        this.WriteByte(stream, writer.GetBinaryTypeCode());

                        writer.Write(stream, elementValue);
                    }
        }

    }

}
