using System;
using System.Collections.Concurrent;

namespace XmobiTea.ProtonNet.Server.Services
{
    /// <summary>
    /// Interface that defines methods for renting and returning byte arrays 
    /// based on a given buffer, position, and count.
    /// </summary>
    public interface IByteArrayManagerService
    {
        /// <summary>
        /// Rents a byte array from the pool or creates a new one if necessary.
        /// Copies data from the given buffer starting at the specified position.
        /// </summary>
        /// <param name="buffer">The source buffer from which to copy data.</param>
        /// <param name="position">The starting position in the source buffer.</param>
        /// <param name="length">The number of bytes to copy.</param>
        /// <returns>A byte array containing the copied data.</returns>
        byte[] Rent(byte[] buffer, int position, int length);

        /// <summary>
        /// Returns a byte array to the pool for reuse.
        /// </summary>
        /// <param name="buffer">The byte array to be returned to the pool.</param>
        void Return(byte[] buffer);

    }

    /// <summary>
    /// Service that manages byte array pooling by renting and returning byte arrays.
    /// Implements the IByteArrayManagerService interface.
    /// </summary>
    public class ByteArrayManagerService : IByteArrayManagerService
    {
        /// <summary>
        /// Concurrent dictionary that maps the size of byte arrays to their corresponding stacks for pooling.
        /// </summary>
        private ConcurrentDictionary<int, ConcurrentStack<byte[]>> _pools { get; }

        /// <summary>
        /// Initializes a new instance of the ByteArrayManagerService class and 
        /// sets up the pool storage.
        /// </summary>
        public ByteArrayManagerService() => this._pools = new ConcurrentDictionary<int, ConcurrentStack<byte[]>>();

        /// <summary>
        /// Rents a byte array from the pool with a size based on the length - position 
        /// of the source buffer. If no matching array is available in the pool, a new one is created.
        /// The content of the source buffer is copied to the rented array.
        /// </summary>
        /// <param name="buffer">The source buffer to copy data from.</param>
        /// <param name="position">The starting index in the source buffer.</param>
        /// <param name="length">The length of the data to copy from the source buffer.</param>
        /// <returns>A rented byte array with the copied data.</returns>
        public byte[] Rent(byte[] buffer, int position, int length)
        {
            var size = length - position;

            byte[] answer;

            if (!this._pools.TryGetValue(size, out ConcurrentStack<byte[]> pool))
                answer = new byte[size];
            else
            {
                if (!pool.TryPop(out answer))
                    answer = new byte[size];
            }

            Array.Copy(buffer, position, answer, 0, length);

            return answer;
        }

        /// <summary>
        /// Returns the specified byte array to the pool for reuse. 
        /// The pool is based on the size of the array being returned.
        /// </summary>
        /// <param name="buffer">The byte array to be returned to the pool.</param>
        public void Return(byte[] buffer)
        {
            var pool = this._pools.GetOrAdd(buffer.Length, _ => new ConcurrentStack<byte[]>());

            pool.Push(buffer);
        }

    }

}
