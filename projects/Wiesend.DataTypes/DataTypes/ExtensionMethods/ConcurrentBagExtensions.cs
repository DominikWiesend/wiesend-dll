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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// ConcurrentBag extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ConcurrentBagExtensions
    {
        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ConcurrentBag<T> Add<T>([NotNull] this ConcurrentBag<T> Collection, IEnumerable<T> Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Items == null)
                return Collection;
            Parallel.ForEach(Items, x => Collection.Add(x));
            return Collection;
        }

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ConcurrentBag<T> Add<T>([NotNull] this ConcurrentBag<T> Collection, params T[] Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Items == null)
                return Collection;
            Parallel.ForEach(Items, x => Collection.Add(x));
            return Collection;
        }

        /// <summary>
        /// Adds an item to a list and returns the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Item">Item to add to the collection</param>
        /// <returns>The original item</returns>
        public static T AddAndReturn<T>([NotNull] this ConcurrentBag<T> Collection, T Item)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            Collection.Add(Item);
            return Item;
        }

        /// <summary>
        /// Adds items to the collection if it passes the predicate test
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">
        /// Predicate that an item needs to satisfy in order to be added
        /// </param>
        /// <returns>True if any are added, false otherwise</returns>
        public static bool AddIf<T>([NotNull] this ConcurrentBag<T> Collection, [NotNull] Predicate<T> Predicate, params T[] Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Items == null)
                return true;
            return Items.ForEachParallel(Item =>
            {
                if (Predicate(Item))
                {
                    Collection.Add(Item);
                    return true;
                }
                return false;
            }).Any(x => x);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">
        /// Predicate that an item needs to satisfy in order to be added
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIf<T>([NotNull] this ConcurrentBag<T> Collection, [NotNull] Predicate<T> Predicate, IEnumerable<T> Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Items == null)
                return true;
            return Collection.AddIf(Predicate, Items.ToArray());
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>([NotNull] this ConcurrentBag<T> Collection, params T[] Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Items == null)
                return true;
            return Collection.AddIf(x => !Collection.Contains(x), Items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>([NotNull] this ConcurrentBag<T> Collection, [NotNull] Func<T, T, bool> Predicate, params T[] Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Items == null)
                return true;
            return Collection.AddIf(x => !Collection.Any(y => Predicate(x, y)), Items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>([NotNull] this ConcurrentBag<T> Collection, IEnumerable<T> Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Items == null)
                return true;
            return Collection.AddIf(x => !Collection.Contains(x), Items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>([NotNull] this ConcurrentBag<T> Collection, [NotNull] Func<T, T, bool> Predicate, IEnumerable<T> Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Predicate == null) throw new ArgumentNullException(nameof(Predicate));
            if (Items == null)
                return true;
            return Collection.AddIf(x => !Collection.Any(y => Predicate(x, y)), Items);
        }

        /// <summary>
        /// Determines whether the bag contains the item specified
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Collection">The collection.</param>
        /// <param name="Item">The item.</param>
        /// <param name="Comparer">The comparer.</param>
        /// <returns>
        /// True if the item is present, false otherwise
        /// </returns>
        public static bool Contains<T>([NotNull] this ConcurrentBag<T> Collection, T Item, IEqualityComparer<T> Comparer = null)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            Comparer = Comparer.Check(new GenericEqualityComparer<T>());
            foreach (T TempValue in Collection)
            {
                if (Comparer.Equals(TempValue, Item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection to remove items from</param>
        /// <param name="Predicate">Predicate used to determine what items to remove</param>
        public static ConcurrentBag<T> Remove<T>([NotNull] this ConcurrentBag<T> Collection, Func<T, bool> Predicate)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            return new ConcurrentBag<T>(Collection.Where(x => !Predicate(x)));
        }

        /// <summary>
        /// Removes all items in the list from the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to remove</param>
        /// <returns>The collection with the items removed</returns>
        public static ConcurrentBag<T> Remove<T>([NotNull] this ConcurrentBag<T> Collection, IEnumerable<T> Items)
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Items == null)
                return Collection;
            return new ConcurrentBag<T>(Collection.Where(x => !Items.Contains(x)));
        }
    }
}