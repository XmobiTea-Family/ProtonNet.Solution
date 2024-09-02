using System.IO;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer.Core
{
    interface IBinaryWriter
    {
        byte GetBinaryTypeCode();
        int GetDataLength(object value);
        void Write(Stream stream, object value);

    }

    interface IBinaryWriter<T>
    {
        int GetDataLength(T value);
        void Write(Stream stream, T value);

    }

    abstract class BinaryWriter<T> : IBinaryWriter<T>, IBinaryWriter
    {
        protected IBinarySerializer binarySerializer { get; }

        public BinaryWriter(IBinarySerializer binarySerializer)
        {
            this.binarySerializer = binarySerializer;
        }

        public abstract byte GetBinaryTypeCode();
        public abstract int GetDataLength(T value);
        public abstract void Write(Stream stream, T value);

        int IBinaryWriter.GetDataLength(object value) => this.GetDataLength((T)value);

        void IBinaryWriter.Write(Stream stream, object value) => this.Write(stream, (T)value);

        protected void WriteBytes(Stream stream, byte[] data) => stream.Write(data, 0, data.Length);

        protected void WriteByte(Stream stream, byte data) => stream.WriteByte(data);

    }

}
