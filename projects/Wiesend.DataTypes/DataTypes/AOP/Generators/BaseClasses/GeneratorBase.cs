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
using System.Linq;
using System.Reflection;

namespace Wiesend.DataTypes.AOP.Generators.BaseClasses
{
    /// <summary>
    /// Generator base class
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorBase"/> class.
        /// </summary>
        protected GeneratorBase()
        {
        }

        /// <summary>
        /// Gets the assemblies associated with the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The assemblies associated with the type</returns>
        protected Assembly[] GetAssemblies(Type type)
        {
            var Types = new List<Assembly>();
            Type TempType = type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly.GetReferencedAssemblies().ForEach(x =>
                {
                    try
                    {
                        return Assembly.Load(x);
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(x => x != null));
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType.GetEvents().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.EventHandlerType)));
                TempType.GetFields().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.FieldType)));
                TempType.GetProperties().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.PropertyType)));
                TempType.GetMethods().ForEach(x =>
                {
                    Types.AddIfUnique(GetAssembliesSimple(x.ReturnType));
                    x.GetParameters().ForEach(y => Types.AddIfUnique(GetAssembliesSimple(y.ParameterType)));
                });
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }

        /// <summary>
        /// Gets the assemblies associated with the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The assemblies associated with the type</returns>
        protected Assembly[] GetAssembliesSimple(Type type)
        {
            var Types = new List<Assembly>();
            Type TempType = type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }
    }
}