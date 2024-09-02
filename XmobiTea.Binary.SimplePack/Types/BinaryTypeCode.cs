namespace XmobiTea.Binary.SimplePack.Types
{
    static class BinaryTypeCode
    {
        public static readonly byte Null = 0;
        public static readonly byte Boolean = 1;
        public static readonly byte Char = 2;
        public static readonly byte SByte = 3;
        public static readonly byte Byte = 4;
        public static readonly byte Short = 5;
        public static readonly byte UShort = 6;
        public static readonly byte Int = 7;
        public static readonly byte UInt = 8;
        public static readonly byte Long = 9;
        public static readonly byte ULong = 10;
        public static readonly byte Float = 11;
        public static readonly byte Double = 12;
        public static readonly byte Decimal = 13;
        public static readonly byte DateTime = 14;
        public static readonly byte String = 15;

        public static readonly byte CollectionOfIdentify = 16;
        public static readonly byte CollectionOfObject = 17;
        public static readonly byte DictionaryKeyIdentifyValueIdentify = 20;
        public static readonly byte DictionaryKeyObjectValueIdentify = 21;
        public static readonly byte DictionaryKeyIdentifyValueObject = 22;
        public static readonly byte DictionaryKeyObjectValueObject = 23;

        public static readonly byte UnknownType = 31;

    }

}
