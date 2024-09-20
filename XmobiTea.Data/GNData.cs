namespace XmobiTea.Data
{
    /// <summary>
    /// Abstract class representing generic data operations.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to access data.</typeparam>
    public abstract class GNData<TKey> : IGNData
    {
        /// <summary>
        /// Creates data that can be used from the original data.
        /// </summary>
        /// <param name="value">The original data value.</param>
        /// <returns>A data object suitable for use, or the original value if no conversion is needed.</returns>
        protected object CreateUseDataFromOriginData(object value)
        {
            if (value == null) return null;

            if (value is System.Collections.IList list)
            {
                var answer = new GNArray.Builder().AddAll(list).Build();
                return answer;
            }

            if (value is System.Collections.IDictionary dict)
            {
                var answer = new GNHashtable.Builder().AddAll(dict).Build();
                return answer;
            }

            if (value is GNArray.Builder gnArrayBuilder)
                return gnArrayBuilder.Build();

            if (value is GNHashtable.Builder gnHashtableBuilder)
                return gnHashtableBuilder.Build();

            return value;
        }

        /// <summary>
        /// Converts the use data back into its original form.
        /// </summary>
        /// <param name="value">The data used in the application.</param>
        /// <returns>The original data object.</returns>
        protected object CreateDataFromUseData(object value)
        {
            if (value == null) return null;

            if (value is IGNData gnData)
            {
                return gnData.ToData();
            }

            return value;
        }

        /// <summary>
        /// Abstract method to get an object based on the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the object.</param>
        /// <returns>The object associated with the key.</returns>
        public abstract object GetObject(TKey k);

        /// <summary>
        /// Gets a signed byte value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The signed byte value.</returns>
        public sbyte GetSByte(TKey k, sbyte defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToSByte(obj);
        }

        /// <summary>
        /// Gets a byte value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The byte value.</returns>
        public byte GetByte(TKey k, byte defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToByte(obj);
        }

        /// <summary>
        /// Gets a short value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The short value.</returns>
        public short GetShort(TKey k, short defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToInt16(obj);
        }

        /// <summary>
        /// Gets an unsigned short value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The unsigned short value.</returns>
        public ushort GetUShort(TKey k, ushort defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToUInt16(obj);
        }

        /// <summary>
        /// Gets an int value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The int value.</returns>
        public int GetInt(TKey k, int defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToInt32(obj);
        }

        /// <summary>
        /// Gets an unsigned int value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The unsigned int value.</returns>
        public uint GetUInt(TKey k, uint defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToUInt32(obj);
        }

        /// <summary>
        /// Gets a float value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The float value.</returns>
        public float GetFloat(TKey k, float defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToSingle(obj);
        }

        /// <summary>
        /// Gets a long value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The long value.</returns>
        public long GetLong(TKey k, long defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToInt64(obj);
        }

        /// <summary>
        /// Gets an unsigned long value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The unsigned long value.</returns>
        public ulong GetULong(TKey k, ulong defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToUInt64(obj);
        }

        /// <summary>
        /// Gets a double value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The double value.</returns>
        public double GetDouble(TKey k, double defaultValue = 0)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToDouble(obj);
        }

        /// <summary>
        /// Gets a boolean value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The boolean value.</returns>
        public bool GetBool(TKey k, bool defaultValue = false)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Gets a string value associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the value.</param>
        /// <param name="defaultValue">The default value if the key does not exist.</param>
        /// <returns>The string value.</returns>
        public string GetString(TKey k, string defaultValue = null)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return Helper.Convert.ToString(obj);
        }

        /// <summary>
        /// Gets an object associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the object.</param>
        /// <param name="defaultValue">The default object if the key does not exist.</param>
        /// <returns>The object value.</returns>
        public object GetObject(TKey k, object defaultValue = null)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return obj;
        }

        /// <summary>
        /// Gets an array of a specified type associated with the provided key.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="k">The key to retrieve the array.</param>
        /// <param name="defaultValue">The default array if the key does not exist.</param>
        /// <returns>An array of the specified type.</returns>
        public T[] GetArray<T>(TKey k, T[] defaultValue = null)
        {
            var value0 = this.GetGNArray(k);
            if (value0 != null)
            {
                return value0.ToArray<T>();
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a list of a specified type associated with the provided key.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="k">The key to retrieve the list.</param>
        /// <param name="defaultValue">The default list if the key does not exist.</param>
        /// <returns>A list of the specified type.</returns>
        public System.Collections.Generic.IList<T> GetList<T>(TKey k, System.Collections.Generic.IList<T> defaultValue = null)
        {
            var value0 = this.GetGNArray(k);
            if (value0 != null)
            {
                return value0.ToList<T>();
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a dictionary of a specified type associated with the provided key.
        /// </summary>
        /// <typeparam name="TObject">The type of values in the dictionary.</typeparam>
        /// <param name="k">The key to retrieve the dictionary.</param>
        /// <param name="defaultValue">The default dictionary if the key does not exist.</param>
        /// <returns>A dictionary with string keys and values of the specified type.</returns>
        public System.Collections.Generic.IDictionary<string, TObject> GetDictionary<TObject>(TKey k, System.Collections.Generic.IDictionary<string, TObject> defaultValue = null)
        {
            var value0 = this.GetGNHashtable(k);
            if (value0 != null)
            {
                return value0.ToDictionary<TObject>();
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets a GNArray object associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the GNArray.</param>
        /// <param name="defaultValue">The default GNArray if the key does not exist.</param>
        /// <returns>The GNArray object.</returns>
        public GNArray GetGNArray(TKey k, GNArray defaultValue = null)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return (GNArray)obj;
        }

        /// <summary>
        /// Gets a GNHashtable object associated with the provided key.
        /// </summary>
        /// <param name="k">The key to retrieve the GNHashtable.</param>
        /// <param name="defaultValue">The default GNHashtable if the key does not exist.</param>
        /// <returns>The GNHashtable object.</returns>
        public GNHashtable GetGNHashtable(TKey k, GNHashtable defaultValue = null)
        {
            var obj = this.GetObject(k);
            if (obj == null) return defaultValue;
            return (GNHashtable)obj;
        }

        /// <summary>
        /// Custom method to retrieve an object based on the key and its type.
        /// </summary>
        /// <param name="k">The key to retrieve the object.</param>
        /// <returns>The object, converted to the appropriate type if necessary.</returns>
        protected object CustomGet(TKey k)
        {
            var obj = this.GetObject(k);
            if (obj == null) return null;

            var objType = obj.GetType();
            if (objType == typeof(sbyte)) return this.GetSByte(k);
            else if (objType == typeof(byte)) return this.GetByte(k);
            else if (objType == typeof(short)) return this.GetShort(k);
            else if (objType == typeof(ushort)) return this.GetUShort(k);
            else if (objType == typeof(int)) return this.GetInt(k);
            else if (objType == typeof(uint)) return this.GetUInt(k);
            else if (objType == typeof(float)) return this.GetFloat(k);
            else if (objType == typeof(long)) return this.GetLong(k);
            else if (objType == typeof(ulong)) return this.GetULong(k);
            else if (objType == typeof(double)) return this.GetDouble(k);
            else if (objType == typeof(bool)) return this.GetBool(k);
            else if (objType == typeof(string)) return this.GetString(k);
            else if (objType == typeof(GNArray) || objType == typeof(System.Collections.IList)) return this.GetGNArray(k);
            else if (objType == typeof(GNHashtable) || objType == typeof(System.Collections.IDictionary)) return this.GetGNHashtable(k);

            return obj;
        }

        /// <summary>
        /// Abstract method to convert the data to an object.
        /// </summary>
        /// <returns>The converted object.</returns>
        public abstract object ToData();

    }

}
