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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// IDictionary extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IDictionaryExtensions
    {
        /// <summary>
        /// Copies the dictionary to another dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="Dictionary">The dictionary.</param>
        /// <param name="Target">The target dictionary.</param>
        /// <returns>
        /// This
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        public static IDictionary<TKey, TValue> CopyTo<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> Dictionary, [NotNull] IDictionary<TKey, TValue> Target)
        {
            if (Dictionary == null) throw new ArgumentNullException(nameof(Dictionary));
            if (Target == null) throw new ArgumentNullException(nameof(Target));
            foreach (KeyValuePair<TKey, TValue> Pair in Dictionary)
            {
                Target.SetValue(Pair.Key, Pair.Value);
            }
            return Dictionary;
        }

        /// <summary>
        /// Gets the value from a dictionary or the default value if it isn't found
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to get the value from</param>
        /// <param name="Key">Key to look for</param>
        /// <param name="Default">Default value if the key is not found</param>
        /// <returns>
        /// The value associated with the key or the default value if the key is not found
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public static TValue GetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> Dictionary, TKey Key, TValue Default = default)
        {
            if (Dictionary == null) throw new ArgumentNullException(nameof(Dictionary));
            TValue ReturnValue = Default;
            return Dictionary.TryGetValue(Key, out ReturnValue) ? ReturnValue : Default;
        }

        /// <summary>
        /// Sets the value in a dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to set the value in</param>
        /// <param name="Key">Key to look for</param>
        /// <param name="Value">Value to add</param>
        /// <returns>The dictionary</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        public static IDictionary<TKey, TValue> SetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> Dictionary, TKey Key, TValue Value)
        {
            if (Dictionary == null) throw new ArgumentNullException(nameof(Dictionary));
            if (Dictionary.ContainsKey(Key))
                Dictionary[Key] = Value;
            else
                Dictionary.Add(Key, Value);
            return Dictionary;
        }

        /// <summary>
        /// Sorts a dictionary
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to sort</param>
        /// <param name="Comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> Sort<T1, T2>([NotNull] this IDictionary<T1, T2> Dictionary, IComparer<T1> Comparer = null)
            where T1 : IComparable
        {
            if (Dictionary == null) throw new ArgumentNullException(nameof(Dictionary));
            return Dictionary.Sort(x => x.Key, Comparer);
        }

        /// <summary>
        /// Sorts a dictionary
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <typeparam name="T3">Order by type</typeparam>
        /// <param name="Dictionary">Dictionary to sort</param>
        /// <param name="OrderBy">Function used to order the dictionary</param>
        /// <param name="Comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> Sort<T1, T2, T3>([NotNull] this IDictionary<T1, T2> Dictionary, [NotNull] Func<KeyValuePair<T1, T2>, T3> OrderBy, IComparer<T3> Comparer = null)
            where T3 : IComparable
        {
            if (Dictionary == null) throw new ArgumentNullException(nameof(Dictionary));
            if (OrderBy == null) throw new ArgumentNullException(nameof(OrderBy));
            return Dictionary.OrderBy(OrderBy, Comparer.Check(() => new GenericComparer<T3>())).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}