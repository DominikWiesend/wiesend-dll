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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Extension methods related to the stack trace
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StackTraceExtensions
    {
        /// <summary>
        /// Gets the methods involved in the stack trace
        /// </summary>
        /// <param name="Stack">Stack trace to get methods from</param>
        /// <param name="ExcludedAssemblies">Excludes methods from the specified assemblies</param>
        /// <returns>A list of methods involved in the stack trace</returns>
        public static IEnumerable<MethodBase> GetMethods([NotNull] this StackTrace Stack, params Assembly[] ExcludedAssemblies)
        {
            if (Stack == null) throw new ArgumentNullException(nameof(Stack));
            return Stack.GetFrames().GetMethods(ExcludedAssemblies);
        }

        /// <summary>
        /// Gets the methods involved in the individual frames
        /// </summary>
        /// <param name="Frames">Frames to get the methods from</param>
        /// <param name="ExcludedAssemblies">Excludes methods from the specified assemblies</param>
        /// <returns>The list of methods involved</returns>
        public static IEnumerable<MethodBase> GetMethods(this IEnumerable<StackFrame> Frames, params Assembly[] ExcludedAssemblies)
        {
            var Methods = new List<MethodBase>();
            if (Frames == null)
                return Methods;
            foreach (StackFrame Frame in Frames)
            {
                Methods.AddIf(x => x.DeclaringType != null
                    && !ExcludedAssemblies.Contains(x.DeclaringType.Assembly)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("System", StringComparison.InvariantCulture)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("mscorlib", StringComparison.InvariantCulture)
                    && !x.DeclaringType.Assembly.FullName.StartsWith("WebDev.WebHost40", StringComparison.InvariantCulture),
                        Frame.GetMethod());
            }
            return Methods;
        }
    }
}