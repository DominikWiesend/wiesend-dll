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

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Wiesend.IoC.BaseClasses;
using Wiesend.IoC.Default.Interfaces;

namespace Wiesend.IoC.Default
{
    /// <summary>
    /// Default bootstrapper if one isn't found
    /// </summary>
    public class DefaultBootstrapper : BootstrapperBase<IDictionary<Tuple<Type, string>, ITypeBuilder>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="types">The types.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1829:Use Length/Count property instead of Count() when available", Justification = "<Pending>")]
        public DefaultBootstrapper(IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
            : base(assemblies, types)
        {
            _AppContainer = new ConcurrentDictionary<Tuple<Type, string>, ITypeBuilder>();
            GenericRegisterMethod = GetType().GetMethods().First(x => x.Name == "Register" && x.GetGenericArguments().Count() == 2);
            GenericResolveMethod = GetType().GetMethods().First(x => x.Name == "Resolve" && x.IsGenericMethod && x.GetParameters().Length == 1);
#if NET45
            GenericResolveAllMethod = GetType().GetMethod("ResolveAll", new Type[] { });
#else
            GenericResolveAllMethod = GetType().GetMethod("ResolveAll", Array.Empty<Type>());
#endif
        }

        /// <summary>
        /// Name of the bootstrapper
        /// </summary>
        public override string Name
        {
            get { return "Default bootstrapper"; }
        }

        /// <summary>
        /// App container
        /// </summary>
        protected override IDictionary<Tuple<Type, string>, ITypeBuilder> AppContainer
        {
            get { return _AppContainer; }
        }

        /// <summary>
        /// Gets or sets the generic register method.
        /// </summary>
        /// <value>The generic register method.</value>
        private MethodInfo GenericRegisterMethod { get; set; }

        /// <summary>
        /// Gets or sets the generic resolve all method.
        /// </summary>
        /// <value>The generic resolve all method.</value>
        private MethodInfo GenericResolveAllMethod { get; set; }

        /// <summary>
        /// Gets or sets the generic resolve method.
        /// </summary>
        /// <value>The generic resolve method.</value>
        private MethodInfo GenericResolveMethod { get; set; }

        private ConcurrentDictionary<Tuple<Type, string>, ITypeBuilder> _AppContainer;

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Object">Object to return</param>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T>(T Object, string Name = "")
        {
            Register<T>(() => Object, Name);
        }

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T>(string Name = "")
        {
            Register<T, T>(Name);
        }

        /// <summary>
        /// Registers two types together
        /// </summary>
        /// <typeparam name="T1">Interface/base class</typeparam>
        /// <typeparam name="T2">Implementation</typeparam>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T1, T2>(string Name = "")
        {
            Type Type = typeof(T2);
            Register<T1>(() =>
            {
                var Constructor = FindConstructor(Type);
                if (Constructor != null)
                    return (T1)Activator.CreateInstance(Type, GetParameters(Constructor).ToArray());
                return null;
            }, Name);
        }

        /// <summary>
        /// Registers a function with a type
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Function">Function used to create the type</param>
        /// <param name="Name">Name to associate with the function</param>
        public override void Register<T>(Func<T> Function, string Name = "")
        {
            var Key = new Tuple<Type, string>(typeof(T), Name);
            _AppContainer.AddOrUpdate(Key,
                x => new TypeBuilder<T>(Function),
                (x, y) => new TypeBuilder<T>(Function));
        }

        /// <summary>
        /// Registers all objects of a certain type with the bootstrapper
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        public override void RegisterAll<T>()
        {
            foreach (Type Type in Types.Where(x => IsOfType(x, typeof(T))
                                                    && x.IsClass
                                                    && !x.IsAbstract
                                                    && !x.ContainsGenericParameters))
            {
                GenericRegisterMethod.MakeGenericMethod(typeof(T), Type)
                    .Invoke(this, new object[] { Types.Count == 1 ? "" : Type.FullName });
                GenericRegisterMethod.MakeGenericMethod(Type, Type)
                    .Invoke(this, new object[] { "" });
            }
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override T Resolve<T>(T DefaultObject = default)
        {
            return (T)Resolve(typeof(T), "", DefaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <param name="Name">Name of the object to return</param>
        /// <returns>Object of the type specified</returns>
        public override T Resolve<T>(string Name, T DefaultObject = default)
        {
            return (T)Resolve(typeof(T), Name, DefaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override object Resolve(Type ObjectType, object DefaultObject = null)
        {
            return Resolve(ObjectType, "", DefaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="Name">Name of the object to return</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override object Resolve(Type ObjectType, string Name, object DefaultObject = null)
        {
            try
            {
                var Key = new Tuple<Type, string>(ObjectType, Name);
                return _AppContainer.TryGetValue(Key, out ITypeBuilder Builder) ? Builder.Create() : DefaultObject;
            }
            catch { return DefaultObject; }
        }

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <typeparam name="T">Type of objects to return</typeparam>
        /// <returns>An IEnumerable containing all objects of the type specified</returns>
        public override IEnumerable<T> ResolveAll<T>()
        {
            return ResolveAll(typeof(T)).Select(x => (T)x);
        }

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <returns>An IEnumerable containing all objects of the type specified</returns>
        public override IEnumerable<object> ResolveAll(Type ObjectType)
        {
            var ReturnValues = new ConcurrentBag<object>();
            foreach (Tuple<Type, string> Key in _AppContainer.Keys.Where(x => x.Item1 == ObjectType))
                ReturnValues.Add(Resolve(Key.Item1, Key.Item2, null));
            return ReturnValues;
        }

        /// <summary>
        /// Converts the bootstrapper to a string
        /// </summary>
        /// <returns>String version of the bootstrapper</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (Tuple<Type, string> Key in AppContainer.Keys)
                Builder.Append(AppContainer[Key].ToString());
            return Builder.ToString();
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Not used</param>
        protected override void Dispose(bool Managed)
        {
            if (_AppContainer != null)
            {
                foreach (IDisposable Item in _AppContainer.Values.Where(x => IsOfType(x.ReturnType, typeof(IDisposable))).Reverse().Select(x => x.Create()).Cast<IDisposable>())
                    Item.Dispose();
                _AppContainer.Clear();
                _AppContainer = null;
            }
            base.Dispose(Managed);
        }

        /// <summary>
        /// Finds the constructor.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>The constructor that should be used</returns>
        private ConstructorInfo FindConstructor(Type Type)
        {
            if (Type == null)
                throw new ArgumentNullException(nameof(Type));

            var Constructors = Type.GetConstructors();
            ConstructorInfo Constructor = null;
            foreach (ConstructorInfo TempConstructor in Constructors.OrderByDescending(x => x.GetParameters().Length))
            {
                bool Found = true;
                foreach (ParameterInfo Parameter in TempConstructor.GetParameters())
                {
                    Type ParameterType = Parameter.ParameterType;
                    if (Parameter.ParameterType.GetInterfaces().Contains(typeof(IEnumerable)) && Parameter.ParameterType.IsGenericType)
                    {
                        ParameterType = ParameterType.GetGenericArguments().First();
                        if (!AppContainer.Keys.Any(x => x.Item1 == ParameterType))
                        {
                            Found = false;
                            break;
                        }
                    }
                    else if (!this.AppContainer.ContainsKey(new Tuple<Type, string>(ParameterType, "")))
                    {
                        Found = false;
                        break;
                    }
                }
                if (Found)
                {
                    Constructor = TempConstructor;
                    break;
                }
            }
            return Constructor;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="Constructor">The constructor.</param>
        /// <returns>The parameters</returns>
        private List<object> GetParameters(ConstructorInfo Constructor)
        {
            if (Constructor == null)
                throw new ArgumentNullException(nameof(Constructor));

            var Params = new List<object>();
            foreach (ParameterInfo Parameter in Constructor.GetParameters())
            {
                if (Parameter.ParameterType.GetInterfaces().Contains(typeof(IEnumerable)) && Parameter.ParameterType.IsGenericType)
                {
                    var GenericParamType = Parameter.ParameterType.GetGenericArguments().First();
#if NET45
                    Params.Add(GenericResolveAllMethod.MakeGenericMethod(GenericParamType).Invoke(this, new object[] { }));
#else
                    Params.Add(GenericResolveAllMethod.MakeGenericMethod(GenericParamType).Invoke(this, Array.Empty<object>()));
#endif
                }
                else
                {
                    Params.Add(GenericResolveMethod.MakeGenericMethod(Parameter.ParameterType)
                                                   .Invoke(this, new object[] {
                                                                    Parameter.ParameterType.IsValueType ? Activator.CreateInstance(Parameter.ParameterType) : null
                                                   }));
                }
            }
            return Params;
        }

        private bool IsOfType(Type x, Type type)
        {
            if (x == typeof(object) || x == null)
                return false;
            if (x == type || x.GetInterfaces().Any(y => y == type))
                return true;
            return IsOfType(x.BaseType, type);
        }
    }
}