using System;
using System.Collections.Generic;
using System.Threading;

namespace XmobiTea.Collections.Generic
{
    /// <summary>
    /// Provides a thread-safe array implementation that supports safe access to elements in a multi-threaded environment.
    /// </summary>
    public class ThreadSafeArray<T>
    {
        /// <summary>
        /// Stores the underlying array of elements.
        /// </summary>
        private T[] originArray { get; }

        /// <summary>
        /// Manages the synchronization of read and write operations.
        /// </summary>
        private ReaderWriterLockSlim _lock { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeArray{T}"/> class with the specified length.
        /// </summary>
        /// <param name="length">The length of the array to be initialized.</param>
        public ThreadSafeArray(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

            this._lock = new ReaderWriterLockSlim();
            this.originArray = new T[length];
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        public T this[int index]
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this.originArray[index];
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
                    this.originArray[index] = value;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the length of the array.
        /// </summary>
        public int Length => this.originArray.Length;

        /// <summary>
        /// Returns an enumerator that iterates through the elements of the array.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the array.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            this._lock.EnterReadLock();
            try
            {
                var snapshot = new T[this.originArray.Length];
                Array.Copy(this.originArray, snapshot, this.originArray.Length);
                return ((IEnumerable<T>)snapshot).GetEnumerator();
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Destructor to release resources.
        /// </summary>
        ~ThreadSafeArray()
        {
            this._lock.Dispose();
        }

    }

}
