using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace XmobiTea.Collections.Generic
{
    /// <summary>
    /// Provides a thread-safe dictionary implementation that supports concurrent access to elements in a multi-threaded environment.
    /// </summary>
    public class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Manages the synchronization of read and write operations.
        /// </summary>
        private ReaderWriterLockSlim _lock { get; }

        /// <summary>
        /// Stores the underlying dictionary of key-value pairs.
        /// </summary>
        public IDictionary<TKey, TValue> originDict { get; }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value is to be accessed or modified.</param>
        public TValue this[TKey key]
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originDict[key];
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
            set
            {
                this._lock.EnterWriteLock();
                try
                {
                    this.originDict[key] = value;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets a collection containing the keys in the dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originDict.Keys.ToList();
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets a collection containing the values in the dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originDict.Values.ToList();
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the dictionary.
        /// </summary>
        public int Count
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originDict.Count;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the dictionary is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originDict.IsReadOnly;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            this._lock.EnterWriteLock();
            try
            {
                this.originDict.Add(key, value);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Adds the specified key-value pair to the dictionary.
        /// </summary>
        /// <param name="item">The key-value pair to add to the dictionary.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this._lock.EnterWriteLock();
            try
            {
                this.originDict.Add(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public void Clear()
        {
            this._lock.EnterWriteLock();
            try
            {
                this.originDict.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key-value pair.
        /// </summary>
        /// <param name="item">The key-value pair to locate in the dictionary.</param>
        /// <returns>True if the dictionary contains the specified key-value pair; otherwise, false.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            this._lock.EnterReadLock();
            try
            {
                return this.originDict.Contains(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <returns>True if the dictionary contains the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            this._lock.EnterReadLock();
            try
            {
                return this.originDict.ContainsKey(key);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies the elements of the dictionary to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the dictionary.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this._lock.EnterReadLock();
            try
            {
                this.originDict.CopyTo(array, arrayIndex);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the dictionary.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            this._lock.EnterReadLock();
            try
            {
                return this.originDict.ToList().GetEnumerator();
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Removes the value with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>True if the element is successfully found and removed; otherwise, false.</returns>
        public bool Remove(TKey key)
        {
            this._lock.EnterWriteLock();
            try
            {
                return this.originDict.Remove(key);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the specified key-value pair from the dictionary.
        /// </summary>
        /// <param name="item">The key-value pair to remove from the dictionary.</param>
        /// <returns>True if the key-value pair is successfully found and removed; otherwise, false.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            this._lock.EnterWriteLock();
            try
            {
                return this.originDict.Remove(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>True if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            this._lock.EnterReadLock();
            try
            {
                return this.originDict.TryGetValue(key, out value);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class.
        /// </summary>
        public ThreadSafeDictionary()
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
        public ThreadSafeDictionary(int capacity)
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class with the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys, or null to use the default equality comparer for the type of the key.</param>
        public ThreadSafeDictionary(IEqualityComparer<TKey> comparer)
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class by copying the elements from the specified dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary whose elements are copied to the new dictionary.</param>
        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class with the specified capacity and comparer.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys, or null to use the default equality comparer for the type of the key.</param>
        public ThreadSafeDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeDictionary{TKey, TValue}"/> class by copying the elements from the specified dictionary and using the specified comparer.
        /// </summary>
        /// <param name="dictionary">The dictionary whose elements are copied to the new dictionary.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys, or null to use the default equality comparer for the type of the key.</param>
        public ThreadSafeDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this._lock = new ReaderWriterLockSlim();
            this.originDict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Destructor to release resources.
        /// </summary>
        ~ThreadSafeDictionary()
        {
            this._lock.Dispose();
        }
    }
}
