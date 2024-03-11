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
using System.Linq;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.IO.Compression.Interfaces;

namespace Wiesend.IO.Compression
{
    /// <summary>
    /// Compression manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Compressors">The compressors.</param>
        public Manager([NotNull] IEnumerable<ICompressor> Compressors)
        {
            if (Compressors == null) throw new ArgumentNullException(nameof(Compressors));
            this.Compressors = Compressors.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Compressors
        /// </summary>
        public IDictionary<string, ICompressor> Compressors { get; private set; }

        /// <summary>
        /// Compresses the data
        /// </summary>
        /// <param name="Data">Data to compress</param>
        /// <param name="Compressor">Compressor name</param>
        /// <returns>The compressed data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public byte[] Compress(byte[] Data, string Compressor)
        {
            if (Compressors == null) throw new NullReferenceException("Compressors");
            return Compressors.ContainsKey(Compressor) ? Compressors.GetValue(Compressor).Compress(Data) : Data;
        }

        /// <summary>
        /// Decompresses the data
        /// </summary>
        /// <param name="Data">Data to decompress</param>
        /// <param name="Compressor">Compressor name</param>
        /// <returns>The decompressed data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public byte[] Decompress(byte[] Data, string Compressor)
        {
            if (Compressors == null) throw new NullReferenceException("Compressors");
            return Compressors.ContainsKey(Compressor) ? Compressors.GetValue(Compressor).Decompress(Data) : Data;
        }

        /// <summary>
        /// String info for the manager
        /// </summary>
        /// <returns>The string info that the manager contains</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("Compressors: {0}", Compressors.OrderBy(x => x.Key).ToString(x => x.Key, ", "));
            return Builder.ToString();
        }
    }
}