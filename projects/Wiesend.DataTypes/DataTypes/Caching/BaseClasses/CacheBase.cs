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

using System.Collections.Generic;
using Wiesend.DataTypes.Caching.Interfaces;
using Wiesend.DataTypes.Patterns.BaseClasses;

namespace Wiesend.DataTypes.Caching.BaseClasses
{
    /// <summary>
    /// Cache base class
    /// </summary>
    public abstract class CacheBase : SafeDisposableBaseClass, ICache
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected CacheBase()
        {
            TagMappings = new ListMapping<string, string>();
        }

        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Keys
        /// </summary>
        public abstract ICollection<string> Keys { get; }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The tags used thus far
        /// </summary>
        public IEnumerable<string> Tags { get { return TagMappings.Keys; } }

        /// <summary>
        /// Values
        /// </summary>
        public abstract ICollection<object> Values { get; }

        /// <summary>
        /// Tag mappings
        /// </summary>
        protected ListMapping<string, string> TagMappings { get; private set; }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The object specified</returns>
        public object this[string key]
        {
            get
            {
                TryGetValue(key, out object Value);
                return Value;
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public abstract void Add(string key, object value);

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds a value/key combination and assigns tags to it
        /// </summary>
        /// <param name="Key">Key to add</param>
        /// <param name="Tags">Tags to associate with the key/value pair</param>
        /// <param name="Value">Value to add</param>
        public void Add(string Key, object Value, IEnumerable<string> Tags)
        {
            Add(Key, Value);
            Tags.ForEach(x => TagMappings.Add(x, Key));
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public abstract bool Contains(KeyValuePair<string, object> item);

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public abstract bool ContainsKey(string key);

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public abstract void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex);

        /// <summary>
        /// Gets the objects associated with a specific tag
        /// </summary>
        /// <param name="Tag">Tag to use</param>
        /// <returns>The objects associated with the tag</returns>
        public IEnumerable<object> GetByTag(string Tag)
        {
            var ReturnValue = new List<object>();
            if (!TagMappings.ContainsKey(Tag))
                return ReturnValue;
            foreach (string Key in TagMappings[Tag])
            {
                if (ContainsKey(Key))
                    ReturnValue.Add(this[Key]);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public abstract bool Remove(string key);

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public abstract bool Remove(KeyValuePair<string, object> item);

        /// <summary>
        /// Removes all items associated with the tag specified
        /// </summary>
        /// <param name="Tag">Tag to remove</param>
        public void RemoveByTag(string Tag)
        {
            if (!TagMappings.ContainsKey(Tag))
                return;
            TagMappings[Tag].ForEach(x => Remove(x));
            TagMappings.Remove(Tag);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public abstract bool TryGetValue(string key, out object value);
    }
}