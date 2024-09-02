using System.IO;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Serialize.Writer
{
    class StringBinaryWriter : AbstractCollectionBinaryWriter<string>
    {
        public StringBinaryWriter(IBinarySerializer binarySerializer) : base(binarySerializer)
        {
        }

        public override byte GetBinaryTypeCode() => BinaryTypeCode.String;

        public override int GetDataLength(string value)
        {
            var answer = this.GetLengthByte(value == null ? -1 : System.Text.Encoding.UTF8.GetBytes(value).Length);

            return answer;
        }

        public override void Write(Stream stream, string value)
        {
            if (value == null)
                this.WriteCollectionLength(stream, -1);
            else
                if (value.Length == 0)
                this.WriteCollectionLength(stream, 0);
            else
            {
                var data = System.Text.Encoding.UTF8.GetBytes(value);

                this.WriteCollectionLength(stream, data.Length);
                this.WriteBytes(stream, data);
            }

        }

    }

}
