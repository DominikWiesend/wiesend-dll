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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Wiesend.IoC.Default;
using Wiesend.IoC.Interfaces;

namespace Wiesend.IoC
{
    /// <summary>
    /// IoC manager class
    /// </summary>
    public class Manager : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Manager()
        {
            var LoadedAssemblies = LoadAssemblies();
            var LoadedTypes = GetTypes(ref LoadedAssemblies);
            var Bootstrappers = LoadedTypes.Where(x => x.GetInterfaces().Contains(typeof(IBootstrapper)) && x.IsClass && !x.IsAbstract && !x.ContainsGenericParameters && !x.Namespace.StartsWith("WIESEND", StringComparison.OrdinalIgnoreCase)).ToList();
            if (Bootstrappers.Count == 0)
                Bootstrappers.Add(typeof(DefaultBootstrapper));
            InternalBootstrapper = (IBootstrapper)Activator.CreateInstance(Bootstrappers[0], LoadedAssemblies, LoadedTypes);
            InternalBootstrapper.RegisterAll<IModule>();
            foreach (IModule Module in InternalBootstrapper.ResolveAll<IModule>().OrderBy(x => x.Order))
                Module.Load(InternalBootstrapper);
        }

        /// <summary>
        /// Gets the instance of the manager
        /// </summary>
        public static IBootstrapper Bootstrapper
        {
            get
            {
                if (_Instance == null)
                    lock (Temp)
                        _Instance ??= new Manager();
                return _Instance.InternalBootstrapper;
            }
        }

        /// <summary>
        /// Bootstrapper object
        /// </summary>
        protected IBootstrapper InternalBootstrapper { get; private set; }

        private static Manager _Instance = new();

        private static readonly object Temp = 1;

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Displays information about the IoC container in string form
        /// </summary>
        /// <returns>Information about the IoC container</returns>
        public override string ToString()
        {
            return Bootstrapper.Name;
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected virtual void Dispose(bool Managed)
        {
            if (InternalBootstrapper != null)
            {
                InternalBootstrapper.Dispose();
                InternalBootstrapper = null;
            }
        }

        /// <summary>
        /// Gets the types available
        /// </summary>
        /// <param name="LoadedAssemblies">The loaded assemblies.</param>
        /// <returns>The list of</returns>
        private static List<Type> GetTypes([NotNull] ref ConcurrentBag<Assembly> LoadedAssemblies)
        {
            if (LoadedAssemblies == null) throw new ArgumentNullException(nameof(LoadedAssemblies));
            List<Type> TempTypes = new();
            LoadedAssemblies = new ConcurrentBag<Assembly>(LoadedAssemblies.Where(x =>
            {
                try { TempTypes.AddRange(x.GetTypes()); }
                catch (ReflectionTypeLoadException) { return false; }
                return true;
            }));
            return TempTypes;
        }

        /// <summary>
        /// Loads the assemblies.
        /// </summary>
        /// <returns>The list of assemblies that the system has loaded</returns>
        private static ConcurrentBag<Assembly> LoadAssemblies()
        {
            List<FileInfo> Files = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.TopDirectoryOnly)
                                                                                              .Where(x => !x.Name.Equals("CULGeneratedTypes.dll", StringComparison.OrdinalIgnoreCase))
                                                                                              .Where(x => !x.FullName.StartsWith(@"C:\Windows\system32\WindowsPowerShell", StringComparison.InvariantCultureIgnoreCase))
                                                                                              .ToList();
            if (!new DirectoryInfo(".").FullName.Contains(System.Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))
                    && !new DirectoryInfo(".").FullName.Contains(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles))
                    && !new DirectoryInfo(".").FullName.Contains(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86))
                    && !new DirectoryInfo(".").FullName.Contains(System.Environment.GetFolderPath(Environment.SpecialFolder.System)))
            {
                Files.AddRange(new DirectoryInfo(".").GetFiles("*.dll", SearchOption.TopDirectoryOnly)
                                                     .Where(x => !x.Name.Equals("CULGeneratedTypes.dll", StringComparison.OrdinalIgnoreCase)));
            }
            Files = Files.Distinct().ToList();
            var LoadedAssemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            LoadAssemblies(LoadedAssemblies, Files.Select(x => AssemblyName.GetAssemblyName(x.FullName)).ToArray());
            var GeneratedFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\CULGeneratedTypes.dll");
            if (GeneratedFile.Exists
                && !LoadedAssemblies.Any(x => !x.FullName.Contains("vshost32")
                                            && !x.IsDynamic
                                            && new System.IO.FileInfo(x.Location).LastWriteTime > GeneratedFile.LastWriteTime))
            {
                LoadAssemblies(LoadedAssemblies, AssemblyName.GetAssemblyName(GeneratedFile.FullName));
            }
            return new ConcurrentBag<Assembly>(LoadedAssemblies.Distinct(new AssemblyComparer()));
        }

        /// <summary>
        /// Loads the assemblies.
        /// </summary>
        /// <param name="Assemblies">The assemblies.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        private static void LoadAssemblies(List<Assembly> Assemblies, params AssemblyName[] assemblyName)
        {
            if (assemblyName == null)
                return;
            foreach (var Name in assemblyName.Where(x => x != null
                                                      && !x.FullName.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase)
                                                      && !x.FullName.StartsWith("Microsoft.", StringComparison.InvariantCultureIgnoreCase)
                                                      && !Assemblies.Any(y => string.Equals(y.FullName, x.FullName, StringComparison.OrdinalIgnoreCase))))
            {
                try
                {
                    var TempAssembly = AppDomain.CurrentDomain.Load(Name);
                    Assemblies.Add(TempAssembly);
                    LoadAssemblies(Assemblies, TempAssembly.GetReferencedAssemblies());
                }
                catch { }
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Manager()
        {
            Dispose(false);
        }

        /// <summary>
        /// Assembly comparer
        /// </summary>
        private class AssemblyComparer : IEqualityComparer<Assembly>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type System.Reflection.Assembly to compare.</param>
            /// <param name="y">The second object of type System.Reflection.Assembly to compare.</param>
            /// <returns>true if the specified objects are equal; otherwise, false.</returns>
            public bool Equals(Assembly x, Assembly y)
            {
                return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <param name="obj">The object.</param>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data
            /// structures like a hash table.
            /// </returns>
            public int GetHashCode(Assembly obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}