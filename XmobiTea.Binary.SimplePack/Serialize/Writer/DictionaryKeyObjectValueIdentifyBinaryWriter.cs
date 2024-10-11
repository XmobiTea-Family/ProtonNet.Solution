using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DictionaryKeyObjectValueIdentifyBinaryWriter : AbstractCollectionBinaryWriter<IDictionary>
    {
        public DictionaryKeyObjectValueIdentifyBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyObjectValueIdentify;

        public override int GetDataLength(IDictionary value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : value.Count);

            if (value != null)
            {
                answer += this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfObject).GetDataLength(value.Keys) - 2;
                answer += this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfIdentify).GetDataLength(value.Values) - 2;
            }

            return answer;
        }

        public override void Write(Stream stream, IDictionary value)
        {
            this.WriteCollectionLength(stream, value == null ? -1 : value.Count);

            if (value != null)
            {
                var genericArguments = value.GetType().GetGenericArguments();

                var valueBinaryTypeCode = BinaryUtils.GetBinaryTypeCode(genericArguments[1]);

                var collectionOfIdentifyBinaryWriter = (CollectionOfIdentifyBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfIdentify);
                var collectionOfObjectBinaryWriter = (CollectionOfObjectBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfObject);

                collectionOfObjectBinaryWriter.WriteData(stream, (ICollection)value.Keys);
                collectionOfIdentifyBinaryWriter.WriteData(stream, (ICollection)value.Values, valueBinaryTypeCode);
            }
        }

    }

}
