using System;
using System.Text;
using XmobiTea.ProtonNetCommon.Extensions;

namespace XmobiTea.ProtonNetCommon
{
    /// <summary>
    /// Interface defining the operations for a memory buffer.
    /// </summary>
    public interface IMemoryBuffer
    {
        /// <summary>
        /// Gets the capacity of the buffer.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Gets the byte array buffer.
        /// </summary>
        byte[] Buffer { get; }

        /// <summary>
        /// Gets the current length of the buffer.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets the current position in the buffer.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets the byte at the specified index.
        /// </summary>
        byte this[int index] { get; }

        /// <summary>
        /// Checks if the buffer is empty.
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Converts the buffer to a byte array.
        /// </summary>
        byte[] ToArray();

        /// <summary>
        /// Converts a portion of the buffer to a byte array.
        /// </summary>
        byte[] ToArray(int position, int length);

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        void Clear();

        /// <summary>
        /// Extracts a string from the buffer.
        /// </summary>
        string ExtractString(int position, int length);

        /// <summary>
        /// Removes a portion of the buffer.
        /// </summary>
        void Remove(int position, int length);

        /// <summary>
        /// Reserves additional capacity for the buffer.
        /// </summary>
        void Reserve(int capacity);

        /// <summary>
        /// Resizes the buffer to the specified length.
        /// </summary>
        void Resize(int length);

        /// <summary>
        /// Shifts the current position in the buffer.
        /// </summary>
        void Shift(int positionAmount);

        /// <summary>
        /// Writes a byte to the buffer.
        /// </summary>
        int Write(byte value);

        /// <summary>
        /// Writes a string to the buffer.
        /// </summary>
        int Write(string text);

        /// <summary>
        /// Writes a byte array to the buffer.
        /// </summary>
        int Write(byte[] buffer);

        /// <summary>
        /// Writes a portion of a byte array to the buffer.
        /// </summary>
        int Write(byte[] buffer, int position, int length);

    }

    /// <summary>
    /// Implementation of the IMemoryBuffer interface, providing methods to manage a byte buffer.
    /// </summary>
    public sealed class MemoryBuffer : IMemoryBuffer
    {
        /// <summary>
        /// Gets or sets the current buffer.
        /// </summary>
        private byte[] currentBuffer { get; set; }

        /// <summary>
        /// Gets or sets the current length of the buffer.
        /// </summary>
        private int currentLength { get; set; }

        /// <summary>
        /// Gets or sets the current position in the buffer.
        /// </summary>
        private int currentPosition { get; set; }

        /// <summary>
        /// Gets the capacity of the current buffer.
        /// </summary>
        public int Capacity => this.currentBuffer.Length;

        /// <summary>
        /// Gets the current buffer as a byte array.
        /// </summary>
        public byte[] Buffer => this.currentBuffer;

        /// <summary>
        /// Gets the current length of the buffer.
        /// </summary>
        public int Length => this.currentLength;

        /// <summary>
        /// Gets the current position in the buffer.
        /// </summary>
        public int Position => this.currentPosition;

        /// <summary>
        /// Gets the byte at the specified index in the buffer.
        /// </summary>
        public byte this[int index] => this.currentBuffer[index];

