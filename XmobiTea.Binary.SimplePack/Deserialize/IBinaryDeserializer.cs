using System;
using System.IO;
using XmobiTea.Binary.SimplePack.Deserialize.Reader.Core;

namespace XmobiTea.Binary.SimplePack.Deserialize
{
    interface IBinaryDeserializer
    {
        IBinaryReader GetReader(byte binaryTypeCode);
        IBinaryReader GetReader(Type type);
        T Deserialize<T>(Stream stream);
        T Deserialize<T>(byte[] data);

    }

}
