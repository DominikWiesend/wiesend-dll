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
using System.ComponentModel;
using JetBrains.Annotations;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Wiesend.IO;
using Wiesend.Web.Streams;

namespace Wiesend.Web
{
    /// <summary>
    /// Set of HTML related extensions (and HTTP related)
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HTMLExtensions
    {
        private const string DEFLATE = "deflate";

        private const string GZIP = "gzip";

        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Returns the absolute root
        /// </summary>
        public static Uri AbsoluteRoot([NotNull] this HttpContextBase Context)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            if (Context.Items["absoluteurl"] == null)
                Context.Items["absoluteurl"] = new Uri(Context.Request.Url.GetLeftPart(UriPartial.Authority) + Context.RelativeRoot());
            return Context.Items["absoluteurl"] as Uri;
        }

        /// <summary>
        /// Returns the absolute root
        /// </summary>
        public static Uri AbsoluteRoot([NotNull] this HttpContext Context)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            if (Context.Items["absoluteurl"] == null)
                Context.Items["absoluteurl"] = new Uri(Context.Request.Url.GetLeftPart(UriPartial.Authority) + Context.RelativeRoot());
            return Context.Items["absoluteurl"] as Uri;
        }

        /// <summary>
        /// Adds a script file to the header of the current page
        /// </summary>
        /// <param name="File">Script file</param>
        /// <param name="Page">Page to add it to</param>
        public static void AddScriptFile([NotNull] this System.Web.UI.Page Page, [NotNull] FileInfo File)
        {
            if (File == null) throw new ArgumentNullException(nameof(File), "File");
            if (!(File.Exists)) throw new System.IO.FileNotFoundException(nameof(File), "File does not exist");
            if (Page == null) throw new ArgumentNullException(nameof(Page), "Page");
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(typeof(System.Web.UI.Page), File.FullName))
                Page.ClientScript.RegisterClientScriptInclude(typeof(System.Web.UI.Page), File.FullName, File.FullName);
        }

        /// <summary>
        /// Decides if the string contains HTML
        /// </summary>
        /// <param name="Input">Input string to check</param>
        /// <returns>false if it does not contain HTML, true otherwise</returns>
        public static bool ContainsHTML(this string Input)
        {
            return !string.IsNullOrEmpty(Input) && STRIP_HTML_REGEX.IsMatch(Input);
        }

        /// <summary>
        /// Decides if the file contains HTML
        /// </summary>
        /// <param name="Input">Input file to check</param>
        /// <returns>false if it does not contain HTML, true otherwise</returns>
        public static bool ContainsHTML([NotNull] this FileInfo Input)
        {
            if (Input == null) throw new ArgumentNullException(nameof(Input), "Input");
            return Input.Exists && Input.Read().ContainsHTML();
        }

        /// <summary>
        /// Adds HTTP compression to the current context
        /// </summary>
        /// <param name="Context">Current context</param>
        /// <param name="RemovePrettyPrinting">
        /// Sets the response filter to a special stream that removes pretty printing from content
        /// </param>
        /// <param name="Type">
        /// The minification type to use (defaults to HTML if RemovePrettyPrinting is set to true,
        /// but can also deal with CSS and Javascript)
        /// </param>
        public static void HTTPCompress([NotNull] this HttpContextBase Context, bool RemovePrettyPrinting = false, MinificationType Type = MinificationType.HTML)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            if (Context.Request.UserAgent != null && Context.Request.UserAgent.Contains("MSIE 6"))
                return;
            Context.Response.Filter = RemovePrettyPrinting ? (System.IO.Stream)new UglyStream(Context.Response.Filter, CompressionType.GZip, Type) : new GZipStream(Context.Response.Filter, CompressionMode.Compress);
        }

        /// <summary>
        /// Adds HTTP compression to the current context
        /// </summary>
        /// <param name="Context">Current context</param>
        /// <param name="RemovePrettyPrinting">
        /// Sets the response filter to a special stream that removes pretty printing from content
        /// </param>
        /// <param name="Type">
        /// The minification type to use (defaults to HTML if RemovePrettyPrinting is set to true,
        /// but can also deal with CSS and Javascript)
        /// </param>
        public static void HTTPCompress([NotNull] this HttpContext Context, bool RemovePrettyPrinting = false, MinificationType Type = MinificationType.HTML)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            if (Context.Request.UserAgent != null && Context.Request.UserAgent.Contains("MSIE 6"))
                return;
            Context.Response.Filter = RemovePrettyPrinting ? (System.IO.Stream)new UglyStream(Context.Response.Filter, CompressionType.GZip, Type) : new GZipStream(Context.Response.Filter, CompressionMode.Compress);
        }

        /// <summary>
        /// Checks the request headers to see if the specified encoding is accepted by the client.
        /// </summary>
        public static bool IsEncodingAccepted(this HttpContextBase Context, string Encoding)
        {
            if (Context == null)
                return false;
            return Context.Request.Headers["Accept-encoding"] != null && Context.Request.Headers["Accept-encoding"].Contains(Encoding);
        }

        /// <summary>
        /// Checks the request headers to see if the specified encoding is accepted by the client.
        /// </summary>
        public static bool IsEncodingAccepted(this HttpContext Context, string Encoding)
        {
            if (Context == null)
                return false;
            return Context.Request.Headers["Accept-encoding"] != null && Context.Request.Headers["Accept-encoding"].Contains(Encoding);
        }

        /// <summary>
        /// Gets the relative root of the web site
        /// </summary>
        /// <param name="Context">Current context</param>
        /// <returns>The relative root of the web site</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "Context")]
        public static string RelativeRoot(this HttpContextBase Context)
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        /// Gets the relative root of the web site
        /// </summary>
        /// <param name="Context">Current context</param>
        /// <returns>The relative root of the web site</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "Context")]
        public static string RelativeRoot(this HttpContext Context)
        {
            return VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        /// Removes illegal characters (used in uri's, etc.)
        /// </summary>
        /// <param name="Input">string to be converted</param>
        /// <returns>A stripped string</returns>
        public static string RemoveURLIllegalCharacters(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Input = Input.Replace(":", string.Empty)
                        .Replace("/", string.Empty)
                        .Replace("?", string.Empty)
                        .Replace("#", string.Empty)
                        .Replace("[", string.Empty)
                        .Replace("]", string.Empty)
                        .Replace("@", string.Empty)
                        .Replace(".", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("&", string.Empty)
                        .Replace("'", string.Empty)
                        .Replace(" ", "-");
            Input = RemoveExtraHyphen(Input);
            Input = RemoveDiacritics(Input);
            return Input.URLEncode().Replace("%", string.Empty);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="Encoding">Encoding to set</param>
        /// <param name="Context">Context to set the encoding on</param>
        public static void SetEncoding([NotNull] this HttpContextBase Context, string Encoding)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            Context.Response.AppendHeader("Content-encoding", Encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="Encoding">Encoding to set</param>
        /// <param name="Context">Context to set the encoding on</param>
        public static void SetEncoding([NotNull] this HttpContext Context, string Encoding)
        {
            if (Context == null) throw new ArgumentNullException(nameof(Context), "Context");
            Context.Response.AppendHeader("Content-encoding", Encoding);
        }

        /// <summary>
        /// Removes HTML elements from a string
        /// </summary>
        /// <param name="HTML">HTML laiden string</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML(this string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return "";
            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            return HTML.Replace("&nbsp;", " ")
                       .Replace("&#160;", string.Empty);
        }

        /// <summary>
        /// Removes HTML elements from a string
        /// </summary>
        /// <param name="HTML">HTML laiden file</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML([NotNull] this FileInfo HTML)
        {
            if (HTML == null) throw new ArgumentNullException(nameof(HTML), "HTML");
            if (!(HTML.Exists)) throw new System.IO.FileNotFoundException(nameof(HTML), "File does not exist");
            return HTML.Read().StripHTML();
        }

        /// <summary>
        /// URL decodes a string
        /// </summary>
        /// <param name="Input">Input to decode</param>
        /// <returns>A decoded string</returns>
        public static string URLDecode(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            return HttpUtility.UrlDecode(Input);
        }

        /// <summary>
        /// URL encodes a string
        /// </summary>
        /// <param name="Input">Input to encode</param>
        /// <returns>An encoded string</returns>
        public static string URLEncode(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            return HttpUtility.UrlEncode(Input);
        }

        /// <summary>
        /// Removes special characters (Diacritics) from the string
        /// </summary>
        /// <param name="Input">String to strip</param>
        /// <returns>Stripped string</returns>
        private static string RemoveDiacritics(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            var Normalized = Input.Normalize(NormalizationForm.FormD);
            var Builder = new StringBuilder();
            for (int i = 0; i < Normalized.Length; i++)
            {
                Char TempChar = Normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(TempChar) != UnicodeCategory.NonSpacingMark)
                    Builder.Append(TempChar);
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Removes extra hyphens from a string
        /// </summary>
        /// <param name="Input">string to be stripped</param>
        /// <returns>Stripped string</returns>
        private static string RemoveExtraHyphen(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            return new Regex(@"[-]{2,}", RegexOptions.None).Replace(Input, "-");
        }
    }
}