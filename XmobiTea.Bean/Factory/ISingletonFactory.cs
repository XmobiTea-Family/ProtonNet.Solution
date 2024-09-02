namespace XmobiTea.Bean.Factory
{
    /// <summary>
    /// Defines the interface for a singleton factory.
    /// </summary>
    interface ISingletonFactory
    {
        /// <summary>Creates a singleton instance of the specified type.</summary>
        /// <param name="type">The type of the singleton to create.</param>
        /// <param name="args">The arguments to pass to the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        object CreateSingleton(System.Type type, params object[] args);

        /// <summary>Gets the singleton instance of the specified type.</summary>
        /// <param name="type">The type of the singleton to retrieve.</param>
        /// <returns>The singleton instance of the specified type.</returns>
        object GetSingleton(System.Type type);

        /// <summary>Sets the singleton instance for the specified type.</summary>
        /// <param name="type">The type of the singleton to set.</param>
        /// <param name="singletonObj">The singleton instance to set.</param>
        void SetSingleton(System.Type type, object singletonObj);

        /// <summary>Gets all singleton instances managed by this factory.</summary>
        /// <returns>An enumerable collection of singleton instances.</returns>
        System.Collections.Generic.IEnumerable<object> GetSingletons();
    }

    /// <summary>
    /// A factory class for creating and managing singleton instances.
    /// </summary>
    class SingletonFactory : ISingletonFactory
    {
        /// <summary>
        /// Gets the Type object for the base object class.
        /// </summary>
        private static System.Type typeObject { get; }

        /// <summary>
        /// Static constructor to initialize the <see cref="SingletonFactory"/> class.
        /// </summary>
        static SingletonFactory()
        {
            typeObject = typeof(object);
        }

        /// <summary>
        /// A dictionary to store singleton instances mapped by their types.
        /// </summary>
        private System.Collections.Generic.IDictionary<System.Type, object> singletonDict { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonFactory"/> class.
        /// </summary>
        public SingletonFactory()
        {
            this.singletonDict = new System.Collections.Generic.Dictionary<System.Type, object>();
        }

        /// <summary>
        /// Gets the singleton instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the singleton to retrieve.</param>
        /// <returns>The singleton instance of the specified type.</returns>
        public object GetSingleton(System.Type type)
        {
            if (this.singletonDict.TryGetValue(type, out var singletonObj))
                return singletonObj;

            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface != null && @interface != typeObject)
                {
                    var baseSingletonObj = this.GetSingleton(@interface);
                    if (baseSingletonObj != null) return baseSingletonObj;
                }
            }

            var baseType = type.BaseType;
            if (baseType != null && baseType != typeObject)
            {
                var baseSingletonObj = this.GetSingleton(baseType);
                if (baseSingletonObj != null) return baseSingletonObj;
            }

            return null;
        }

        /// <summary>
        /// Creates a singleton instance of the specified type.
        /// </summary>
        /// <param name="type">The type of the singleton to create.</param>
        /// <param name="args">The arguments to pass to the constructor.</param>
        /// <returns>The created singleton instance.</returns>
        public object CreateSingleton(System.Type type, params object[] args)
        {
            if (this.singletonDict.ContainsKey(type))
                throw new System.Exception("Cannot create singleton, type " + type + " already has a singleton.");

            var singletonObj = System.Activator.CreateInstance(type, args);
            this.SetSingleton(type, singletonObj);

            return singletonObj;
        }

        /// <summary>
        /// Sets the singleton instance for the specified type.
        /// </summary>
        /// <param name="type">The type of the singleton to set.</param>
        /// <param name="singletonObj">The singleton instance to set.</param>
        public void SetSingleton(System.Type type, object singletonObj)
        {
            this.singletonDict[type] = singletonObj;

            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface != null && @interface != typeObject)
                {
                    this.SetSingleton(@interface, singletonObj);
                }
            }

            var baseType = type.BaseType;
            if (baseType != null && baseType != typeObject)
            {
                this.SetSingleton(baseType, singletonObj);
            }
        }

        /// <summary>
        /// Gets all singleton instances managed by this factory.
        /// </summary>
        /// <returns>An enumerable collection of singleton instances.</returns>
        public System.Collections.Generic.IEnumerable<object> GetSingletons() => this.singletonDict.Values;

    }

}
