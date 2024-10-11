using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DictionaryKeyObjectValueObjectBinaryWriter : AbstractCollectionBinaryWriter<IDictionary>
    {
        public DictionaryKeyObjectValueObjectBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyObjectValueObject;

        public override int GetDataLength(IDictionary value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : value.Count);

            if (value != null)
            {
                var collectionOfObjectBinaryWriter = this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfObject);

                answer += collectionOfObjectBinaryWriter.GetDataLength(value.Keys) - 2;
                answer += collectionOfObjectBinaryWriter.GetDataLength(value.Values) - 2;
            }

            return answer;
        }

        public override void Write(Stream stream, IDictionary value)
        {
            this.WriteCollectionLength(stream, value == null ? -1 : value.Count);

            if (value != null)
            {
                var collectionOfObjectBinaryWriter = (CollectionOfObjectBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfObject);

                collectionOfObjectBinaryWriter.WriteData(stream, (ICollection)value.Keys);
                collectionOfObjectBinaryWriter.WriteData(stream, (ICollection)value.Values);
            }
        }

    }

}
