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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Wiesend.DataTypes;
using Wiesend.IO.Enums;
using Wiesend.IO.FileSystem.BaseClasses;
using Wiesend.IO.FileSystem.Interfaces;
#if NET
using System.Net.Http;
#endif

namespace Wiesend.IO.FileSystem.Default
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class WebDirectory : DirectoryBase<Uri, WebDirectory>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WebDirectory()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public WebDirectory(string Path, string UserName = "", string Password = "", string Domain = "")
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
        public WebDirectory(Uri Directory, string UserName = "", string Password = "", string Domain = "")
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
            get { return InternalDirectory == null ? null : new WebDirectory((string)InternalDirectory.AbsolutePath.Take(InternalDirectory.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1), UserName, Password, Domain); }
        }

        /// <summary>
        /// Root
        /// </summary>
        public override IDirectory Root
        {
            get { return InternalDirectory == null ? null : new WebDirectory(InternalDirectory.Scheme + "://" + InternalDirectory.Host, UserName, Password, Domain); }
        }

        /// <summary>
        /// Size (returns 0)
        /// </summary>
        public override long Size
        {
            get { return 0; }
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Options</param>
        /// <returns>Newly created directory</returns>
        public override IDirectory CopyTo(IDirectory Directory, CopyOptions Options = CopyOptions.CopyAlways)
        {
            if (Directory == null)
                return this;
            string TempName = Name;
            if (TempName == "/")
                TempName = "index.html";
            var NewDirectory = new FileInfo(Directory.FullName + "\\" + TempName.Right(TempName.Length - (TempName.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain);
            var OldFile = new FileInfo(InternalDirectory.AbsoluteUri, UserName, Password, Domain);
            NewDirectory.Write(OldFile.Read(), FileMode.Create);
            return Directory;
        }

        /// <summary>
        /// Not used
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        public override void Create()
        {
#if NETFRAMEWORK || NETSTANDARD
            HttpWebRequest Request = WebRequest.Create(InternalDirectory) as HttpWebRequest;
            Request.Method = "POST";
            Request.ContentType = "text/xml";
            SetupData(Request, "");
            SetupCredentials(Request);
            SendRequest(Request);
#endif
#if NET
            HttpClientHandler Handler = new HttpClientHandler();
            HttpRequestMessage Request = new HttpRequestMessage { Method = HttpMethod.Post };
            Request.Headers.Add("Content-Type", "text/xml");
            Request.RequestUri = InternalDirectory;
            SetupData(Request, "");
            SetupCredentials(Handler);
            SendRequest(Handler, Request);
#endif
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Delete()
        {
            if (InternalDirectory == null)
                return;
#if NETFRAMEWORK || NETSTANDARD
            HttpWebRequest Request = WebRequest.Create(InternalDirectory) as HttpWebRequest;
            Request.Method = "DELETE";
            Request.ContentType = "text/xml";
            SetupData(Request, "");
            SetupCredentials(Request);
            SendRequest(Request);
#endif
#if NET
            HttpClientHandler Handler = new();
            HttpRequestMessage Request = new() { Method = HttpMethod.Delete };
            Request.Headers.Add("Content-Type", "text/xml");
            Request.RequestUri = InternalDirectory;
            SetupData(Request, "");
            SetupCredentials(Handler);
            SendRequest(Handler, Request);
#endif
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return new List<WebDirectory>();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return new List<WebFile>();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Name"></param>
        public override void Rename(string Name)
        {
        }

#if NETFRAMEWORK || NETSTANDARD
        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <returns>The string returned by the service</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        private static string SendRequest([NotNull] HttpWebRequest Request)
        {
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            using HttpWebResponse Response = Request.GetResponse() as HttpWebResponse;
            if (Response.StatusCode != HttpStatusCode.OK)
                return "";
            using StreamReader Reader = new StreamReader(Response.GetResponseStream());
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <param name="Data">Data to send with the request</param>
        private static void SetupData([NotNull] HttpWebRequest Request, string Data)
        {
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            if (string.IsNullOrEmpty(Data))
            {
                Request.ContentLength = 0;
                return;
            }
            var ByteData = Data.ToByteArray();
            Request.ContentLength = ByteData.Length;
            using Stream RequestStream = Request.GetRequestStream();
            RequestStream.Write(ByteData, 0, ByteData.Length);
        }

        /// <summary>
        /// Sets up any credentials (basic authentication, for OAuth, please use the OAuth class to
        /// create the URL)
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupCredentials(HttpWebRequest Request)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                Request.Credentials = new NetworkCredential(UserName, Password);
        }
#endif

#if NET
        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Handler">The http client handler object</param>
        /// <param name="Request">The request message object</param>
        /// <returns>The string returned by the service</returns>
        private static string SendRequest([NotNull] HttpClientHandler Handler, [NotNull] HttpRequestMessage Request)
        {
            if (Handler == null) throw new ArgumentNullException(nameof(Handler));
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            using HttpClient client = new(Handler);
            HttpResponseMessage Response = client.SendAsync(Request).Result;
            if (Response.StatusCode != HttpStatusCode.OK)
                return "";
            using StreamReader Reader = new(Response.Content.ReadAsStreamAsync().Result);
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The request message object</param>
        /// <param name="Data">Data to send with the request</param>
        private static void SetupData([NotNull] HttpRequestMessage Request, string Data)
        {
            if (Request == null) throw new ArgumentNullException(nameof(Request));
            if (string.IsNullOrEmpty(Data))
            {
                Request.Headers.Add("Content-Length", "0");
                return;
            }
            var ByteData = Data.ToByteArray();
            Request.Headers.Add("Content-Length", ByteData.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
            Request.Content = new ByteArrayContent(ByteData);
        }

        /// <summary>
        /// Sets up any credentials (basic authentication, for OAuth, 
        /// please use the OAuth class to create the URL)
        /// </summary>
        /// <param name="Handler">The http client handler object</param>
        private void SetupCredentials(HttpClientHandler Handler)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                Handler.Credentials = new NetworkCredential(UserName, Password);
        }
#endif
    }
}