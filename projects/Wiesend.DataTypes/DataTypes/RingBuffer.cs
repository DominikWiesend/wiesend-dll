#region Project Description [About this]
// =================================================================================
//            The whole Project is Licensed under the MIT License
// =================================================================================
// =================================================================================
//    Wiesend's Dynamic Link Library is a collection of reusable code that 
//    I've written, or found throughout my programming career. 
//
//    I tried my very best to mention all of the original copyright holders. 
//    I hope that all code which I've copied from others is mentioned and all 
//    their copyrights are given. The copied code (or code snippets) could 
//    already be modified by me or others.
// =================================================================================
#endregion of Project Description
#region Original Source Code [Links to all original source code]
// =================================================================================
//          Original Source Code [Links to all original source code]
// =================================================================================
// https://github.com/JaCraig/Craig-s-Utility-Library
// =================================================================================
//    I didn't wrote this source totally on my own, this class could be nearly a 
//    clone of the project of James Craig, I did highly get inspired by him, so 
//    this piece of code isn't totally mine, shame on me, I'm not the best!
// =================================================================================
#endregion of where is the original source code
#region Licenses [MIT Licenses]
#region MIT License [James Craig]
// =================================================================================
//    Copyright(c) 2014 <a href="http://www.gutgames.com">James Craig</a>
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [James Craig] 
#region MIT License [Dominik Wiesend]
// =================================================================================
//    Copyright(c) 2016 Dominik Wiesend. All rights reserved.
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
// =================================================================================
#endregion of MIT License [Dominik Wiesend] 
#endregion of Licenses [MIT Licenses]

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Threading;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Implements a ring buffer
    /// </summary>
    /// <typeparam name="T">Type of the data it holds</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "<Pending>")]
    public class RingBuffer<T> : ICollection<T>, ICollection
    {
        private object Root;

        /// <summary>
        /// Constructor
        /// </summary>
        public RingBuffer()
            : this(10, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MaxCapacity">Max capacity for the circular buffer</param>
        /// <param name="AllowOverflow">Is overflow allowed (defaults to false)</param>
        public RingBuffer(int MaxCapacity, bool AllowOverflow = false)
        {
            if (!(MaxCapacity > 0)) throw new ArgumentException("Max capacity must be above 0", nameof(MaxCapacity));
            Count = 0;
            IsReadOnly = false;
            this.AllowOverflow = AllowOverflow;
            this.MaxCapacity = MaxCapacity;
            IsSynchronized = false;
            ReadPosition = 0;
            WritePosition = 0;
            Buffer = new T[MaxCapacity];
        }

        /// <summary>
        /// Is overflow allowed?
        /// </summary>
        public bool AllowOverflow { get; protected set; }

        /// <summary>
        /// Item count for the circular buffer
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// Is this synchronized?
        /// </summary>
        public bool IsSynchronized { get; protected set; }

        /// <summary>
        /// Maximum capacity
        /// </summary>
        public int MaxCapacity { get; protected set; }

        /// <summary>
        /// Sync root
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (Root == null)
                    Interlocked.CompareExchange(ref Root, new object(), null);
                return Root;
            }
        }

        /// <summary>
        /// Buffer that the circular buffer uses
        /// </summary>
        protected T[] Buffer { get; set; }

        /// <summary>
        /// Read position
        /// </summary>
        protected int ReadPosition { get; set; }

        /// <summary>
        /// Write position
        /// </summary>
        protected int WritePosition { get; set; }

        /// <summary>
        /// Allows getting an item at a specific position in the buffer
        /// </summary>
        /// <param name="Position">Position to look at</param>
        /// <returns>The specified item</returns>
        public T this[int Position]
        {
            get
            {
                Position %= Count;
                int FinalPosition = (ReadPosition + Position) % MaxCapacity;
                return Buffer[FinalPosition];
            }
            set
            {
                Position %= Count;
                int FinalPosition = (ReadPosition + Position) % MaxCapacity;
                Buffer[FinalPosition] = value;
            }
        }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The value as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator string(RingBuffer<T> Value)
        {
            if (Value == null) throw new ArgumentNullException(nameof(Value));
            return Value.ToString();
        }

        /// <summary>
        /// Adds an item to the buffer
        /// </summary>
        /// <param name="item">Item to add</param>
        public virtual void Add(T item)
        {
            if (Count >= MaxCapacity && !AllowOverflow)
                throw new InvalidOperationException("Unable to add item to circular buffer because the buffer is full");
            Buffer[WritePosition] = item;
            ++Count;
            ++WritePosition;
            if (WritePosition >= MaxCapacity)
                WritePosition = 0;
            if (Count >= MaxCapacity)
                Count = MaxCapacity;
        }

        /// <summary>
        /// Adds a number of items to the buffer
        /// </summary>
        /// <param name="Items">Items to add</param>
        public virtual void Add([NotNull] IEnumerable<T> Items)
        {
            if (Items == null) throw new ArgumentNullException(nameof(Items));
            Items.ForEach(x => Add(x));
        }

        /// <summary>
        /// Adds a number of items to the buffer
        /// </summary>
        /// <param name="buffer">Items to add</param>
        /// <param name="count">Number of items to add</param>
        /// <param name="offset">Offset to start at</param>
        public virtual void Add([NotNull] T[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (!(count <= buffer.Length - offset)) throw new ArgumentOutOfRangeException(nameof(count), $"Contract assertion not met: {nameof(count)} <= {nameof(buffer.Length)} - {nameof(offset)}");
            for (int x = offset; x < offset + count; ++x)
                Add(buffer[x]);
        }

        /// <summary>
        /// Clears the buffer
        /// </summary>
        public virtual void Clear()
        {
            ReadPosition = 0;
            WritePosition = 0;
            Count = 0;
            for (int x = 0; x < MaxCapacity; ++x)
                Buffer[x] = default;
        }

        /// <summary>
        /// Determines if the buffer contains the item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if the item is present, false otherwise</returns>
        public virtual bool Contains(T item)
        {
            int y = ReadPosition;
            var Comparer = new GenericEqualityComparer<T>();
            for (int x = 0; x < Count; ++x)
            {
                if (Comparer.Equals(Buffer[y], item))
                    return true;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            return false;
        }

        /// <summary>
        /// Copies the buffer to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Array index to start at</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            int y = ReadPosition;
            int y2 = arrayIndex;
            int MaxLength = (array.Length - arrayIndex) < Count ? (array.Length - arrayIndex) : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array[y2] = Buffer[y];
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Copies the buffer to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="index">Array index to start at</param>
        public virtual void CopyTo(Array array, int index)
        {
            int y = ReadPosition;
            int y2 = index;
            int MaxLength = (array.Length - index) < Count ? (array.Length - index) : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array.SetValue(Buffer[y], y2);
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Gets the enumerator for the buffer
        /// </summary>
        /// <returns>The enumerator</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            int y = ReadPosition;
            for (int x = 0; x < Count; ++x)
            {
                yield return Buffer[y];
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Gets the enumerator for the buffer
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        /// <summary>
        /// Reads the next item from the buffer
        /// </summary>
        /// <returns>The next item from the buffer</returns>
        public virtual T Remove()
        {
            if (Count == 0)
                return default;
            T ReturnValue = Buffer[ReadPosition];
            Buffer[ReadPosition] = default;
            ++ReadPosition;
            ReadPosition %= MaxCapacity;
            --Count;
            return ReturnValue;
        }

        /// <summary>
        /// Reads the next X number of items from the buffer
        /// </summary>
        /// <param name="Amount">Number of items to return</param>
        /// <returns>The next X items from the buffer</returns>
        public virtual IEnumerable<T> Remove(int Amount)
        {
            if (Count == 0)
                return new List<T>();
            var ReturnValue = new List<T>();
            for (int x = 0; x < Amount; ++x)
                ReturnValue.Add(Remove());
            return ReturnValue;
        }

        /// <summary>
        /// Removes an item from the buffer
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(T item)
        {
            int y = ReadPosition;
            var Comparer = new GenericEqualityComparer<T>();
            for (int x = 0; x < Count; ++x)
            {
                if (Comparer.Equals(Buffer[y], item))
                {
                    Buffer[y] = default;
                    return true;
                }
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            return false;
        }

        /// <summary>
        /// Reads the next X number of items and places them in the array passed in
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="offset">Offset to start at</param>
        /// <param name="count">Number of items to read</param>
        /// <returns>The number of items that were read</returns>
        public virtual int Remove([NotNull] T[] array, int offset, int count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (!(Count <= array.Length - offset)) throw new ArgumentOutOfRangeException(nameof(count), $"Contract assertion not met: {nameof(count)} <= {nameof(array.Length)} - {nameof(offset)}");
            if (Count == 0)
                return 0;
            int y = ReadPosition;
            int y2 = offset;
            int MaxLength = count < Count ? count : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array[y2] = Buffer[y];
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            this.Count -= MaxLength;
            return MaxLength;
        }

        /// <summary>
        /// Skips ahead in the buffer
        /// </summary>
        /// <param name="Count">Number of items in the buffer to skip</param>
        public virtual void Skip(int Count)
        {
            if (Count > this.Count)
                Count = this.Count;
            ReadPosition += Count;
            this.Count -= Count;
            if (ReadPosition >= MaxCapacity)
                ReadPosition %= MaxCapacity;
        }

        /// <summary>
        /// Returns the buffer as a string
        /// </summary>
        /// <returns>The buffer as a string</returns>
        public override string ToString()
        {
            return Buffer.ToString<T>();
        }
    }
}