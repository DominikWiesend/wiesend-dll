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

namespace Wiesend.IoC.Interfaces.Contracts
{
    /// <summary>
    /// IBootstrapper contract class
    /// </summary>
    //[ContractClassFor(typeof(IBootstrapper))]
    internal abstract class IBootstrapperContract : IBootstrapper
    {
        /// <summary>
        /// Name of the bootstrapper
        /// </summary>
        [NotNull]
        public string Name
        {
            get
            {
                var result = "";
                if (string.IsNullOrEmpty(result)) throw new System.InvalidOperationException("Contract assertion not met: !string.IsNullOrEmpty(result)");
                return result;
            }
        }

        /// <summary>
        /// Adds the assembly.
        /// </summary>
        /// <param name="Assemblies">The assemblies.</param>
        public void AddAssembly([NotNull] params Assembly[] Assemblies)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Registers an object with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to register</param>
        /// <param name="Name">Name associated with the object</param>
        public void Register<T>(T Object, [NotNull] string Name = "") where T : class
        {
            if (Name == null) throw new ArgumentNullException(nameof(Name));
        }

        /// <summary>
        /// Registers a type with the default constructor
        /// </summary>
        /// <typeparam name="T">Object type to register</typeparam>
        /// <param name="Name">Name associated with the object</param>
        public void Register<T>([NotNull] string Name = "") where T : class, new()
        {
            if (Name == null) throw new ArgumentNullException(nameof(Name));
        }

        /// <summary>
        /// Registers a type with the default constructor of a child class
        /// </summary>
        /// <typeparam name="T1">Base class/interface type</typeparam>
        /// <typeparam name="T2">Child class type</typeparam>
        /// <param name="Name">Name associated with the object</param>
        public void Register<T1, T2>([NotNull] string Name = "")
            where T1 : class
            where T2 : class, T1
        {
            if (Name == null) throw new ArgumentNullException(nameof(Name));
        }

        /// <summary>
        /// Registers a type with a function
        /// </summary>
        /// <typeparam name="T">Type that the function returns</typeparam>
        /// <param name="Function">Function to register with the type</param>
        /// <param name="Name">Name associated with the object</param>
        public void Register<T>([NotNull] Func<T> Function, string Name = "") where T : class
        {
            if (Function == null) throw new ArgumentNullException(nameof(Function));
        }

        /// <summary>
        /// Registers all objects of a certain type with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        public void RegisterAll<T>() where T : class
        {
        }

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="DefaultObject">Default object to return if the type can not be resolved</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public T Resolve<T>(T DefaultObject = default) where T : class
        {
            return default;
        }

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <param name="Name">Name associated with the object</param>
        /// <param name="DefaultObject">Default object to return if the type can not be resolved</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public T Resolve<T>([NotNull] string Name, T DefaultObject = default) where T : class
        {
            if (Name == null) throw new ArgumentNullException(nameof(Name));
            return default;
        }

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="DefaultObject">Default object to return if the type can not be resolved</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public object Resolve([NotNull] Type ObjectType, object DefaultObject = null)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            return default;
        }

        /// <summary>
        /// Resolves the object based on the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Name">Name associated with the object</param>
        /// <param name="DefaultObject">Default object to return if the type can not be resolved</param>
        /// <returns>
        /// An object of the specified type
        /// </returns>
        public object Resolve([NotNull] Type ObjectType, [NotNull] string Name, object DefaultObject = null)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            if (Name == null) throw new ArgumentNullException(nameof(Name));
            return default;
        }

        /// <summary>
        /// Resolves the objects based on the type specified
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>
        /// A list of objects of the specified type
        /// </returns>
        [NotNull]
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            var result = new List<T>() ?? throw new InvalidOperationException("Contract assertion not met: result != null");
            return result;
        }

        /// <summary>
        /// Resolves all objects based on the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>
        /// A list of objects of the specified type
        /// </returns>
        [NotNull]
        public IEnumerable<object> ResolveAll([NotNull] Type ObjectType)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            var result = new List<object>() ?? throw new InvalidOperationException("Contract assertion not met: result != null");
            return result;
        }
    }
}