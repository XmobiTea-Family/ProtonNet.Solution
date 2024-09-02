namespace XmobiTea.Binary.MessagePack.Types
{
    /// <summary>
    /// Represents the different type codes used in the MessagePack format.
    /// </summary>
    static class MessagePackTypeCode
    {
        /// <summary>
        /// Represents the type code for nil (null) values.
        /// </summary>
        public static readonly byte Nil = 0xc0;

        /// <summary>
        /// Represents a type code that is never used.
        /// </summary>
        public static readonly byte NeverUsed = 0xc1;

        /// <summary>
        /// Represents the type code for the boolean value true.
        /// </summary>
        public static readonly byte True = 0xc3;

        /// <summary>
        /// Represents the type code for the boolean value false.
        /// </summary>
        public static readonly byte False = 0xc2;

        /// <summary>
        /// Represents the type code for fixed negative integers.
        /// </summary>
        public static readonly byte NegativeIntFix = 0xE0;

        /// <summary>
        /// Represents the type code for fixed positive integers.
        /// </summary>
        public static readonly byte PositiveIntFix = 0x00;

        /// <summary>
        /// Represents the type code for unsigned 8-bit integers.
        /// </summary>
        public static readonly byte UInt8 = 0xcc;

        /// <summary>
        /// Represents the type code for unsigned 16-bit integers.
        /// </summary>
        public static readonly byte UInt16 = 0xcd;

        /// <summary>
        /// Represents the type code for unsigned 32-bit integers.
        /// </summary>
        public static readonly byte UInt32 = 0xce;

        /// <summary>
        /// Represents the type code for unsigned 64-bit integers.
        /// </summary>
        public static readonly byte UInt64 = 0xcf;

        /// <summary>
        /// Represents the type code for signed 8-bit integers.
        /// </summary>
        public static readonly byte Int8 = 0xd0;

        /// <summary>
        /// Represents the type code for signed 16-bit integers.
        /// </summary>
        public static readonly byte Int16 = 0xd1;

        /// <summary>
        /// Represents the type code for signed 32-bit integers.
        /// </summary>
        public static readonly byte Int32 = 0xd2;

        /// <summary>
        /// Represents the type code for signed 64-bit integers.
        /// </summary>
        public static readonly byte Int64 = 0xd3;

        /// <summary>
        /// Represents the type code for 32-bit floating-point numbers.
        /// </summary>
        public static readonly byte Float32 = 0xca;

        /// <summary>
        /// Represents the type code for 64-bit floating-point numbers.
        /// </summary>
        public static readonly byte Float64 = 0xcb;

        /// <summary>
        /// Represents the type code for fixed-length strings.
        /// </summary>
        public static readonly byte StrFix = 0xa0;

        /// <summary>
        /// Represents the type code for 8-bit length-prefixed strings.
        /// </summary>
        public static readonly byte Str8 = 0xd9;

        /// <summary>
        /// Represents the type code for 16-bit length-prefixed strings.
        /// </summary>
        public static readonly byte Str16 = 0xda;

        /// <summary>
        /// Represents the type code for 32-bit length-prefixed strings.
        /// </summary>
        public static readonly byte Str32 = 0xdb;

        /// <summary>
        /// Represents the type code for 8-bit length-prefixed binary data.
        /// </summary>
        public static readonly byte Bin8 = 0xc4;

        /// <summary>
        /// Represents the type code for 16-bit length-prefixed binary data.
        /// </summary>
        public static readonly byte Bin16 = 0xc5;

        /// <summary>
        /// Represents the type code for 32-bit length-prefixed binary data.
        /// </summary>
        public static readonly byte Bin32 = 0xc6;

        /// <summary>
        /// Represents the type code for fixed-length arrays.
        /// </summary>
        public static readonly byte ArrayFix = 0x90;

        /// <summary>
        /// Represents the type code for 16-bit length-prefixed arrays.
        /// </summary>
        public static readonly byte Array16 = 0xdc;

        /// <summary>
        /// Represents the type code for 32-bit length-prefixed arrays.
        /// </summary>
        public static readonly byte Array32 = 0xdd;

        /// <summary>
        /// Represents the type code for fixed-length maps.
        /// </summary>
        public static readonly byte MapFix = 0x80;

        /// <summary>
        /// Represents the type code for 16-bit length-prefixed maps.
        /// </summary>
        public static readonly byte Map16 = 0xde;

        /// <summary>
        /// Represents the type code for 32-bit length-prefixed maps.
        /// </summary>
        public static readonly byte Map32 = 0xdf;

        /// <summary>
        /// Represents the type code for fixed-length extension types of 1 byte.
        /// </summary>
        public static readonly byte ExtFix1 = 0xd4;

        /// <summary>
        /// Represents the type code for fixed-length extension types of 2 bytes.
        /// </summary>
        public static readonly byte ExtFix2 = 0xd5;

        /// <summary>
        /// Represents the type code for fixed-length extension types of 4 bytes.
        /// </summary>
        public static readonly byte ExtFix4 = 0xd6;

        /// <summary>
        /// Represents the type code for fixed-length extension types of 8 bytes.
        /// </summary>
        public static readonly byte ExtFix8 = 0xd7;

        /// <summary>
        /// Represents the type code for fixed-length extension types of 16 bytes.
        /// </summary>
        public static readonly byte ExtFix16 = 0xd8;

        /// <summary>
        /// Represents the type code for 8-bit length-prefixed extension types.
        /// </summary>
        public static readonly byte Ext8 = 0xc7;

        /// <summary>
        /// Represents the type code for 16-bit length-prefixed extension types.
        /// </summary>
        public static readonly byte Ext16 = 0xc8;

        /// <summary>
        /// Represents the type code for 32-bit length-prefixed extension types.
        /// </summary>
        public static readonly byte Ext32 = 0xc9;

    }

}
