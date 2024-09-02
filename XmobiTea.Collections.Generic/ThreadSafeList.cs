using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace XmobiTea.Collections.Generic
{
    /// <summary>
    /// Provides a thread-safe list implementation that supports safe access to elements in a multi-threaded environment.
    /// </summary>
    public class ThreadSafeList<T> : IList<T>
    {
        /// <summary>
        /// Manages the synchronization of read and write operations.
        /// </summary>
        private ReaderWriterLockSlim _lock { get; }

        /// <summary>
        /// Stores the underlying list of elements.
        /// </summary>
        public List<T> _internalList { get; }

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
                    return this._internalList[index];
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
                    this._internalList[index] = value;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the list.
        /// </summary>
        public int Count
        {
            get
            {
                this._lock.EnterReadLock();
                try
                {
                    return this._internalList.Count;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the list is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The object to add to the list.</param>
        public void Add(T item)
        {
            this._lock.EnterWriteLock();
            try
            {
                this._internalList.Add(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            this._lock.EnterWriteLock();
            try
            {
                this._internalList.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the list contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>True if item is found in the list; otherwise, false.</returns>
        public bool Contains(T item)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._internalList.Contains(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Copies the elements of the list to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the list.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._lock.EnterReadLock();
            try
            {
                this._internalList.CopyTo(array, arrayIndex);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            this._lock.EnterReadLock();
            try
            {
                return new List<T>(this._internalList).GetEnumerator();
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Determines the index of a specific item in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._internalList.IndexOf(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Inserts an item to the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the list.</param>
        public void Insert(int index, T item)
        {
            this._lock.EnterWriteLock();
            try
            {
                this._internalList.Insert(index, item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the list.
        /// </summary>
        /// <param name="item">The object to remove from the list.</param>
        /// <returns>True if item was successfully removed from the list; otherwise, false.</returns>
        public bool Remove(T item)
        {
            this._lock.EnterWriteLock();
            try
            {
                return this._internalList.Remove(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes the list item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            this._lock.EnterWriteLock();
            try
            {
                this._internalList.RemoveAt(index);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire list.
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the conditions of the element to search for.</param>
        /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
        public T Find(Predicate<T> match)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._internalList.Find(match);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the conditions of the elements to search for.</param>
        /// <returns>A List<T> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty List<T>.</returns>
        public List<T> FindAll(Predicate<T> match)
        {
            this._lock.EnterReadLock();
            try
            {
                return this._internalList.FindAll(match);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeList{T}"/> class.
        /// </summary>
        public ThreadSafeList()
        {
            this._lock = new ReaderWriterLockSlim();
            this._internalList = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeList{T}"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the list can contain.</param>
        public ThreadSafeList(int capacity)
        {
            this._lock = new ReaderWriterLockSlim();
            this._internalList = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeList{T}"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        public ThreadSafeList(IEnumerable<T> collection)
        {
            this._lock = new ReaderWriterLockSlim();
            this._internalList = new List<T>(collection);
        }

        /// <summary>
        /// Destructor to release resources.
        /// </summary>
        ~ThreadSafeList()
        {
            this._lock.Dispose();
        }
    }
}
