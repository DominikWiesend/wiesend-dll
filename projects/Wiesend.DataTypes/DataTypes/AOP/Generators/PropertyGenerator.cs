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
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using Wiesend.DataTypes.AOP.Generators.BaseClasses;
using Wiesend.DataTypes.AOP.Interfaces;

namespace Wiesend.DataTypes.AOP.Generators
{
    /// <summary>
    /// Property generator
    /// </summary>
    /// <seealso cref="Wiesend.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class PropertyGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGenerator"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        public PropertyGenerator(PropertyInfo propertyInfo)
        {
            Contract.Requires(propertyInfo != null);
            PropertyInfo = propertyInfo;
            DeclaringType = PropertyInfo.DeclaringType;
        }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public Type DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the property information.
        /// </summary>
        /// <value>The property information.</value>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <param name="aspects">The aspects.</param>
        /// <returns>The string version of this property</returns>
        public string Generate(List<Assembly> assembliesUsing, IEnumerable<IAspect> aspects)
        {
            aspects = aspects ?? new List<IAspect>();
            var Builder = new StringBuilder();
            var GetMethodInfo = PropertyInfo.GetGetMethod();
            var SetMethodInfo = PropertyInfo.GetSetMethod();
            if (assembliesUsing != null)
                assembliesUsing.AddIfUnique(GetAssemblies(PropertyInfo.PropertyType));
            if (GetMethodInfo != null && SetMethodInfo != null)
            {
                Builder.AppendLineFormat(@"
        {0}
        {{
            get
            {{
                {1}
            }}
            set
            {{
                {2}
            }}
        }}

        {3}",
                                            ToString(),
                                            SetupMethod(DeclaringType, GetMethodInfo, aspects),
                                            SetupMethod(DeclaringType, SetMethodInfo, aspects),
                                            CreateBackingField(GetMethodInfo.IsAbstract | DeclaringType.IsInterface));
            }
            else if (GetMethodInfo != null)
            {
                Builder.AppendLineFormat(@"
        {0}
        {{
            get
            {{
                {1}
            }}
        }}",
                                            ToString(),
                                            SetupMethod(DeclaringType, GetMethodInfo, aspects));
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var Method = PropertyInfo.GetGetMethod() ?? PropertyInfo.GetSetMethod();
            return string.Format(@"{0} {1} {2} {3}",
                "public",
                (Method.IsAbstract | Method.IsVirtual) & !DeclaringType.IsInterface ? "override" : "",
                Method.ReturnType.GetName(),
                PropertyInfo.Name);
        }

        private string CreateBackingField(bool v)
        {
            if (!v)
                return "";
            return string.Format("{0} {1};\r\n", PropertyInfo.PropertyType.GetName(), "_" + PropertyInfo.Name);
        }

        private string SetupMethod(Type type, MethodInfo methodInfo, IEnumerable<IAspect> aspects)
        {
            if (methodInfo == null)
                return "";
            var Builder = new StringBuilder();
            var BaseMethodName = methodInfo.Name.Replace("get_", "").Replace("set_", "");
            string ReturnValue = methodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = "";
            if (!methodInfo.IsAbstract & !DeclaringType.IsInterface)
            {
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName : ReturnValue + "=base." + BaseMethodName;
            }
            else
            {
                BaseCall += (string.IsNullOrEmpty(ReturnValue) ? "_" : ReturnValue + "=_") + PropertyInfo.Name;
            }
            var Parameters = methodInfo.GetParameters();
            BaseCall += Parameters.Length > 0 ? "=" + Parameters.ToString(x => x.Name) + ";" : ";";
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
                methodInfo.ReturnType != typeof(void) ? methodInfo.ReturnType.GetName() + " " + ReturnValue + ";" : "",
                aspects.ForEach(x => x.SetupStartMethod(methodInfo, type)).ToString(x => x, "\r\n"),
                BaseCall,
                aspects.ForEach(x => x.SetupEndMethod(methodInfo, type, ReturnValue)).ToString(x => x, "\r\n"),
                string.IsNullOrEmpty(ReturnValue) ? "" : "return " + ReturnValue + ";",
                aspects.ForEach(x => x.SetupExceptionMethod(methodInfo, type)).ToString(x => x, "\r\n"));
            return Builder.ToString();
        }
    }
}