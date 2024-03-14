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
using System.Web;
using Wiesend.IO.FileSystem.BaseClasses;

namespace Wiesend.IO.FileSystem.Default
{
    /// <summary>
    /// Relative local file system
    /// </summary>
    public class RelativeLocalFileSystem : LocalFileSystemBase
    {
        /// <summary>
        /// Name of the file system
        /// </summary>
        public override string Name { get { return "Relative Local"; } }

        /// <summary>
        /// Relative starter
        /// </summary>
        protected override string HandleRegexString { get { return @"^[~|\.]"; } }

        /// <summary>
        /// Gets the absolute path of the variable passed in
        /// </summary>
        /// <param name="Path">Path to convert to absolute</param>
        /// <returns>The absolute path of the path passed in</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        protected override string AbsolutePath(string Path)
        {
            Path = Path.Replace("/", "\\");
            string BaseDirectory = "";
            string ParentDirectory = "";
            if (HttpContext.Current == null)
            {
                BaseDirectory = new System.IO.DirectoryInfo(".").FullName;
                ParentDirectory = new LocalDirectory(BaseDirectory).Parent.FullName;
            }
            else
            {
                BaseDirectory = HttpContext.Current.Server.MapPath("~/");
                ParentDirectory = new LocalDirectory(BaseDirectory).Parent.FullName;
            }
            if (Path.StartsWith("..\\", StringComparison.OrdinalIgnoreCase))
                Path = ParentDirectory + Path.Remove(0, 2);
            else if (Path.StartsWith(".\\", StringComparison.OrdinalIgnoreCase))
                Path = BaseDirectory + Path.Remove(0, 1);
            else if (Path.StartsWith("~\\", StringComparison.OrdinalIgnoreCase))
                Path = BaseDirectory + Path.Remove(0, 1);
            return Path;
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            base.Dispose(Managed);
        }
    }
}