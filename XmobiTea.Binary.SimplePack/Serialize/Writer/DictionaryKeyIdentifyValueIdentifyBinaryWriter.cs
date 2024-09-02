using System.Collections;
using System.IO;
using XmobiTea.Binary.SimplePack.Helper;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class DictionaryKeyIdentifyValueIdentifyBinaryWriter : AbstractCollectionBinaryWriter<IDictionary>
    {
        public DictionaryKeyIdentifyValueIdentifyBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyIdentifyValueIdentify;

        public override int GetDataLength(IDictionary value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : value.Count);

            if (value != null)
            {
                var collectionOfIdentifyBinaryWriter = this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfIdentify);

                answer += collectionOfIdentifyBinaryWriter.GetDataLength(value.Keys) - 2;
                answer += collectionOfIdentifyBinaryWriter.GetDataLength(value.Values) - 2;
            }

            return answer;
        }

        public override void Write(Stream stream, IDictionary value)
        {
            this.WriteCollectionLength(stream, value == null ? -1 : value.Count);

            if (value != null)
            {
                var genericArguments = value.GetType().GetGenericArguments();

                var keyBinaryTypeCode = BinaryUtils.GetBinaryTypeCode(genericArguments[0]);
                var valueBinaryTypeCode = BinaryUtils.GetBinaryTypeCode(genericArguments[1]);

                var collectionOfIdentifyBinaryWriter = (CollectionOfIdentifyBinaryWriter)this.binarySerializer.GetWriter(BinaryTypeCode.CollectionOfIdentify);

                collectionOfIdentifyBinaryWriter.WriteData(stream, (IList)value.Keys, keyBinaryTypeCode);
                collectionOfIdentifyBinaryWriter.WriteData(stream, (IList)value.Values, valueBinaryTypeCode);
            }
        }

    }

}
