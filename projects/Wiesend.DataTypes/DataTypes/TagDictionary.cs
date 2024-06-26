﻿#region Project Description [About this]
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
//    Copyright(c) 2018 Dominik Wiesend. All rights reserved.
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

using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Dictionary that matches multiple keys to each value
    /// </summary>
    /// <typeparam name="Key">Key type</typeparam>
    /// <typeparam name="Value">Value type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public class TagDictionary<Key, Value> : IDictionary<Key, IEnumerable<Value>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TagDictionary()
        {
            Items = new ConcurrentBag<TaggedItem<Key, Value>>();
            KeyList = new List<Key>();
        }

        /// <summary>
        /// Number of items in the dictionary
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1829:Use Length/Count property instead of Count() when available", Justification = "<Pending>")]
        public int Count
        {
            get { return Items.Count(); }
        }

        /// <summary>
        /// Always false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the keys found in the dictionary
        /// </summary>
        public ICollection<Key> Keys
        {
            get { return KeyList; }
        }

        /// <summary>
        /// Gets the values found in the dictionary
        /// </summary>
        public ICollection<IEnumerable<Value>> Values
        {
            get { return new IEnumerable<Value>[] { Items.ToArray(x => x.Value) }; }
        }

        /// <summary>
        /// Items in the dictionary
        /// </summary>
        private ConcurrentBag<TaggedItem<Key, Value>> Items { get; set; }

        /// <summary>
        /// List of keys that have been entered
        /// </summary>
        private List<Key> KeyList { get; set; }

        /// <summary>
        /// Gets the values based on a key
        /// </summary>
        /// <param name="key">Key to get the values of</param>
        /// <returns>The values associated with the key</returns>
        public IEnumerable<Value> this[Key key]
        {
            get
            {
                return Items.Where(x => x.Keys.Contains(key)).ToArray(x => x.Value);
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Adds a list of values to the key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Values to add</param>
        public void Add(Key key, IEnumerable<Value> value)
        {
            value.ToArray(x => new TaggedItem<Key, Value>(key, x)).ForEach(x => Items.Add(x));
            KeyList.AddIfUnique(key);
        }

        /// <summary>
        /// Adds a value to the dicionary
        /// </summary>
        /// <param name="Value">Value to add</param>
        /// <param name="Keys">Keys to associate the value with</param>
        public void Add(Value Value, [NotNull] params Key[] Keys)
        {
            if (Keys == null) throw new ArgumentNullException(nameof(Keys));
            Items.Add(new TaggedItem<Key, Value>(Keys, Value));
            Keys.ForEach(x => KeyList.AddIfUnique(x));
        }

        /// <summary>
        /// Adds an item to the dictionary
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<Key, IEnumerable<Value>> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            Items = new ConcurrentBag<TaggedItem<Key, Value>>();
        }

        /// <summary>
        /// Determines if the dictionary contains the key/value pair
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool Contains(KeyValuePair<Key, IEnumerable<Value>> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Determines if a key is in the dictionary
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsKey(Key key)
        {
            return KeyList.Contains(key);
        }

        /// <summary>
        /// Copies itself to an array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(KeyValuePair<Key, IEnumerable<Value>>[] array, int arrayIndex)
        {
            for (int x = 0; x < Keys.Count; ++x)
            {
                array[arrayIndex + x] = new KeyValuePair<Key, IEnumerable<Value>>(Keys.ElementAt(x), this[Keys.ElementAt(x)]);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<Key, IEnumerable<Value>>> GetEnumerator()
        {
            foreach (Key Key in Keys)
            {
                yield return new KeyValuePair<Key, IEnumerable<Value>>(Key, this[Key]);
            }
        }

        /// <summary>
        /// Removes all items that are associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns true if the key was found, false otherwise</returns>
        public bool Remove(Key key)
        {
            var ReturnValue = ContainsKey(key);
            Items = new ConcurrentBag<TaggedItem<Key, Value>>(Items.ToArray(x => x).Where(x => !x.Keys.Contains(key)));
            KeyList.Remove(key);
            return ReturnValue;
        }

        /// <summary>
        /// Removes a specific key/value pair
        /// </summary>
        /// <param name="item">item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<Key, IEnumerable<Value>> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Key Key in Keys)
            {
                yield return this[Key];
            }
        }

        /// <summary>
        /// Attempts to get the values associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Values associated with a key</param>
        /// <returns>True if something is returned, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1827:Do not use Count() or LongCount() when Any() can be used", Justification = "<Pending>")]
        public bool TryGetValue(Key key, out IEnumerable<Value> value)
        {
            value = new List<Value>();
            try
            {
                value = this[key];
            }
            catch { }
            return value.Count() > 0;
        }

        /// <summary>
        /// Holds information about each value
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        private class TaggedItem<TKey, TValue>
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Keys">Keys</param>
            /// <param name="Value">Value</param>
            public TaggedItem(IEnumerable<TKey> Keys, TValue Value)
            {
                this.Keys = new ConcurrentBag<TKey>(Keys);
                this.Value = Value;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Key">Key</param>
            /// <param name="Value">Value</param>
            public TaggedItem(TKey Key, TValue Value)
            {
                this.Keys = new ConcurrentBag<TKey>(new TKey[] { Key });
                this.Value = Value;
            }

            /// <summary>
            /// The list of keys associated with the value
            /// </summary>
            public ConcurrentBag<TKey> Keys { get; set; }

            /// <summary>
            /// Value
            /// </summary>
            public TValue Value { get; set; }
        }
    }
}