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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// IComparable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IComparableExtensions
    {
        /// <summary>
        /// Checks if an item is between two values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="Value">Value to check</param>
        /// <param name="Min">Minimum value</param>
        /// <param name="Max">Maximum value</param>
        /// <param name="Comparer">
        /// Comparer used to compare the values (defaults to GenericComparer)"
        /// </param>
        /// <returns>True if it is between the values, false otherwise</returns>
        public static bool Between<T>(this T Value, T Min, T Max, IComparer<T> Comparer = null)
            where T : IComparable
        {
            Comparer = Comparer.Check(() => new GenericComparer<T>());
            return Comparer.Compare(Max, Value) >= 0 && Comparer.Compare(Value, Min) >= 0;
        }

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="Value">Value sent in</param>
        /// <param name="Max">Max value it can be (inclusive)</param>
        /// <param name="Min">Min value it can be (inclusive)</param>
        /// <param name="Comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The value set between Min and Max</returns>
        public static T Clamp<T>(this T Value, T Max, T Min, IComparer<T> Comparer = null)
            where T : IComparable
        {
            Comparer = Comparer.Check(() => new GenericComparer<T>());
            if (Comparer.Compare(Max, Value) < 0)
                return Max;
            if (Comparer.Compare(Value, Min) < 0)
                return Min;
            return Value;
        }

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <param name="Comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The maximum value</returns>
        public static T Max<T>(this T InputA, T InputB, IComparer<T> Comparer = null)
            where T : IComparable
        {
            Comparer = Comparer.Check(() => new GenericComparer<T>());
            return Comparer.Compare(InputA, InputB) < 0 ? InputB : InputA;
        }

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <param name="Comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The minimum value</returns>
        public static T Min<T>(this T InputA, T InputB, IComparer<T> Comparer = null)
            where T : IComparable
        {
            Comparer = Comparer.Check(() => new GenericComparer<T>());
            return Comparer.Compare(InputA, InputB) > 0 ? InputB : InputA;
        }
    }
}