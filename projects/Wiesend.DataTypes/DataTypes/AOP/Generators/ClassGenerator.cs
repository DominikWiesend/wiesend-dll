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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wiesend.DataTypes.AOP.Generators.BaseClasses;
using Wiesend.DataTypes.AOP.Interfaces;

namespace Wiesend.DataTypes.AOP.Generators
{
    /// <summary>
    /// Class generator interface
    /// </summary>
    /// <seealso cref="Wiesend.DataTypes.AOP.Interfaces.IClassGenerator"/>
    public class ClassGenerator : GeneratorBase, IClassGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassGenerator"/> class.
        /// </summary>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <param name="aspects">The aspects.</param>
        public ClassGenerator(Type declaringType, ConcurrentBag<IAspect> aspects)
        {
            Aspects = aspects;
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets the aspects.
        /// </summary>
        /// <value>The aspects.</value>
        protected ConcurrentBag<IAspect> Aspects { get; private set; }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        protected Type DeclaringType { get; private set; }

        /// <summary>
        /// Generates the specified type.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <returns>The string representation of the generated class</returns>
        public string Generate(string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing)
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat(@"namespace {1}
{{
    {0}

    public class {2} : {3}{4} {5}
    {{
", usings.ToString(x => "using " + x + ";", "\r\n"),
 @namespace,
 className,
 DeclaringType.FullName.Replace("+", "."),
 interfaces.Count > 0 ? "," : "", interfaces.ToString(x => x.Name));
            if (DeclaringType.HasDefaultConstructor() || DeclaringType.IsInterface)
            {
                Builder.AppendLine(new ConstructorGenerator(DeclaringType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                                                         .FirstOrDefault(x => x.GetParameters().Length == 0),
                                                            DeclaringType)
                                   .Generate(assembliesUsing, Aspects));
            }

            Aspects.ForEach(x => Builder.AppendLine(x.SetupInterfaces(DeclaringType)));

            Type TempType = DeclaringType;
            var MethodsAlreadyDone = new List<string>();
            while (TempType != null)
            {
                foreach (PropertyInfo Property in TempType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    var GetMethodInfo = Property.GetGetMethod();
                    var SetMethodInfo = Property.GetSetMethod();
                    if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && !MethodsAlreadyDone.Contains("set_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo != null
                        && SetMethodInfo.IsPublic
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                    else if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo == null
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                    }
                    else
                    {
                        if (GetMethodInfo != null)
                            MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        if (SetMethodInfo != null)
                            MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                }
                foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                                                        .Where(x => !MethodsAlreadyDone.Contains(x.Name)
                                                            && x.IsVirtual
                                                            && !x.IsFinal
                                                            && !x.IsPrivate
                                                            && !x.Name.StartsWith("add_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.Name.StartsWith("remove_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.IsGenericMethod))
                {
                    Builder.AppendLine(new MethodGenerator(Method).Generate(assembliesUsing, Aspects));
                    MethodsAlreadyDone.Add(Method.Name);
                }
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            Builder.AppendLine(@"   }
}");
            return Builder.ToString();
        }
    }
}