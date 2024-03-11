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
using System.IO;
using System.Text.RegularExpressions;
using Wiesend.DataTypes;
using Wiesend.IO;

namespace Wiesend.Web.Streams
{
    /// <summary>
    /// Removes "pretty printing" from HTML
    /// </summary>
    public class UglyStream : Stream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StreamUsing">The stream for the page</param>
        /// <param name="Compression">The compression we're using (gzip or deflate)</param>
        /// <param name="Type">Minification type to use (defaults to HTML)</param>
        public UglyStream(Stream StreamUsing, CompressionType Compression, MinificationType Type = MinificationType.HTML)
        {
            this.Compression = Compression;
            this.StreamUsing = StreamUsing;
            this.Type = Type;
        }

        /// <summary>
        /// Doesn't deal with reading
        /// </summary>
        public override bool CanRead
        {
            get { return false; }
        }

        /// <summary>
        /// No seeking
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Can write out though
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Don't worry about
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// No position to take care of
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Compression using
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        private CompressionType Compression;

        /// <summary>
        /// Final output string
        /// </summary>
        private string FinalString;

        /// <summary>
        /// Stream using
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "<Pending>")]
        private Stream StreamUsing;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        private MinificationType Type;

        /// <summary>
        /// Nothing to flush
        /// </summary>
        public override void Flush()
        {
            if (string.IsNullOrEmpty(FinalString))
                return;
            var Data = FinalString.Minify(Type).ToByteArray();
            Data = Data.Compress(Compression);
            if (Data != null)
                StreamUsing.Write(Data, 0, Data.Length);
            FinalString = "";
        }

        /// <summary>
        /// Don't worry about
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Once again not implemented
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Don't worry about
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Actually writes out the data
        /// </summary>
        /// <param name="buffer">the page's data in byte form</param>
        /// <param name="offset">offset of the data</param>
        /// <param name="count">the amount of data</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] Data = new byte[count];
            Buffer.BlockCopy(buffer, offset, Data, 0, count);
            var inputstring = Data.ToString(null);
            FinalString += inputstring;
        }

        /// <summary>
        /// Evaluates whether the text has spaces, page breaks, etc. and removes them.
        /// </summary>
        /// <param name="Matcher">Match found</param>
        /// <returns>The string minus any extra white space</returns>
        protected static string Evaluate([NotNull] Match Matcher)
        {
            if (Matcher == null) throw new ArgumentNullException(nameof(Matcher));
            var MyString = Matcher.ToString();
            MyString = Regex.Replace(MyString, @"\r\n\s*", "");
            return MyString;
        }
    }
}