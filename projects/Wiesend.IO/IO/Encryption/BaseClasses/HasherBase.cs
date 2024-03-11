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
using System.Security.Cryptography;
using Wiesend.IO.Encryption.Interfaces;

namespace Wiesend.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Hasher base class
    /// </summary>
    public abstract class HasherBase : IHasher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected HasherBase()
        {
            ImplementedAlgorithms = new Dictionary<string, Func<HashAlgorithm>>();
        }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Algorithms this implements
        /// </summary>
        protected IDictionary<string, Func<HashAlgorithm>> ImplementedAlgorithms { get; private set; }

        /// <summary>
        /// Can this handle the algorithm specified
        /// </summary>
        /// <param name="Algorithm">The algorithm name</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanHandle(string Algorithm)
        {
            return ImplementedAlgorithms.ContainsKey(Algorithm.ToUpperInvariant());
        }

        /// <summary>
        /// Hashes the data
        /// </summary>
        /// <param name="Data">Data to hash</param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The hashed version of the data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public byte[] Hash(byte[] Data, string Algorithm)
        {
            if (Data == null)
                return null;
            using HashAlgorithm Hasher = GetAlgorithm(Algorithm);
            byte[] HashedArray = new byte[0];
            if (Hasher != null)
            {
                HashedArray = Hasher.ComputeHash(Data);
                Hasher.Clear();
            }
            return HashedArray;
        }

        /// <summary>
        /// Gets the hash algorithm that the system uses
        /// </summary>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>The hash algorithm</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        protected HashAlgorithm GetAlgorithm([NotNull] string Algorithm)
        {
            if (string.IsNullOrEmpty(Algorithm)) throw new ArgumentNullException(nameof(Algorithm));
            if (ImplementedAlgorithms == null) throw new NullReferenceException("ImplementedAlgorithms");
            return ImplementedAlgorithms[Algorithm.ToUpperInvariant()]();
        }
    }
}