using System.Collections;
using System.Collections.Generic;
using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Deserialize.Reader
{
    class DictionaryKeyObjectValueObjectBinaryReader : AbstractCollectionBinaryReader<IDictionary>
    {
        public DictionaryKeyObjectValueObjectBinaryReader(IBinaryDeserializer binaryDeserializer) : base(binaryDeserializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.DictionaryKeyObjectValueObject;

        public override IDictionary Read(Stream stream)
        {
            var length = this.GetCollectionLength(stream);

            if (length < 0) return null;

            var collectionOfObjectBinaryReader = (CollectionOfObjectBinaryReader)this.binaryDeserializer.GetReader(BinaryTypeCode.CollectionOfObject);

            var answer = new Dictionary<object, object>();

            if (length != 0)
            {
                var keys = collectionOfObjectBinaryReader.ReadData(stream, length);
                var values = collectionOfObjectBinaryReader.ReadData(stream, length);

                var i = 0;
                var keyObjs = new object[length];
                foreach (var key in keys)
                    keyObjs[i++] = key;

                i = 0;
                var valueObjs = new object[length];
                foreach (var value in values)
                    valueObjs[i++] = value;

                for (i = 0; i < length; i++)
                    answer[keyObjs[i]] = valueObjs[i];
            }

            return answer;
        }

    }

}
