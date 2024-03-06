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
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Text;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Class to be used for sets of data
    /// </summary>
    /// <typeparam name="T">Type that the set holds</typeparam>
    public class Set<T> : List<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Set()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitialSize">Initial size</param>
        public Set(int InitialSize)
            : base(InitialSize)
        {
            if (!(InitialSize >= 0)) throw new ArgumentOutOfRangeException(nameof(InitialSize), "InitialSize should be larger than or equal to 0");
        }

        /// <summary>
        /// Gets the intersection of set 1 and set 2
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The intersection of the two sets</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
        public static Set<T> GetIntersection(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null || Set2 == null || !Set1.Intersect(Set2))
                return null;
            var ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.Count; ++x)
                if (Set2.Contains(Set1[x]))
                    ReturnValue.Add(Set1[x]);

            for (int x = 0; x < Set2.Count; ++x)
                if (Set1.Contains(Set2[x]))
                    ReturnValue.Add(Set2[x]);

            return ReturnValue;
        }

        /// <summary>
        /// Removes items from set 2 from set 1
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The resulting set</returns>
        public static Set<T> operator -(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null) throw new ArgumentNullException(nameof(Set1));
            if (Set2 == null) throw new ArgumentNullException(nameof(Set2));
            var ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.Count; ++x)
                if (!Set2.Contains(Set1[x]))
                    ReturnValue.Add(Set1[x]);
            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are not equivalent
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>False if they are, true otherwise</returns>
        public static bool operator !=(Set<T> Set1, Set<T> Set2)
        {
            return !(Set1 == Set2);
        }

        /// <summary>
        /// Adds two sets together
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The joined sets</returns>
        public static Set<T> operator +(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null) throw new ArgumentNullException(nameof(Set1));
            if (Set2 == null) throw new ArgumentNullException(nameof(Set2));
            var ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.Count; ++x)
                ReturnValue.Add(Set1[x]);
            for (int x = 0; x < Set2.Count; ++x)
                ReturnValue.Add(Set2[x]);
            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Set<T> Set1, Set<T> Set2)
        {
            if (((object)Set1) == null && ((object)Set2) == null)
                return true;
            if (((object)Set1) == null || ((object)Set2) == null)
                return false;
            return Set1.Contains(Set2) && Set2.Contains(Set1);
        }

        /// <summary>
        /// Used to tell if this set contains the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public virtual bool Contains([NotNull] Set<T> Set)
        {
            if (Set == null) throw new ArgumentNullException(nameof(Set));
            return Set.IsSubset(this);
        }

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return this == (obj as Set<T>);
        }

        /// <summary>
        /// Returns the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines if the sets intersect
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if they do, false otherwise</returns>
        public virtual bool Intersect(Set<T> Set)
        {
            if (Set == null)
                return false;
            for (int x = 0; x < this.Count; ++x)
                if (Set.Contains(this[x]))
                    return true;
            return false;
        }

        /// <summary>
        /// Used to tell if this is a subset of the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public virtual bool IsSubset(Set<T> Set)
        {
            if (Set == null || this.Count > Set.Count)
                return false;

            for (int x = 0; x < this.Count; ++x)
                if (!Set.Contains(this[x]))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns the set as a string
        /// </summary>
        /// <returns>The set as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.Append("{ ");
            string Splitter = "";
            for (int x = 0; x < this.Count; ++x)
            {
                Builder.Append(Splitter);
                Builder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}", this[x]);
                Splitter = ",  ";
            }
            Builder.Append(" }");
            return Builder.ToString();
        }
    }
}