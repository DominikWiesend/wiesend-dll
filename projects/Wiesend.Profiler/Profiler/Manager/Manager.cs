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
using System;
using System.Collections.Generic;
using System.Linq;
using Wiesend.DataTypes.Patterns.BaseClasses;
using Wiesend.Profiler.Manager.Interfaces;

namespace Wiesend.Profiler.Manager
{
    /// <summary>
    /// Profiler manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Profilers">The profilers.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        public Manager([NotNull] IEnumerable<IProfiler> Profilers)
        {
            if (Profilers == null) throw new ArgumentNullException(nameof(Profilers));
            Profiler = Profilers.FirstOrDefault(x => !x.GetType().Namespace.StartsWith("WIESEND", StringComparison.OrdinalIgnoreCase));
            if (Profiler == null)
                Profiler = Profilers.FirstOrDefault(x => x.GetType().Namespace.StartsWith("WIESEND", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Root profiler object
        /// </summary>
        protected IProfiler Profiler { get; private set; }

        /// <summary>
        /// Starts the profiler and uses the name specified
        /// </summary>
        /// <param name="Name">Name of the entry</param>
        /// <returns>An IDisposable object that will stop the profiler when disposed of</returns>
        public static IDisposable Profile(string Name)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Profiler.Profile(Name);
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>Starts profiling</returns>
        public static IDisposable StartProfiling()
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Profiler.StartProfiling();
        }

        /// <summary>
        /// Ends profiling
        /// </summary>
        /// <param name="DiscardResults">Determines if the results should be discarded</param>
        /// <returns>Result of the profiling</returns>
        public static IResult StopProfiling(bool DiscardResults)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Profiler.StopProfiling(DiscardResults);
        }

        /// <summary>
        /// Outputs the profiler information as a string
        /// </summary>
        /// <returns>The profiler information as a string</returns>
        public override string ToString()
        {
            return "Profilers: " + Profiler.ToString() + "\r\n";
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (Profiler != null)
            {
                Profiler.Dispose();
                Profiler = null;
            }
        }
    }
}