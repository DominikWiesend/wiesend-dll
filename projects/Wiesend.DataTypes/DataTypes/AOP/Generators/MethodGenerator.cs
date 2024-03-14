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
using System.Reflection;
using System.Text;
using Wiesend.DataTypes.AOP.Generators.BaseClasses;
using Wiesend.DataTypes.AOP.Interfaces;

namespace Wiesend.DataTypes.AOP.Generators
{
    /// <summary>
    /// Method generator
    /// </summary>
    /// <seealso cref="Wiesend.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class MethodGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodGenerator"/> class.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        public MethodGenerator([NotNull] MethodInfo methodInfo)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            DeclaringType = MethodInfo.DeclaringType;
        }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        private Type DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the method information.
        /// </summary>
        /// <value>The method information.</value>
        private MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <param name="aspects">The aspects.</param>
        /// <returns>The generated string of the method</returns>
        public string Generate(List<Assembly> assembliesUsing, IEnumerable<IAspect> aspects)
        {
            aspects ??= new List<IAspect>();
            var Builder = new StringBuilder();
            assembliesUsing?.AddIfUnique(GetAssemblies(MethodInfo.ReturnType));
            Builder.AppendLineFormat(@"
        {0}
        {{
            {1}
        }}",
            ToString(),
            SetupMethod(aspects));
            return Builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public override string ToString()
        {
            return string.Format(@"{4} {5} {0} {1} {2}({3})",
                MethodInfo.IsStatic ? "static" : "",
                MethodInfo.ReturnType.GetName(),
            MethodInfo.Name,
            MethodInfo.GetParameters().ToString(x => new ParameterGenerator(x).Generate(null)),
            "public",
            (MethodInfo.IsAbstract | MethodInfo.IsVirtual) & !DeclaringType.IsInterface ? "override" : "");
        }

        private string SetupMethod(IEnumerable<IAspect> aspects)
        {
            if (MethodInfo == null)
                return "";
            var Builder = new StringBuilder();
            string BaseMethodName = MethodInfo.Name;
            string ReturnValue = MethodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = "";
            if (!MethodInfo.IsAbstract & !DeclaringType.IsInterface)
            {
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName + "(" : ReturnValue + "=base." + BaseMethodName + "(";
                var Parameters = MethodInfo.GetParameters();
                BaseCall += Parameters.Length > 0 ? Parameters.ToString(x => (x.IsOut ? "out " : "") + x.Name) : "";
                BaseCall += ");\r\n";
            }
            else if (!string.IsNullOrEmpty(ReturnValue))
            {
                BaseCall = ReturnValue + "=default(" + MethodInfo.ReturnType.Name + ");\r\n";
            }
            Builder.AppendLineFormat(@"
                try
                {{
                    {0}
                    {1}
                    {2}
                    {3}
                    {4}
                }}
                catch(Exception CaughtException)
                {{
                    {5}
                    throw;
                }}",
                MethodInfo.ReturnType != typeof(void) ? MethodInfo.ReturnType.GetName() + " " + ReturnValue + ";" : "",
                aspects.ForEach(x => x.SetupStartMethod(MethodInfo, DeclaringType)).ToString(x => x, "\r\n"),
                BaseCall,
                aspects.ForEach(x => x.SetupEndMethod(MethodInfo, DeclaringType, ReturnValue)).ToString(x => x, "\r\n"),
                string.IsNullOrEmpty(ReturnValue) ? "" : "return " + ReturnValue + ";",
                aspects.ForEach(x => x.SetupExceptionMethod(MethodInfo, DeclaringType)).ToString(x => x, "\r\n"));
            return Builder.ToString();
        }
    }
}