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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Process extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ProcessExtensions
    {
        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="Process">Process to get information about</param>
        /// <param name="HTMLFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this Process Process, bool HTMLFormat = true)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            var Builder = new StringBuilder();
            return Builder.Append(HTMLFormat ? "<strong>" : "")
                   .Append(Process.ProcessName)
                   .Append(" Information")
                   .Append(HTMLFormat ? "</strong><br />" : "\n")
                   .Append(Process.ToString(HTMLFormat))
                   .Append(HTMLFormat ? "<br />" : "\n")
                   .ToString();
        }

        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="Processes">Processes to get information about</param>
        /// <param name="HTMLFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this IEnumerable<Process> Processes, bool HTMLFormat = true)
        {
            if (Processes == null)
                return "";
            var Builder = new StringBuilder();
            Processes.ForEach(x => Builder.Append(x.GetInformation(HTMLFormat)));
            return Builder.ToString();
        }

        /// <summary>
        /// Kills a process
        /// </summary>
        /// <param name="Process">Process that should be killed</param>
        /// <param name="TimeToKill">Amount of time (in ms) until the process is killed.</param>
        public static void KillProcessAsync(this Process Process, int TimeToKill = 0)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            ThreadPool.QueueUserWorkItem(delegate { KillProcessAsyncHelper(Process, TimeToKill); });
        }

        /// <summary>
        /// Kills a list of processes
        /// </summary>
        /// <param name="Processes">Processes that should be killed</param>
        /// <param name="TimeToKill">Amount of time (in ms) until the processes are killed.</param>
        public static void KillProcessAsync(this IEnumerable<Process> Processes, int TimeToKill = 0)
        {
            Contract.Requires<ArgumentNullException>(Processes != null, "Processes");
            Processes.ForEach(x => ThreadPool.QueueUserWorkItem(delegate { KillProcessAsyncHelper(x, TimeToKill); }));
        }

        /// <summary>
        /// Kills a process asyncronously
        /// </summary>
        /// <param name="Process">Process to kill</param>
        /// <param name="TimeToKill">Amount of time until the process is killed</param>
        private static void KillProcessAsyncHelper(Process Process, int TimeToKill)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            if (TimeToKill > 0)
                Thread.Sleep(TimeToKill);
            Process.Kill();
        }
    }
}