        /// <summary>
        /// Initializes a new instance of the MemoryBuffer class.
        /// </summary>
        public MemoryBuffer()
        {
            this.currentBuffer = new byte[0];
            this.currentLength = 0;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Initializes a new instance of the MemoryBuffer class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the buffer.</param>
        public MemoryBuffer(int capacity)
        {
            this.currentBuffer = new byte[capacity];
            this.currentLength = 0;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Initializes a new instance of the MemoryBuffer class with the specified byte array.
        /// </summary>
        /// <param name="buffer">The initial byte array.</param>
        public MemoryBuffer(byte[] buffer)
        {
            this.currentBuffer = buffer;
            this.currentLength = buffer.Length;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Checks if the buffer is empty.
        /// </summary>
        /// <returns>True if the buffer is empty; otherwise, false.</returns>
        public bool IsEmpty() => this.currentLength == 0;

        /// <summary>
        /// Converts the entire buffer to a byte array.
        /// </summary>
        /// <returns>A new byte array containing the contents of the buffer.</returns>
        public byte[] ToArray() => this.currentBuffer.ToClone(this.currentPosition, this.currentPosition + this.currentLength);

        /// <summary>
        /// Converts a portion of the buffer to a byte array.
        /// </summary>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the portion to convert.</param>
        /// <returns>A new byte array containing the specified portion of the buffer.</returns>
        public byte[] ToArray(int position, int length) => this.currentBuffer.ToClone(position, position + length);

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        public void Clear()
        {
            this.currentLength = 0;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Extracts a string from the buffer.
        /// </summary>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the string to extract.</param>
        /// <returns>The extracted string.</returns>
        /// <exception cref="ArgumentException">Thrown when the position and length are invalid.</exception>
        public string ExtractString(int position, int length)
        {
            if (position + length > this.Length)
                throw new ArgumentException("Invalid position & length!");

            return Encoding.UTF8.GetString(this.currentBuffer, position, length);
        }

        /// <summary>
        /// Removes a portion of the buffer.
        /// </summary>
        /// <param name="position">The starting position in the buffer.</param>
        /// <param name="length">The length of the portion to remove.</param>
        /// <exception cref="ArgumentException">Thrown when the position and length are invalid.</exception>
        public void Remove(int position, int length)
        {
            if (position + length > this.Length)
                throw new ArgumentException("Invalid position & length!");

            Array.Copy(this.currentBuffer, position + length, this.currentBuffer, position, this.currentLength - length - position);
            this.currentLength -= length;
            if (this.currentPosition >= (position + length))
                this.currentPosition -= length;
            else if (this.currentPosition >= position)
            {
                this.currentPosition -= this.currentPosition - position;
                if (this.currentPosition > this.Length)
                    this.currentPosition = this.Length;
            }
        }

        /// <summary>
        /// Reserves additional capacity for the buffer.
        /// </summary>
        /// <param name="capacity">The new capacity to reserve.</param>
        /// <exception cref="ArgumentException">Thrown when the capacity is less than 0.</exception>
        public void Reserve(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Capacity parameters must be greater than 0");

            if (capacity > this.Capacity)
            {
                var data = new byte[Math.Max(capacity, this.Capacity * 2)];
                Array.Copy(this.currentBuffer, 0, data, 0, this.currentLength);
                this.currentBuffer = data;
            }
        }

        /// <summary>
        /// Resizes the buffer to the specified length.
        /// </summary>
        /// <param name="length">The new length of the buffer.</param>
        public void Resize(int length)
        {
            this.Reserve(length);
            this.currentLength = length;
            if (this.currentPosition > this.currentLength)
                this.currentPosition = this.currentLength;
        }

        /// <summary>
        /// Shifts the current position in the buffer by the specified amount.
        /// </summary>
        /// <param name="positionAmount">The amount by which to shift the position.</param>
        public void Shift(int positionAmount) => this.currentPosition += positionAmount;

        /// <summary>
        /// Writes a byte to the buffer.
        /// </summary>
        /// <param name="value">The byte to write.</param>
        /// <returns>The number of bytes written (1).</returns>
        public int Write(byte value)
        {
            this.Reserve(this.currentLength + 1);
            this.currentBuffer[this.currentLength] = value;
            this.currentLength += 1;
            return 1;
        }

        /// <summary>
        /// Writes a string to the buffer.
        /// </summary>
        /// <param name="text">The string to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int Write(string text)
        {
            var length = Encoding.UTF8.GetMaxByteCount(text.Length);
            this.Reserve(this.currentLength + length);
            var result = Encoding.UTF8.GetBytes(text, 0, text.Length, this.currentBuffer, this.currentLength);
            this.currentLength += result;
            return result;
        }

        /// <summary>
        /// Writes a byte array to the buffer.
        /// </summary>
        /// <param name="buffer">The byte array to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int Write(byte[] buffer) => this.Write(buffer, 0, buffer.Length);

        /// <summary>
        /// Writes a portion of a byte array to the buffer.
        /// </summary>
        /// <param name="buffer">The byte array to write from.</param>
        /// <param name="position">The starting position in the byte array.</param>
        /// <param name="length">The number of bytes to write.</param>
        /// <returns>The number of bytes written.</returns>
        public int Write(byte[] buffer, int position, int length)
        {
            this.Reserve(this.currentLength + length);
            Array.Copy(buffer, position, this.currentBuffer, this.currentLength, length);
            this.currentLength += length;
            return length;
        }

    }

}
