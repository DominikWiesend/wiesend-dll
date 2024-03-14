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
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Wiesend.DataTypes;
using Wiesend.IO.FileFormats.BaseClasses;

namespace Wiesend.IO.FileFormats
{
    /// <summary>
    /// Class for helping with INI files
    /// </summary>
    public class INI : StringFormatBase<INI>
    {
        private string _FileName = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public INI()
        {
            InternalLoad("");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileName">Name of the file</param>
        public INI(string FileName)
        {
            this.FileName = FileName;
        }

        /// <summary>
        /// Name of the file
        /// </summary>
        public virtual string FileName
        {
            get { return _FileName; }
            set { _FileName = value; InternalLoad(_FileName); }
        }

        private Dictionary<string, Dictionary<string, string>> FileContents { get; set; }

        /// <summary>
        /// Deletes a section from the INI file
        /// </summary>
        /// <param name="Section">Section to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool DeleteFromINI(string Section)
        {
            bool ReturnValue = false;
            if (FileContents.ContainsKey(Section))
            {
                ReturnValue = FileContents.Remove(Section);
                Save(_FileName);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Deletes a key from the INI file
        /// </summary>
        /// <param name="Section">Section the key is under</param>
        /// <param name="Key">Key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool DeleteFromINI(string Section, string Key)
        {
            bool ReturnValue = false;
            if (FileContents.ContainsKey(Section) && FileContents.GetValue(Section).ContainsKey(Key))
            {
                ReturnValue = FileContents.GetValue(Section).Remove(Key);
                Save(_FileName);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Reads a value from an INI file
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="DefaultValue">Default value if it does not exist</param>
        public virtual string ReadFromINI(string Section, string Key, string DefaultValue = "")
        {
            if (FileContents.ContainsKey(Section) && FileContents.GetValue(Section).ContainsKey(Key))
                return FileContents.GetValue(Section).GetValue(Key);
            return DefaultValue;
        }

        /// <summary>
        /// Convert the INI to a string
        /// </summary>
        /// <returns>The INI file as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (string Header in FileContents.Keys)
            {
                Builder.Append("[" + Header + "]\r\n");
                foreach (string Key in FileContents[Header].Keys)
                    Builder.Append(Key + "=" + FileContents[Header][Key] + "\r\n");
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Returns an XML representation of the INI file
        /// </summary>
        /// <returns>An XML representation of the INI file</returns>
        public virtual string ToXML()
        {
            if (string.IsNullOrEmpty(this.FileName))
                return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<INI>\r\n</INI>";
            var Builder = new StringBuilder();
            Builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            Builder.Append("<INI>\r\n");
            foreach (string Header in FileContents.Keys)
            {
                Builder.Append("<section name=\"" + Header + "\">\r\n");
                foreach (string Key in FileContents[Header].Keys)
                    Builder.Append("<key name=\"" + Key + "\">" + FileContents[Header][Key] + "</key>\r\n");
                Builder.Append("</section>\r\n");
            }
            Builder.Append("</INI>");
            return Builder.ToString();
        }

        /// <summary>
        /// Writes a change to an INI file
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        public virtual void WriteToINI(string Section, string Key, string Value)
        {
            if (FileContents.ContainsKey(Section))
            {
                if (FileContents.GetValue(Section).ContainsKey(Key))
                    FileContents.GetValue(Section).SetValue(Key, Value);
                else
                    FileContents.GetValue(Section).Add(Key, Value);
            }
            else
            {
                var TempDictionary = new Dictionary<string, string> { { Key, Value } };
                FileContents.Add(Section, TempDictionary);
            }
            Save(_FileName);
        }

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            FileContents = new Dictionary<string, Dictionary<string, string>>();
            string Contents = Data;
            var Section = new Regex("[" + Regex.Escape(" ") + "\t]*" + Regex.Escape("[") + ".*" + Regex.Escape("]\r\n"));
            var Sections = Section.Split(Contents);
            var SectionHeaders = Section.Matches(Contents);
            int Counter = 1;
            foreach (Match SectionHeader in SectionHeaders)
            {
                string[] Splitter = { "\r\n" };
                string[] Splitter2 = { "=" };
                var Items = Sections[Counter].Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
                var SectionValues = new Dictionary<string, string>();
                foreach (string Item in Items)
                {
                    var ItemSplit = Item.Split(Splitter2, StringSplitOptions.None);
                    if (ItemSplit.Length >= 2)
                        SectionValues.Add(ItemSplit[0], ItemSplit[1]);
                }
                FileContents.Add(SectionHeader.Value.Replace("[", "").Replace("]\r\n", ""), SectionValues);
                ++Counter;
            }
        }
    }
}