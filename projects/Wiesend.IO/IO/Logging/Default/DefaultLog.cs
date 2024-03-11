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
using System.Globalization;
using System.Web;
using Wiesend.IO.Logging.BaseClasses;
using Wiesend.IO.Logging.Enums;

namespace Wiesend.IO.Logging.Default
{
    /// <summary>
    /// Outputs messages to a file in ~/App_Data/Logs/ if a web app or ~/Logs/ if windows app with
    /// the format Name+DateTime.Now+".log"
    /// </summary>
    public class DefaultLog : LogBase<DefaultLog>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultLog(string Name)
            : base(Name)
        {
            File = new FileInfo(FileName);
            Start = x => File.Write("Logging started at " + DateTime.Now + Environment.NewLine);
            End = x => File.Write("Logging ended at " + DateTime.Now + Environment.NewLine, System.IO.FileMode.Append);
            Log.Add(MessageType.Debug, x => File.Write(x, System.IO.FileMode.Append));
            Log.Add(MessageType.Error, x => File.Write(x, System.IO.FileMode.Append));
            Log.Add(MessageType.General, x => File.Write(x, System.IO.FileMode.Append));
            Log.Add(MessageType.Info, x => File.Write(x, System.IO.FileMode.Append));
            Log.Add(MessageType.Trace, x => File.Write(x, System.IO.FileMode.Append));
            Log.Add(MessageType.Warn, x => File.Write(x, System.IO.FileMode.Append));
            FormatMessage = (Message, Type, args) => Type.ToString()
                + ": " + (args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, Message, args) : Message)
                + Environment.NewLine;
            Start(this);
        }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_FileName))
                {
                    _FileName = HttpContext.Current == null ?
                        "~/Logs/" + Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.CurrentCulture) + ".log" :
                        "~/App_Data/Logs/" + Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.CurrentCulture) + ".log";

                }
                return _FileName;
            }
        }

        /// <summary>
        /// File object that the log uses
        /// </summary>
        protected FileInfo File { get; private set; }

        private string _FileName = "";
    }
}