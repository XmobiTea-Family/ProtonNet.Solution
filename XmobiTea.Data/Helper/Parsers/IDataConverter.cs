namespace XmobiTea.Data.Helper.Parsers
{
    /// <summary>
    /// Interface for data conversion.
    /// </summary>
    interface IDataConverter
    {
        /// <summary>
        /// Converts the given object to a data object.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted data object.</returns>
        object ToData(object obj);

    }

    /// <summary>
    /// Generic interface for data conversion.
    /// </summary>
    /// <typeparam name="T">The type to which the object is converted.</typeparam>
    interface IDataConverter<T>
    {
        /// <summary>
        /// Converts the given object to a data object of type T.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted data object of type T.</returns>
        T ToData(object obj);

    }

    /// <summary>
    /// Converter class for converting various types to a boolean.
    /// </summary>
    class BoolDataConverter : IDataConverter<bool>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a boolean.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted boolean value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a boolean.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted boolean value.</returns>
        public bool ToData(object obj)
        {
            if (obj is bool objBool) return objBool;
            if (obj is char objChar) return objChar % 2 != 0;
            if (obj is sbyte objSByte) return objSByte % 2 != 0;
            if (obj is byte objByte) return objByte % 2 != 0;
            if (obj is short objShort) return objShort % 2 != 0;
            if (obj is ushort objUShort) return objUShort % 2 != 0;
            if (obj is int objInt) return objInt % 2 != 0;
            if (obj is uint objUInt) return objUInt % 2 != 0;
            if (obj is long objLong) return objLong % 2 != 0;
            if (obj is ulong objULong) return objULong % 2 != 0;
            if (obj is float objFloat) return objFloat % 2 != 0;
            if (obj is double objDouble) return objDouble % 2 != 0;
            if (obj is string objString) return bool.Parse(objString);

            return System.Convert.ToBoolean(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a char.
    /// </summary>
    class CharDataConverter : IDataConverter<char>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a char.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted char value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a char.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted char value.</returns>
        public char ToData(object obj)
        {
            if (obj is char objChar) return System.Convert.ToChar(objChar);
            if (obj is bool objBool) return System.Convert.ToChar(objBool);
            if (obj is sbyte objSByte) return System.Convert.ToChar(objSByte);
            if (obj is byte objByte) return System.Convert.ToChar(objByte);
            if (obj is short objShort) return System.Convert.ToChar(objShort);
            if (obj is ushort objUShort) return System.Convert.ToChar(objUShort);
            if (obj is int objInt) return System.Convert.ToChar(objInt);
            if (obj is uint objUInt) return System.Convert.ToChar(objUInt);
            if (obj is long objLong) return System.Convert.ToChar(objLong);
            if (obj is ulong objULong) return System.Convert.ToChar(objULong);
            if (obj is float objFloat) return System.Convert.ToChar(objFloat);
            if (obj is double objDouble) return System.Convert.ToChar(objDouble);
            if (obj is string objString) return System.Convert.ToChar(objString);

            return System.Convert.ToChar(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a signed byte.
    /// </summary>
    class SByteDataConverter : IDataConverter<sbyte>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a signed byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed byte value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a signed byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted signed byte value.</returns>
        public sbyte ToData(object obj)
        {
            if (obj is char objChar) return (sbyte)objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return (sbyte)objByte;
            if (obj is short objShort) return (sbyte)objShort;
            if (obj is ushort objUShort) return (sbyte)objUShort;
            if (obj is int objInt) return (sbyte)objInt;
            if (obj is uint objUInt) return (sbyte)objUInt;
            if (obj is long objLong) return (sbyte)objLong;
            if (obj is ulong objULong) return (sbyte)objULong;
            if (obj is float objFloat) return (sbyte)objFloat;
            if (obj is double objDouble) return (sbyte)objDouble;
            if (obj is bool objBool) return System.Convert.ToSByte(objBool);
            if (obj is string objString) return System.Convert.ToSByte(objString);

            return System.Convert.ToSByte(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a byte.
    /// </summary>
    class ByteDataConverter : IDataConverter<byte>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted byte value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a byte.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted byte value.</returns>
        public byte ToData(object obj)
        {
            if (obj is char objChar) return (byte)objChar;
            if (obj is sbyte objSByte) return (byte)objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return (byte)objShort;
            if (obj is ushort objUShort) return (byte)objUShort;
            if (obj is int objInt) return (byte)objInt;
            if (obj is uint objUInt) return (byte)objUInt;
            if (obj is long objLong) return (byte)objLong;
            if (obj is ulong objULong) return (byte)objULong;
            if (obj is float objFloat) return (byte)objFloat;
            if (obj is double objDouble) return (byte)objDouble;
            if (obj is bool objBool) return System.Convert.ToByte(objBool);
            if (obj is string objString) return System.Convert.ToByte(objString);

            return System.Convert.ToByte(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a short.
    /// </summary>
    class ShortDataConverter : IDataConverter<short>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted short value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted short value.</returns>
        public short ToData(object obj)
        {
            if (obj is char objChar) return (short)objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return objShort;
            if (obj is ushort objUShort) return (short)objUShort;
            if (obj is int objInt) return (short)objInt;
            if (obj is uint objUInt) return (short)objUInt;
            if (obj is long objLong) return (short)objLong;
            if (obj is ulong objULong) return (short)objULong;
            if (obj is float objFloat) return (short)objFloat;
            if (obj is double objDouble) return (short)objDouble;
            if (obj is bool objBool) return System.Convert.ToInt16(objBool);
            if (obj is string objString) return System.Convert.ToInt16(objString);

            return System.Convert.ToInt16(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to an unsigned short.
    /// </summary>
    class UShortDataConverter : IDataConverter<ushort>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to an unsigned short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned short value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to an unsigned short.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned short value.</returns>
        public ushort ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return (ushort)objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return (ushort)objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return (ushort)objInt;
            if (obj is uint objUInt) return (ushort)objUInt;
            if (obj is long objLong) return (ushort)objLong;
            if (obj is ulong objULong) return (ushort)objULong;
            if (obj is float objFloat) return (ushort)objFloat;
            if (obj is double objDouble) return (ushort)objDouble;
            if (obj is bool objBool) return System.Convert.ToUInt16(objBool);
            if (obj is string objString) return System.Convert.ToUInt16(objString);

            return System.Convert.ToUInt16(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to an integer.
    /// </summary>
    class IntDataConverter : IDataConverter<int>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to an integer.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted integer value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to an integer.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted integer value.</returns>
        public int ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return objInt;
            if (obj is uint objUInt) return (int)objUInt;
            if (obj is long objLong) return (int)objLong;
            if (obj is ulong objULong) return (int)objULong;
            if (obj is float objFloat) return (int)objFloat;
            if (obj is double objDouble) return (int)objDouble;
            if (obj is bool objBool) return System.Convert.ToInt32(objBool);
            if (obj is string objString) return System.Convert.ToInt32(objString);

            return System.Convert.ToInt32(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to an unsigned integer.
    /// </summary>
    class UIntDataConverter : IDataConverter<uint>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to an unsigned integer.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned integer value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to an unsigned integer.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned integer value.</returns>
        public uint ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return (uint)objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return (uint)objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return (uint)objInt;
            if (obj is uint objUInt) return objUInt;
            if (obj is long objLong) return (uint)objLong;
            if (obj is ulong objULong) return (uint)objULong;
            if (obj is float objFloat) return (uint)objFloat;
            if (obj is double objDouble) return (uint)objDouble;
            if (obj is bool objBool) return System.Convert.ToUInt32(objBool);
            if (obj is string objString) return System.Convert.ToUInt32(objString);

            return System.Convert.ToUInt32(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a long.
    /// </summary>
    class LongDataConverter : IDataConverter<long>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted long value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted long value.</returns>
        public long ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return objInt;
            if (obj is uint objUInt) return objUInt;
            if (obj is long objLong) return objLong;
            if (obj is ulong objULong) return (long)objULong;
            if (obj is float objFloat) return (long)objFloat;
            if (obj is double objDouble) return (long)objDouble;
            if (obj is bool objBool) return System.Convert.ToInt64(objBool);
            if (obj is string objString) return System.Convert.ToInt64(objString);

            return System.Convert.ToInt64(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to an unsigned long.
    /// </summary>
    class ULongDataConverter : IDataConverter<ulong>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to an unsigned long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned long value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to an unsigned long.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted unsigned long value.</returns>
        public ulong ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return (ulong)objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return (ulong)objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return (ulong)objInt;
            if (obj is uint objUInt) return objUInt;
            if (obj is long objLong) return (ulong)objLong;
            if (obj is ulong objULong) return objULong;
            if (obj is float objFloat) return (ulong)objFloat;
            if (obj is double objDouble) return (ulong)objDouble;
            if (obj is bool objBool) return System.Convert.ToUInt64(objBool);
            if (obj is string objString) return System.Convert.ToUInt64(objString);

            return System.Convert.ToUInt64(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a float.
    /// </summary>
    class FloatDataConverter : IDataConverter<float>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a float.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted float value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a float.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted float value.</returns>
        public float ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return objInt;
            if (obj is uint objUInt) return objUInt;
            if (obj is long objLong) return objLong;
            if (obj is ulong objULong) return objULong;
            if (obj is float objFloat) return objFloat;
            if (obj is double objDouble) return (float)objDouble;
            if (obj is bool objBool) return System.Convert.ToSingle(objBool);
            if (obj is string objString) return System.Convert.ToSingle(objString);

            return System.Convert.ToSingle(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a double.
    /// </summary>
    class DoubleDataConverter : IDataConverter<double>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a double.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a double.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted double value.</returns>
        public double ToData(object obj)
        {
            if (obj is char objChar) return objChar;
            if (obj is sbyte objSByte) return objSByte;
            if (obj is byte objByte) return objByte;
            if (obj is short objShort) return objShort;
            if (obj is ushort objUShort) return objUShort;
            if (obj is int objInt) return objInt;
            if (obj is uint objUInt) return objUInt;
            if (obj is long objLong) return objLong;
            if (obj is ulong objULong) return objULong;
            if (obj is float objFloat) return objFloat;
            if (obj is double objDouble) return objDouble;
            if (obj is bool objBool) return System.Convert.ToDouble(objBool);
            if (obj is string objString) return System.Convert.ToDouble(objString);

            return System.Convert.ToDouble(obj);
        }

    }

    /// <summary>
    /// Converter class for converting various types to a string.
    /// </summary>
    class StringDataConverter : IDataConverter<string>, IDataConverter
    {
        /// <summary>
        /// Converts the given object to a string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted string value.</returns>
        object IDataConverter.ToData(object obj) => this.ToData(obj);

        /// <summary>
        /// Converts the given object to a string.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The converted string value.</returns>
        public string ToData(object obj)
        {
            if (obj is bool objBool) return System.Convert.ToString(objBool);
            if (obj is sbyte objSByte) return System.Convert.ToString(objSByte);
            if (obj is byte objByte) return System.Convert.ToString(objByte);
            if (obj is short objShort) return System.Convert.ToString(objShort);
            if (obj is ushort objUShort) return System.Convert.ToString(objUShort);
            if (obj is int objInt) return System.Convert.ToString(objInt);
            if (obj is uint objUInt) return System.Convert.ToString(objUInt);
            if (obj is long objLong) return System.Convert.ToString(objLong);
            if (obj is ulong objULong) return System.Convert.ToString(objULong);
            if (obj is float objFloat) return System.Convert.ToString(objFloat);
            if (obj is double objDouble) return System.Convert.ToString(objDouble);
            if (obj is char objChar) return System.Convert.ToString(objChar);
            if (obj is string objString) return objString;

            return System.Convert.ToString(obj);
        }

    }
}
