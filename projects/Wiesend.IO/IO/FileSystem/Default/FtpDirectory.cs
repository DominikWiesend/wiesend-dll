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

#if NETFRAMEWORK || NETSTANDARD
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Wiesend.IO.FileSystem.BaseClasses;
using Wiesend.IO.FileSystem.Interfaces;

namespace Wiesend.IO.FileSystem.Default
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class FtpDirectory : DirectoryBase<Uri, FtpDirectory>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FtpDirectory()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public FtpDirectory(string Path, string UserName = "", string Password = "", string Domain = "")
            : this(string.IsNullOrEmpty(Path) ? null : new Uri(Path), UserName, Password, Domain)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Directory">Internal directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public FtpDirectory(Uri Directory, string UserName = "", string Password = "", string Domain = "")
            : base(Directory, UserName, Password, Domain)
        {
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Accessed
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Created
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns true
        /// </summary>
        public override bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string Name
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override IDirectory Parent
        {
            get { return InternalDirectory == null ? null : new FtpDirectory((string)InternalDirectory.AbsolutePath.Take(InternalDirectory.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1), UserName, Password, Domain); }
        }

        /// <summary>
        /// Root
        /// </summary>
        public override IDirectory Root
        {
            get { return InternalDirectory == null ? null : new FtpDirectory(InternalDirectory.Scheme + "://" + InternalDirectory.Host, UserName, Password, Domain); }
        }

        /// <summary>
        /// Size (returns 0)
        /// </summary>
        public override long Size
        {
            get { return 0; }
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Create()
        {
            var Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.MakeDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Delete()
        {
            var Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            var Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.ListDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            var Data = SendRequest(Request);
            var Folders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            SetupData(Request, null);
            SetupCredentials(Request);
            Data = SendRequest(Request);
            var DetailedFolders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var Directories = new List<IDirectory>();
            foreach (string Folder in Folders)
            {
                var DetailedFolder = DetailedFolders.FirstOrDefault(x => x.EndsWith(Folder, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(DetailedFolder))
                {
                    if (DetailedFolder.StartsWith("d", StringComparison.OrdinalIgnoreCase) && !DetailedFolder.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                    {
                        Directories.Add(new DirectoryInfo(FullName + "/" + Folder, UserName, Password, Domain));
                    }
                }
            }
            return Directories;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            var Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.ListDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            var Data = SendRequest(Request);
            var Folders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            SetupData(Request, null);
            SetupCredentials(Request);
            Data = SendRequest(Request);
            var DetailedFolders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var Directories = new List<IFile>();
            foreach (string Folder in Folders)
            {
                var DetailedFolder = DetailedFolders.FirstOrDefault(x => x.EndsWith(Folder, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(DetailedFolder))
                {
                    if (!DetailedFolder.StartsWith("d", StringComparison.OrdinalIgnoreCase))
                    {
                        Directories.Add(new FileInfo(FullName + "/" + Folder, UserName, Password, Domain));
                    }
                }
            }
            return Directories;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Name"></param>
        public override void Rename(string Name)
        {
#if NETFRAMEWORK || NETSTANDARD
            var Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.Rename;
            Request.RenameTo = Name;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
            InternalDirectory = new Uri(FullName + "/" + Name);
#endif
#if NET

#endif
        }

        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <returns>The string returned by the service</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        private static string SendRequest([NotNull] FtpWebRequest Request)
        {
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            using FtpWebResponse Response = Request.GetResponse() as FtpWebResponse;
            using StreamReader Reader = new StreamReader(Response.GetResponseStream());
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// Sets up any credentials (basic authentication, for OAuth, please use the OAuth class to
        /// create the
        /// URL)
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupCredentials(FtpWebRequest Request)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                Request.Credentials = new NetworkCredential(UserName, Password);
        }

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <param name="Data">Data to send with the request</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        private void SetupData([NotNull] FtpWebRequest Request, byte[] Data)
        {
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            if (string.IsNullOrEmpty(Name)) throw new NullReferenceException("Name");
            Request.UsePassive = true;
            Request.KeepAlive = false;
            Request.UseBinary = true;
            Request.EnableSsl = Name.ToUpperInvariant().StartsWith("FTPS", StringComparison.OrdinalIgnoreCase);
            if (Data == null)
            {
                Request.ContentLength = 0;
                return;
            }
            Request.ContentLength = Data.Length;
            using Stream RequestStream = Request.GetRequestStream();
            RequestStream.Write(Data, 0, Data.Length);
        }
    }
}
#endif