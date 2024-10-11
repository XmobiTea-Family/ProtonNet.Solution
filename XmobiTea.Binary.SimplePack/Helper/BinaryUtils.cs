using System;
using XmobiTea.Binary.SimplePack.Types;

namespace XmobiTea.Binary.SimplePack.Helper
{
    static class BinaryUtils
    {
        private static Type TypeOfDictionary { get; }
        private static Type TypeOfGenericDictionary { get; }
        private static Type TypeOfCollection { get; }
        private static Type TypeOfGenericCollection { get; }

        static BinaryUtils()
        {
            TypeOfDictionary = typeof(System.Collections.IDictionary);
            TypeOfGenericDictionary = typeof(System.Collections.Generic.IDictionary<,>);
            TypeOfCollection = typeof(System.Collections.ICollection);
            TypeOfGenericCollection = typeof(System.Collections.Generic.ICollection<>);
        }

        public static bool IsIdentifyBinaryTypeCode(byte binaryTypeCode)
        {
            return binaryTypeCode == BinaryTypeCode.Boolean
                || binaryTypeCode == BinaryTypeCode.Char
                || binaryTypeCode == BinaryTypeCode.SByte
                || binaryTypeCode == BinaryTypeCode.Byte
                || binaryTypeCode == BinaryTypeCode.Short
                || binaryTypeCode == BinaryTypeCode.UShort
                || binaryTypeCode == BinaryTypeCode.Int
                || binaryTypeCode == BinaryTypeCode.UInt
                || binaryTypeCode == BinaryTypeCode.Long
                || binaryTypeCode == BinaryTypeCode.ULong
                || binaryTypeCode == BinaryTypeCode.Float
                || binaryTypeCode == BinaryTypeCode.Double
                || binaryTypeCode == BinaryTypeCode.Decimal
                || binaryTypeCode == BinaryTypeCode.DateTime
                || binaryTypeCode == BinaryTypeCode.String;
        }

        public static bool IsBigEndian() => !BitConverter.IsLittleEndian;

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

        public static byte GetBinaryTypeCode(Type type)
        {
            var typeCode = Type.GetTypeCode(type);

            if (typeCode == TypeCode.Empty) return BinaryTypeCode.Null;

            if (typeCode == TypeCode.Object)
                if (TypeOfDictionary.IsAssignableFrom(type) || TypeOfGenericDictionary.IsAssignableFrom(type))
                {
                    if (type.IsGenericType)
                    {
                        var genericArguments = type.GetGenericArguments();

                        var keyType = genericArguments[0];
                        var valueType = genericArguments[1];

                        var keyTypeCode = GetBinaryTypeCode(keyType);
                        var valueTypeCode = GetBinaryTypeCode(valueType);

                        var isIdentifyKey = IsIdentifyBinaryTypeCode(keyTypeCode);
                        var isIdentifyValue = IsIdentifyBinaryTypeCode(valueTypeCode);

                        if (isIdentifyKey)
                        {
                            if (isIdentifyValue) return BinaryTypeCode.DictionaryKeyIdentifyValueIdentify;
                            return BinaryTypeCode.DictionaryKeyIdentifyValueObject;
                        }

                        if (isIdentifyValue) return BinaryTypeCode.DictionaryKeyObjectValueIdentify;
                    }

                    return BinaryTypeCode.DictionaryKeyObjectValueObject;
                }
                else if (TypeOfCollection.IsAssignableFrom(type) || TypeOfGenericCollection.IsAssignableFrom(type))
                {
                    var elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];
                    var binaryTypeCode = GetBinaryTypeCode(elementType);

                    if (IsIdentifyBinaryTypeCode(binaryTypeCode)) return BinaryTypeCode.CollectionOfIdentify;
                    return BinaryTypeCode.CollectionOfObject;
                }

            if (typeCode == TypeCode.DBNull) { }
            if (typeCode == TypeCode.Boolean) return BinaryTypeCode.Boolean;
            if (typeCode == TypeCode.Char) return BinaryTypeCode.Char;
            if (typeCode == TypeCode.SByte) return BinaryTypeCode.SByte;
            if (typeCode == TypeCode.Byte) return BinaryTypeCode.Byte;
            if (typeCode == TypeCode.Int16) return BinaryTypeCode.Short;
            if (typeCode == TypeCode.UInt16) return BinaryTypeCode.UShort;
            if (typeCode == TypeCode.Int32) return BinaryTypeCode.Int;
            if (typeCode == TypeCode.UInt32) return BinaryTypeCode.UInt;
            if (typeCode == TypeCode.Int64) return BinaryTypeCode.Long;
            if (typeCode == TypeCode.UInt64) return BinaryTypeCode.ULong;
            if (typeCode == TypeCode.Single) return BinaryTypeCode.Float;
            if (typeCode == TypeCode.Double) return BinaryTypeCode.Double;
            if (typeCode == TypeCode.Decimal) return BinaryTypeCode.Decimal;
            if (typeCode == TypeCode.DateTime) return BinaryTypeCode.DateTime;
            if (typeCode == TypeCode.String) return BinaryTypeCode.String;

            return BinaryTypeCode.UnknownType;
        }

    }

}
