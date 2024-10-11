using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class CollectionOfObjectBinaryReader : AbstractCollectionBinaryReader<ICollection>
    {
        public CollectionOfObjectBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.CollectionOfObject;

        public override ICollection Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            return this.ReadData(stream, length);
        }

        public ICollection ReadData(Stream stream, int length)
        {
            if (length < 0) return null;

            var answer = new object[length];

            for (var i = 0; i < length; i++)
            {
                var binaryTypeCode = this.ReadByte(stream);

                answer[i] = this.binaryDeserializer.GetReader(binaryTypeCode).Read(stream);
            }

            return answer;
        }

    }

}
