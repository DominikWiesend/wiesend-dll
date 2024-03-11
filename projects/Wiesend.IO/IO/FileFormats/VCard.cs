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

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Wiesend.IO.FileFormats.BaseClasses;

namespace Wiesend.IO.FileFormats
{
    /// <summary>
    /// Enum defining relationships (used for XFN markup)
    /// </summary>
    public enum Relationship
    {
        /// <summary>
        /// Friend
        /// </summary>
        Friend,

        /// <summary>
        /// Acquaintance
        /// </summary>
        Acquaintance,

        /// <summary>
        /// Contact
        /// </summary>
        Contact,

        /// <summary>
        /// Met
        /// </summary>
        Met,

        /// <summary>
        /// Coworker
        /// </summary>
        CoWorker,

        /// <summary>
        /// Colleague
        /// </summary>
        Colleague,

        /// <summary>
        /// Coresident
        /// </summary>
        CoResident,

        /// <summary>
        /// Neighbor
        /// </summary>
        Neighbor,

        /// <summary>
        /// Child
        /// </summary>
        Child,

        /// <summary>
        /// Parent
        /// </summary>
        Parent,

        /// <summary>
        /// Sibling
        /// </summary>
        Sibling,

        /// <summary>
        /// Spouse
        /// </summary>
        Spouse,

        /// <summary>
        /// Kin
        /// </summary>
        Kin,

        /// <summary>
        /// Muse
        /// </summary>
        Muse,

        /// <summary>
        /// Crush
        /// </summary>
        Crush,

        /// <summary>
        /// Date
        /// </summary>
        Date,

        /// <summary>
        /// Sweetheart
        /// </summary>
        Sweetheart,

        /// <summary>
        /// Me
        /// </summary>
        Me
    }

    /// <summary>
    /// VCard class
    /// </summary>
    public class VCard : StringFormatBase<VCard>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Constructor
        /// </summary>
        public VCard()
        {
            Relationships = new List<Relationship>();
        }

        /// <summary>
        /// Work phone number of the individual
        /// </summary>
        public string DirectDial { get; set; }

        /// <summary>
        /// Email of the individual
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Organization the person belongs to
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Relationship to the person (uses XFN)
        /// </summary>
        public ICollection<Relationship> Relationships { get; private set; }

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Title of the person
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Url to the person's site
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        protected string FullName
        {
            get
            {
                var Builder = new StringBuilder();
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Builder.AppendFormat("{0} ", Prefix);
                }
                Builder.AppendFormat("{0} ", FirstName);
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.AppendFormat("{0} ", MiddleName);
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.AppendFormat(" {0}", Suffix);
                }
                return Builder.ToString();
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        protected string Name
        {
            get
            {
                return new StringBuilder().AppendFormat(CultureInfo.CurrentCulture, "{0};{1};{2};{3};{4}", LastName, FirstName, MiddleName, Prefix, Suffix).ToString();
            }
        }

        /// <summary>
        /// Gets the hCard version of the vCard
        /// </summary>
        /// <returns>A hCard in string format</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public string HCard()
        {
            var Builder = new StringBuilder();
            Builder.Append("<div class=\"vcard\">");
            if (string.IsNullOrEmpty(Url))
            {
                Builder.AppendFormat("<div class=\"fn\">{0}</div>", FullName);
            }
            else
            {
                Builder.AppendFormat("<a class=\"fn url\" href=\"{0}\"", Url);
                if (Relationships.Count > 0)
                {
                    Builder.Append(" rel=\"");
                    foreach (Relationship Relationship in Relationships)
                    {
                        Builder.Append(Relationship.ToString()).Append(' ');
                    }
                    Builder.Append('\"');
                }
                Builder.AppendFormat(">{0}</a>", FullName);
            }
            Builder.AppendFormat("<span class=\"n\" style=\"display:none;\"><span class=\"family-name\">{0}</span><span class=\"given-name\">{1}</span></span>", LastName, FirstName);
            if (!string.IsNullOrEmpty(DirectDial))
            {
                Builder.AppendFormat("<div class=\"tel\"><span class=\"type\">Work</span> {0}</div>", DirectDial);
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Builder.AppendFormat("<div>Email: <a class=\"email\" href=\"mailto:{0}\">{0}</a></div>", StripHTML(Email));
            }
            if (!string.IsNullOrEmpty(Organization))
            {
                Builder.AppendFormat("<div>Organization: <span class=\"org\">{0}</span></div>", Organization);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                Builder.AppendFormat("<div>Title: <span class=\"title\">{0}</span></div>", Title);
            }
            Builder.Append("</div>");
            return Builder.ToString();
        }

        /// <summary>
        /// Gets the VCard as a string
        /// </summary>
        /// <returns>VCard as a string</returns>
        public override string ToString()
        {
            return new StringBuilder().Append("BEGIN:VCARD\r\nVERSION:2.1\r\n")
                .AppendFormat(CultureInfo.CurrentCulture, "FN:{0}\r\n", FullName)
                .AppendFormat(CultureInfo.CurrentCulture, "N:{0}\r\n", Name)
                .AppendFormat(CultureInfo.CurrentCulture, "TEL;WORK:{0}\r\n", DirectDial)
                .AppendFormat(CultureInfo.CurrentCulture, "EMAIL;INTERNET:{0}\r\n", StripHTML(Email))
                .AppendFormat(CultureInfo.CurrentCulture, "TITLE:{0}\r\n", Title)
                .AppendFormat(CultureInfo.CurrentCulture, "ORG:{0}\r\n", Organization)
                .AppendFormat(CultureInfo.CurrentCulture, "END:VCARD\r\n")
                .ToString();
        }

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            string Content = Data;
            foreach (Match TempMatch in Regex.Matches(Content, "(?<Title>[^:]+):(?<Value>.*)"))
            {
                switch (TempMatch.Groups["Title"].Value.ToUpperInvariant())
                {
                    case "N":
                        var Name = TempMatch.Groups["Value"].Value.Split(';');
                        if (Name.Length > 0)
                        {
                            LastName = Name[0];
                            if (Name.Length > 1)
                                FirstName = Name[1];
                            if (Name.Length > 2)
                                MiddleName = Name[2];
                            if (Name.Length > 3)
                                Prefix = Name[3];
                            if (Name.Length > 4)
                                Suffix = Name[4];
                        }

                        break;
                    case "TEL;WORK":
                        DirectDial = TempMatch.Groups["Value"].Value;
                        break;
                    case "EMAIL;INTERNET":
                        Email = TempMatch.Groups["Value"].Value;
                        break;
                    case "TITLE":
                        Title = TempMatch.Groups["Value"].Value;
                        break;
                    case "ORG":
                        Organization = TempMatch.Groups["Value"].Value;
                        break;
                }
            }
        }

        private static string StripHTML(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return string.Empty;

            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            HTML = HTML.Replace("&nbsp;", " ");
            return HTML.Replace("&#160;", string.Empty);
        }
    }
}