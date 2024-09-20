using XmobiTea.Binary.MessagePack.Types;

namespace XmobiTea.Binary.MessagePack.Helper
{
    /// <summary>
    /// Provides utility methods for handling binary data, particularly for MessagePack serialization.
    /// </summary>
    static class BinaryUtils
    {
        /// <summary>
        /// Gets the <see cref="System.Type"/> representing non-generic dictionaries.
        /// </summary>
        private static System.Type TypeOfDictionary { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> representing generic dictionaries.
        /// </summary>
        private static System.Type TypeOfGenericDictionary { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> representing non-generic collections.
        /// </summary>
        private static System.Type TypeOfCollection { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> representing generic collections.
        /// </summary>
        private static System.Type TypeOfGenericCollection { get; }

        /// <summary>
        /// Initializes the static members of the <see cref="BinaryUtils"/> class.
        /// </summary>
        static BinaryUtils()
        {
            TypeOfDictionary = typeof(System.Collections.IDictionary);
            TypeOfGenericDictionary = typeof(System.Collections.Generic.IDictionary<,>);
            TypeOfCollection = typeof(System.Collections.ICollection);
            TypeOfGenericCollection = typeof(System.Collections.Generic.ICollection<>);
        }

        /// <summary>
        /// Determines whether the system is big-endian.
        /// </summary>
        /// <returns>True if the system is big-endian; otherwise, false.</returns>
        public static bool IsBigEndian() => !System.BitConverter.IsLittleEndian;

        /// <summary>
        /// Swaps the byte order of the given buffer if the system is little-endian.
        /// </summary>
        /// <param name="buffer">The byte array to potentially swap.</param>
        public static void SwapIfLittleEndian(ref byte[] buffer)
        {
            if (IsBigEndian()) return;

            for (int i = 0, k = buffer.Length - 1; i < k; ++i, --k)
            {
                var c = buffer[i];
                buffer[i] = buffer[k];
                buffer[k] = c;
            }
        }

        /// <summary>
        /// Gets the binary type code associated with a specified <see cref="System.Type"/>.
        /// </summary>
        /// <param name="type">The type to get the binary type code for.</param>
        /// <returns>The binary type code as a byte.</returns>
        public static byte GetBinaryTypeCode(System.Type type)
        {
            var typeCode = System.Type.GetTypeCode(type);

            if (typeCode == System.TypeCode.Empty) return BinaryTypeCode.Nil;

            if (typeCode == System.TypeCode.Object)
            {
                if (TypeOfDictionary.IsAssignableFrom(type) || TypeOfGenericDictionary.IsAssignableFrom(type))
                    return BinaryTypeCode.Map;
                else if (TypeOfCollection.IsAssignableFrom(type) || TypeOfGenericCollection.IsAssignableFrom(type))
                {
                    var elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];
                    if (elementType == typeof(byte)) return BinaryTypeCode.Binary;

                    return BinaryTypeCode.Array;
                }
            }

            if (typeCode == System.TypeCode.Boolean) return BinaryTypeCode.Boolean;
            if (typeCode == System.TypeCode.Char) return BinaryTypeCode.String;
            if (typeCode == System.TypeCode.SByte) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.Byte) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.Int16) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.UInt16) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.Int32) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.UInt32) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.Int64) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.UInt64) return BinaryTypeCode.Integer;
            if (typeCode == System.TypeCode.Single) return BinaryTypeCode.Float;
            if (typeCode == System.TypeCode.Double) return BinaryTypeCode.Float;
            if (typeCode == System.TypeCode.String) return BinaryTypeCode.String;

            return BinaryTypeCode.Extension;
        }

    }

}
