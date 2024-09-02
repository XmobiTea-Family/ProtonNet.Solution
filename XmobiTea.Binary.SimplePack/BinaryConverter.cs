using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize;
using XmobiTea.Binary.SimplePack.Serialize;

namespace XmobiTea.Binary.SimplePack
{
    public class BinaryConverter : IBinaryConverter
    {
        private IBinarySerializer serializer { get; }
        private IBinaryDeserializer deserializer { get; }

        public BinaryConverter()
        {
            this.serializer = new BinarySerializer();
            this.deserializer = new BinaryDeserializer();
        }

        public byte[] Serialize<T>(T tObj) => this.serializer.Serialize(tObj);

        public void Serialize<T>(Stream stream, T tObj) => this.serializer.Serialize(stream, tObj);

        public T Deserialize<T>(Stream stream) => this.deserializer.Deserialize<T>(stream);

        public T Deserialize<T>(byte[] data) => this.deserializer.Deserialize<T>(data);

        public bool TryParse<T>(Stream stream, out T tValue)
        {
            try
            {
                tValue = this.deserializer.Deserialize<T>(stream);
                return true;
            }
            catch
            {
                tValue = default;
                return false;
            }
        }

        public bool TryParse<T>(byte[] data, out T tValue)
        {
            try
            {
                tValue = this.deserializer.Deserialize<T>(data);
                return true;
            }
            catch
            {
                tValue = default;
                return false;
            }
        }

    }

}
