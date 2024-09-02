using System.Collections.Generic;
using XmobiTea.Binary.Helper.Types;

namespace XmobiTea.Binary.Helper.Parsers
{
    /// <summary>
    /// Defines methods for parsing various types of data.
    /// </summary>
    interface IDataParser
    {
        /// <summary>
        /// Converts an object to a Boolean value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The Boolean value represented by the object.</returns>
        bool ToBool(object obj);

        /// <summary>
        /// Converts an object to a byte value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The byte value represented by the object.</returns>
        byte ToByte(object obj);

        /// <summary>
        /// Converts an object to a character value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The character value represented by the object.</returns>
        char ToChar(object obj);

        /// <summary>
        /// Converts an object to a double value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The double value represented by the object.</returns>
        double ToDouble(object obj);

        /// <summary>
        /// Converts an object to a float value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The float value represented by the object.</returns>
        float ToFloat(object obj);

        /// <summary>
        /// Converts an object to an integer value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The integer value represented by the object.</returns>
        int ToInt(object obj);

        /// <summary>
        /// Converts an object to a long value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The long value represented by the object.</returns>
        long ToLong(object obj);

        /// <summary>
        /// Converts an object to an sbyte value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The sbyte value represented by the object.</returns>
        sbyte ToSByte(object obj);

        /// <summary>
        /// Converts an object to a short value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The short value represented by the object.</returns>
        short ToShort(object obj);

        /// <summary>
        /// Converts an object to a string value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The string value represented by the object.</returns>
        string ToString(object obj);

        /// <summary>
        /// Converts an object to a uint value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The uint value represented by the object.</returns>
        uint ToUInt(object obj);

        /// <summary>
        /// Converts an object to a ulong value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The ulong value represented by the object.</returns>
        ulong ToULong(object obj);

        /// <summary>
        /// Converts an object to a ushort value.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The ushort value represented by the object.</returns>
        ushort ToUShort(object obj);
    }

    /// <summary>
    /// Provides an implementation of the <see cref="IDataParser"/> interface for parsing data types.
    /// </summary>
    class DataParser : IDataParser
    {
        /// <summary>
        /// Gets the dictionary mapping data value types to their corresponding converters.
        /// </summary>
        private IDictionary<DataValueType, IDataConverter> valueTypeWithConverterDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataParser"/> class.
        /// </summary>
        public DataParser()
        {
            this.valueTypeWithConverterDict = new Dictionary<DataValueType, IDataConverter>();
            this.AddValueTypeWithConverters();
        }

        /// <summary>
        /// Adds the available data type converters to the dictionary.
        /// </summary>
        private void AddValueTypeWithConverters()
        {
            this.valueTypeWithConverterDict[DataValueType.Bool] = new BoolDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Byte] = new ByteDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Char] = new CharDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Double] = new DoubleDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Float] = new FloatDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Int] = new IntDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Long] = new LongDataConverter();
            this.valueTypeWithConverterDict[DataValueType.SByte] = new SByteDataConverter();
            this.valueTypeWithConverterDict[DataValueType.Short] = new ShortDataConverter();
            this.valueTypeWithConverterDict[DataValueType.String] = new StringDataConverter();
            this.valueTypeWithConverterDict[DataValueType.UInt] = new UIntDataConverter();
            this.valueTypeWithConverterDict[DataValueType.ULong] = new ULongDataConverter();
            this.valueTypeWithConverterDict[DataValueType.UShort] = new UShortDataConverter();
        }

        /// <summary>
        /// Gets the data converter for the specified value type.
        /// </summary>
        /// <param name="valueType">The type of the data to be converted.</param>
        /// <returns>The <see cref="IDataConverter"/> associated with the specified value type.</returns>
        private IDataConverter GetDataConverter(DataValueType valueType) => this.valueTypeWithConverterDict[valueType];

        public bool ToBool(object obj) => obj == null ? default : (bool)this.GetDataConverter(DataValueType.Bool).ToData(obj);

        public byte ToByte(object obj) => obj == null ? default : (byte)this.GetDataConverter(DataValueType.Byte).ToData(obj);

        public char ToChar(object obj) => obj == null ? default : (char)this.GetDataConverter(DataValueType.Char).ToData(obj);

        public double ToDouble(object obj) => obj == null ? default : (double)this.GetDataConverter(DataValueType.Double).ToData(obj);

        public float ToFloat(object obj) => obj == null ? default : (float)this.GetDataConverter(DataValueType.Float).ToData(obj);

        public int ToInt(object obj) => obj == null ? default : (int)this.GetDataConverter(DataValueType.Int).ToData(obj);

        public long ToLong(object obj) => obj == null ? default : (long)this.GetDataConverter(DataValueType.Long).ToData(obj);

        public sbyte ToSByte(object obj) => obj == null ? default : (sbyte)this.GetDataConverter(DataValueType.SByte).ToData(obj);

        public short ToShort(object obj) => obj == null ? default : (short)this.GetDataConverter(DataValueType.Short).ToData(obj);

        public string ToString(object obj) => obj == null ? default : (string)this.GetDataConverter(DataValueType.String).ToData(obj);

        public uint ToUInt(object obj) => obj == null ? default : (uint)this.GetDataConverter(DataValueType.UInt).ToData(obj);

        public ulong ToULong(object obj) => obj == null ? default : (ulong)this.GetDataConverter(DataValueType.ULong).ToData(obj);

        public ushort ToUShort(object obj) => obj == null ? default : (ushort)this.GetDataConverter(DataValueType.UShort).ToData(obj);

    }

}
