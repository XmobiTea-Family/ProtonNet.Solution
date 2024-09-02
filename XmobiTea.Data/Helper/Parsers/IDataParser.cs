using System.Collections.Generic;
using XmobiTea.Data.Helper.Types;

namespace XmobiTea.Data.Helper.Parsers
{
    /// <summary>
    /// Interface defining methods for parsing objects into various data types.
    /// </summary>
    interface IDataParser
    {
        /// <summary>
        /// Converts the given object to a boolean.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted boolean value.</returns>
        bool ToBool(object obj);

        /// <summary>
        /// Converts the given object to a byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted byte value.</returns>
        byte ToByte(object obj);

        /// <summary>
        /// Converts the given object to a char.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted char value.</returns>
        char ToChar(object obj);

        /// <summary>
        /// Converts the given object to a double.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        double ToDouble(object obj);

        /// <summary>
        /// Converts the given object to a float.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted float value.</returns>
        float ToFloat(object obj);

        /// <summary>
        /// Converts the given object to an int.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted int value.</returns>
        int ToInt(object obj);

        /// <summary>
        /// Converts the given object to a long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted long value.</returns>
        long ToLong(object obj);

        /// <summary>
        /// Converts the given object to a signed byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed byte value.</returns>
        sbyte ToSByte(object obj);

        /// <summary>
        /// Converts the given object to a short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted short value.</returns>
        short ToShort(object obj);

        /// <summary>
        /// Converts the given object to a string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted string value.</returns>
        string ToString(object obj);

        /// <summary>
        /// Converts the given object to an unsigned int.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned int value.</returns>
        uint ToUInt(object obj);

        /// <summary>
        /// Converts the given object to an unsigned long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned long value.</returns>
        ulong ToULong(object obj);

        /// <summary>
        /// Converts the given object to an unsigned short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned short value.</returns>
        ushort ToUShort(object obj);

    }

    /// <summary>
    /// Implementation of the IDataParser interface for converting objects to various data types.
    /// </summary>
    class DataParser : IDataParser
    {
        /// <summary>
        /// Dictionary mapping data value types to their respective data converters.
        /// </summary>
        private IDictionary<DataValueType, IDataConverter> valueTypeWithConverterDict { get; }

        /// <summary>
        /// Initializes a new instance of the DataParser class and populates the converter dictionary.
        /// </summary>
        public DataParser()
        {
            this.valueTypeWithConverterDict = new Dictionary<DataValueType, IDataConverter>();
            this.AddValueTypeWithConverters();
        }

        /// <summary>
        /// Populates the converter dictionary with the appropriate data converters.
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
        /// Retrieves the data converter for the specified data value type.
        /// </summary>
        /// <param name="valueType">The data value type.</param>
        /// <returns>The corresponding data converter.</returns>
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
