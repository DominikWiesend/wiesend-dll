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

using System;
using System.IO;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.IO.FileSystem.BaseClasses;
using Wiesend.IO.FileSystem.Interfaces;

namespace Wiesend.IO.FileSystem.Default
{
    /// <summary>
    /// Basic local file class
    /// </summary>
    public class LocalFile : FileBase<System.IO.FileInfo, LocalFile>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocalFile()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public LocalFile(string Path)
            : base(string.IsNullOrEmpty(Path) ? null : new System.IO.FileInfo(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="File">File to use</param>
        public LocalFile(System.IO.FileInfo File)
            : base(File)
        {
        }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public override DateTime Accessed
        {
            get { return InternalFile == null ? DateTime.Now : InternalFile.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public override DateTime Created
        {
            get { return InternalFile == null ? DateTime.Now : InternalFile.CreationTimeUtc; }
        }

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public override IDirectory Directory
        {
            get { return InternalFile == null ? null : new LocalDirectory(InternalFile.Directory); }
        }

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public override bool Exists
        {
            get { return InternalFile != null && InternalFile.Exists; }
        }

        /// <summary>
        /// File extension
        /// </summary>
        public override string Extension
        {
            get { return InternalFile == null ? "" : InternalFile.Extension; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalFile == null ? "" : InternalFile.FullName; }
        }

        /// <summary>
        /// Size of the file
        /// </summary>
        public override long Length
        {
            get { return InternalFile == null || !InternalFile.Exists ? 0 : InternalFile.Length; }
        }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public override DateTime Modified
        {
            get { return InternalFile == null ? DateTime.Now : InternalFile.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Name of the file
        /// </summary>
        public override string Name
        {
            get { return InternalFile == null ? "" : InternalFile.Name; }
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public override IFile CopyTo(IDirectory Directory, bool Overwrite)
        {
            if (Directory == null || !Exists)
                return null;
            Directory.Create();
            var File = new FileInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain);
            if (!File.Exists || Overwrite)
            {
                File.Write(ReadBinary());
                return File;
            }
            return this;
        }

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public override string Delete()
        {
            if (!Exists)
                return "";
            InternalFile.Delete();
            InternalFile.Refresh();
            return "";
        }

        /// <summary>
        /// Moves the file to a new directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public override void MoveTo(IDirectory Directory)
        {
            if (Directory == null || !Exists)
                return;
            Directory.Create();
            InternalFile.MoveTo(Directory.FullName + "\\" + Name);
            InternalFile = new System.IO.FileInfo(Directory.FullName + "\\" + Name);
        }

        /// <summary>
        /// Reads the file in as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        public override string Read()
        {
            if (!Exists)
                return "";
            using StreamReader Reader = InternalFile.OpenText();
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// Reads a file as binary
        /// </summary>
        /// <returns>The file contents as a byte array</returns>
        public override byte[] ReadBinary()
        {
            if (!Exists)
            {
#if NET45
                return new byte[0];
#else
                return Array.Empty<byte>();
#endif
            }
            using FileStream Reader = InternalFile.OpenRead();
            byte[] Buffer = new byte[1024];
            using MemoryStream Temp = new();
            while (true)
            {
                var Count = Reader.Read(Buffer, 0, Buffer.Length);
                if (Count <= 0)
                    return Temp.ToArray();
                Temp.Write(Buffer, 0, Count);
            }
        }

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="NewName">New name for the file</param>
        public override void Rename(string NewName)
        {
            if (string.IsNullOrEmpty(NewName) || !Exists)
                return;
            InternalFile.MoveTo(InternalFile.DirectoryName + "\\" + NewName);
            InternalFile = new System.IO.FileInfo(InternalFile.DirectoryName + "\\" + NewName);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <param name="Encoding">Encoding to use for the content</param>
        /// <returns>The result of the write or original content</returns>
        public override string Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            Content ??= "";
            Encoding ??= new ASCIIEncoding();
            return Write(Encoding.GetBytes(Content), Mode).ToString(Encoding);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <returns>The result of the write or original content</returns>
        public override byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            if (Content == null)
            {
#if NET45
                Content = new byte[0];
#else
                Content = Array.Empty<byte>();
#endif
            }
            Directory.Create();
            using (FileStream Writer = InternalFile.Open(Mode, FileAccess.Write))
                Writer.Write(Content, 0, Content.Length);
            InternalFile.Refresh();
            return Content;
        }
    }
}