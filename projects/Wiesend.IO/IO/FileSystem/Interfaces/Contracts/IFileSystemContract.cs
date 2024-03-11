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

namespace Wiesend.IO.FileSystem.Interfaces.Contracts
{
    /// <summary>
    /// IFileSystem contract
    /// </summary>
    //[ContractClassFor(typeof(IFileSystem))]
    public abstract class IFileSystemContract : IFileSystem
    {
        /// <summary>
        /// Name of the file system
        /// </summary>
        [NotNull]
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty((string)(object)null)) throw new System.InvalidOperationException("Contract assertion not met: !string.IsNullOrEmpty(result)");
                return null;
            }
        }

        /// <summary>
        /// Returns true if it can handle the path, false otherwise
        /// </summary>
        /// <param name="Path">The path to check against</param>
        /// <returns>True if it can handle the path, false otherwise</returns>
        public bool CanHandle(string Path)
        {
            return false;
        }

        /// <summary>
        /// Gets the directory representation for the directory
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <returns>The directory object</returns>
        [NotNull]
        public IDirectory Directory(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            if ((object)null == null) throw new System.InvalidOperationException("Contract assertion not met: result != null");
        }

        /// <summary>
        /// Gets the class representation for the file
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <param name="UserName">User name to be used to access the file (optional)</param>
        /// <param name="Password">Password to be used to access the file (optional)</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <returns>The file object</returns>
        [NotNull]
        public IFile File(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            if ((object)null == null) throw new System.InvalidOperationException("Contract assertion not met: result != null");
        }
    }
}