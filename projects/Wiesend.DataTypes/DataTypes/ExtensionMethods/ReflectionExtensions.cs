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
using JetBrains.Annotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Wiesend.DataTypes
{
    /// <summary>
    /// Reflection oriented extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the attribute from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Attribute specified if it exists</returns>
        public static T Attribute<T>([NotNull] this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            if (Provider == null) throw new ArgumentNullException(nameof(Provider));
            if (Provider.IsDefined(typeof(T), Inherit))
            {
                var Attributes = Provider.Attributes<T>(Inherit);
                if (Attributes.Length > 0)
                    return Attributes[0];
            }
            return default;
        }

        /// <summary>
        /// Gets the attributes from the item
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Provider">Attribute provider</param>
        /// <param name="Inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>Array of attributes</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static T[] Attributes<T>([NotNull] this ICustomAttributeProvider Provider, bool Inherit = true) where T : Attribute
        {
            if (Provider == null) throw new ArgumentNullException(nameof(Provider));
            return Provider.IsDefined(typeof(T), Inherit) ? Provider.GetCustomAttributes(typeof(T), Inherit).ToArray(x => (T)x) : new T[0];
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0270:Use coalesce expression", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static ReturnType Call<ReturnType>([NotNull] this object Object, [NotNull] string MethodName, params object[] InputVariables)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(MethodName)) throw new ArgumentNullException(nameof(MethodName));
            if (InputVariables == null)
                InputVariables = new object[0];
            var ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            var Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + MethodName + " with the appropriate input variables.");
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <returns>The returned value of the method</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0270:Use coalesce expression", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static ReturnType Call<GenericType1, ReturnType>([NotNull] this object Object, [NotNull] string MethodName, params object[] InputVariables)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(MethodName)) throw new ArgumentNullException(nameof(MethodName));
            if (InputVariables == null)
                InputVariables = new object[0];
            var ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            var Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + MethodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1));
            return Object.Call<ReturnType>(Method, InputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <typeparam name="GenericType2">Generic method type 2</typeparam>
        /// <returns>The returned value of the method</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0270:Use coalesce expression", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static ReturnType Call<GenericType1, GenericType2, ReturnType>([NotNull] this object Object, [NotNull] string MethodName, params object[] InputVariables)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(MethodName)) throw new ArgumentNullException(nameof(MethodName));
            if (InputVariables == null)
                InputVariables = new object[0];
            var ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            var Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + MethodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1), typeof(GenericType2));
            return Object.Call<ReturnType>(Method, InputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <typeparam name="GenericType1">Generic method type 1</typeparam>
        /// <typeparam name="GenericType2">Generic method type 2</typeparam>
        /// <typeparam name="GenericType3">Generic method type 3</typeparam>
        /// <returns>The returned value of the method</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0270:Use coalesce expression", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static ReturnType Call<GenericType1, GenericType2, GenericType3, ReturnType>([NotNull] this object Object, [NotNull] string MethodName, params object[] InputVariables)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(MethodName)) throw new ArgumentNullException(nameof(MethodName));
            if (InputVariables == null)
                InputVariables = new object[0];
            var ObjectType = Object.GetType();
            Type[] MethodInputTypes = new Type[InputVariables.Length];
            for (int x = 0; x < InputVariables.Length; ++x)
                MethodInputTypes[x] = InputVariables[x].GetType();
            var Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
            if (Method == null)
                throw new InvalidOperationException("Could not find method " + MethodName + " with the appropriate input variables.");
            Method = Method.MakeGenericMethod(typeof(GenericType1), typeof(GenericType2), typeof(GenericType3));
            return Object.Call<ReturnType>(Method, InputVariables);
        }

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="Method">Method</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <typeparam name="ReturnType">Return type expected</typeparam>
        /// <returns>The returned value of the method</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "<Pending>")]
        public static ReturnType Call<ReturnType>([NotNull] this object Object, [NotNull] MethodInfo Method, params object[] InputVariables)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (Method == null) throw new ArgumentNullException(nameof(Method));
            if (InputVariables == null)
                InputVariables = new object[0];
            return (ReturnType)Method.Invoke(Object, InputVariables);
        }

        /// <summary>
        /// Creates an instance of the type and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static ClassType Create<ClassType>([NotNull] this Type Type, params object[] args)
        {
            if (Type == null) throw new ArgumentNullException(nameof(Type));
            return (ClassType)Type.Create(args);
        }

        /// <summary>
        /// Creates an instance of the type
        /// </summary>
        /// <param name="Type">Type to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the type</returns>
        public static object Create([NotNull] this Type Type, params object[] args)
        {
            if (Type == null) throw new ArgumentNullException(nameof(Type));
            return Activator.CreateInstance(Type, args);
        }

        /// <summary>
        /// Creates an instance of the types and casts it to the specified type
        /// </summary>
        /// <typeparam name="ClassType">Class type to return</typeparam>
        /// <param name="Types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ClassType> Create<ClassType>([NotNull] this IEnumerable<Type> Types, params object[] args)
        {
            if (Types == null) throw new ArgumentNullException(nameof(Types));
            return Types.ForEach(x => x.Create<ClassType>(args));
        }

        /// <summary>
        /// Creates an instance of the types specified
        /// </summary>
        /// <param name="Types">Types to create an instance of</param>
        /// <param name="args">Arguments sent into the constructor</param>
        /// <returns>The newly created instance of the types</returns>
        public static IEnumerable<object> Create([NotNull] this IEnumerable<Type> Types, params object[] args)
        {
            if (Types == null) throw new ArgumentNullException(nameof(Types));
            return Types.ForEach(x => x.Create(args));
        }

        /// <summary>
        /// Returns the type's name (Actual C# name, not the funky version from the Name property)
        /// </summary>
        /// <param name="ObjectType">Type to get the name of</param>
        /// <returns>string name of the type</returns>
        public static string GetName([NotNull] this Type ObjectType)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            var Output = new StringBuilder();
            if (ObjectType.Name == "Void")
            {
                Output.Append("void");
            }
            else
            {
                Output.Append(ObjectType.DeclaringType == null ? ObjectType.Namespace : ObjectType.DeclaringType.GetName())
                    .Append('.');
                if (ObjectType.Name.Contains('`'))
                {
                    var GenericTypes = ObjectType.GetGenericArguments();
                    Output.Append(ObjectType.Name.Remove(ObjectType.Name.IndexOf("`", StringComparison.OrdinalIgnoreCase)))
                        .Append('<');
                    string Seperator = "";
                    foreach (Type GenericType in GenericTypes)
                    {
                        Output.Append(Seperator).Append(GenericType.GetName());
                        Seperator = ",";
                    }
                    Output.Append('>');
                }
                else
                {
                    Output.Append(ObjectType.Name);
                }
            }
            return Output.ToString().Replace("&", "");
        }

        /// <summary>
        /// Determines if the type has a default constructor
        /// </summary>
        /// <param name="Type">Type to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public static bool HasDefaultConstructor([NotNull] this Type Type)
        {
            if (Type == null) throw new ArgumentNullException(nameof(Type));
            return Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                        .Any(x => x.GetParameters().Length == 0);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static bool Is([NotNull] this object Object, [NotNull] Type Type)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (Type == null) throw new ArgumentNullException(nameof(Type));
            return Object.GetType().Is(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Type">Type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this Type ObjectType, [NotNull] Type Type)
        {
            if (Type == null) throw new ArgumentNullException(nameof(Type));
            if (ObjectType == null)
                return false;
            if (Type == typeof(object))
                return true;
            if (Type == ObjectType || ObjectType.GetInterfaces().Any(x => x == Type))
                return true;
            if (ObjectType.BaseType == null)
                return false;
            return ObjectType.BaseType.Is(Type);
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="Object">Object</param>
        /// <typeparam name="BaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static bool Is<BaseObjectType>([NotNull] this object Object)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            return Object.Is(typeof(BaseObjectType));
        }

        /// <summary>
        /// Determines if an object is of a specific type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <typeparam name="BaseObjectType">Base object type</typeparam>
        /// <returns>True if it is, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static bool Is<BaseObjectType>([NotNull] this Type ObjectType)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            return ObjectType.Is(typeof(BaseObjectType));
        }

        /// <summary>
        /// Loads an assembly by its name
        /// </summary>
        /// <param name="Name">Name of the assembly to return</param>
        /// <returns>The assembly specified if it exists</returns>
        public static System.Reflection.Assembly Load([NotNull] this AssemblyName Name)
        {
            if (Name == null) throw new ArgumentNullException(nameof(Name));
            try
            {
                return AppDomain.CurrentDomain.Load(Name);
            }
            catch (BadImageFormatException) { return null; }
        }

        /// <summary>
        /// Loads assemblies within a directory and returns them in an array.
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <param name="Recursive">Determines whether to search recursively or not</param>
        /// <returns>Array of assemblies in the directory</returns>
        public static IEnumerable<Assembly> LoadAssemblies([NotNull] this DirectoryInfo Directory, bool Recursive = false)
        {
            if (Directory == null) throw new ArgumentNullException(nameof(Directory));
            var Assemblies = new List<Assembly>();
            foreach (FileInfo File in Directory.GetFiles("*.dll", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Assemblies.Add(AssemblyName.GetAssemblyName(File.FullName).Load());
                }
                catch (BadImageFormatException) { }
            }
            return Assemblies;
        }

        /// <summary>
        /// Makes a shallow copy of the object
        /// </summary>
        /// <param name="Object">Object to copy</param>
        /// <param name="SimpleTypesOnly">
        /// If true, it only copies simple types (no classes, only items like int, string, etc.),
        /// false copies everything.
        /// </param>
        /// <returns>A copy of the object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static T MakeShallowCopy<T>(this T Object, bool SimpleTypesOnly = false)
        {
            if (Object == null)
                return default;
            var ObjectType = Object.GetType();
            var ClassInstance = ObjectType.Create<T>();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                try
                {
                    if (Property.CanRead
                            && Property.CanWrite
                            && SimpleTypesOnly
                            && Property.PropertyType.IsValueType)
                        Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                    else if (!SimpleTypesOnly
                                && Property.CanRead
                                && Property.CanWrite)
                        Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
                }
                catch { }
            }

            foreach (FieldInfo Field in ObjectType.GetFields())
            {
                try
                {
                    if (SimpleTypesOnly && Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                    else if (!SimpleTypesOnly && Field.IsPublic)
                        Field.SetValue(ClassInstance, Field.GetValue(Object));
                }
                catch { }
            }

            return ClassInstance;
        }

        /// <summary>
        /// Goes through a list of types and determines if they're marked with a specific attribute
        /// </summary>
        /// <typeparam name="T">Attribute type</typeparam>
        /// <param name="Types">Types to check</param>
        /// <param name="Inherit">
        /// When true, it looks up the heirarchy chain for the inherited custom attributes
        /// </param>
        /// <returns>The list of types that are marked with an attribute</returns>
        public static IEnumerable<Type> MarkedWith<T>(this IEnumerable<Type> Types, bool Inherit = true)
            where T : Attribute
        {
            if (Types == null)
                return null;
            return Types.Where(x => x.IsDefined(typeof(T), Inherit) && !x.IsAbstract);
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within an assembly that are of the
        /// specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assembly">Assembly to search within</param>
        /// <param name="Args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ClassType> Objects<ClassType>([NotNull] this Assembly Assembly, params object[] Args)
        {
            if (Assembly == null) throw new ArgumentNullException(nameof(Assembly));
            return Assembly.Types<ClassType>().Where(x => !x.ContainsGenericParameters).Create<ClassType>(Args);
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a group of assemblies that are
        /// of the specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Assemblies">Assemblies to search within</param>
        /// <param name="Args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ClassType> Objects<ClassType>([NotNull] this IEnumerable<Assembly> Assemblies, params object[] Args)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            var ReturnValues = new List<ClassType>();
            foreach (Assembly Assembly in Assemblies)
                ReturnValues.AddRange(Assembly.Objects<ClassType>(Args));
            return ReturnValues;
        }

        /// <summary>
        /// Returns an instance of all classes that it finds within a directory that are of the
        /// specified base type/interface.
        /// </summary>
        /// <typeparam name="ClassType">Base type/interface searching for</typeparam>
        /// <param name="Directory">Directory to search within</param>
        /// <param name="Recursive">Should this be recursive</param>
        /// <param name="Args">Args used to create the object</param>
        /// <returns>A list of objects that are of the type specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ClassType> Objects<ClassType>([NotNull] this DirectoryInfo Directory, bool Recursive, params object[] Args)
        {
            if (Directory == null) throw new ArgumentNullException(nameof(Directory));
            return Directory.LoadAssemblies(Recursive).Objects<ClassType>(Args);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static object Property([NotNull] this object Object, [NotNull] PropertyInfo Property)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            return Property.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the value of property
        /// </summary>
        /// <param name="Object">The object to get the property of</param>
        /// <param name="Property">The property to get</param>
        /// <returns>Returns the property's value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0056:Use index operator", Justification = "<Pending>")]
        public static object Property([NotNull] this object Object, [NotNull] string Property)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(Property)) throw new ArgumentNullException(nameof(Property));
            var Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = Object;
            var TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return null;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            if (DestinationProperty == null)
                throw new NullReferenceException("PropertyInfo can't be null");
            return TempObject.Property(DestinationProperty);
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static object Property([NotNull] this object Object, [NotNull] PropertyInfo Property, [NotNull] object Value, string Format = "")
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            if (Value == null) throw new ArgumentNullException(nameof(Value));
            if (Property.PropertyType == typeof(string))
                Value = Value.FormatToString(Format);
            Property.SetValue(Object, Value.To(Property.PropertyType, null), null);
            return Object;
        }

        /// <summary>
        /// Sets the value of destination property
        /// </summary>
        /// <param name="Object">The object to set the property of</param>
        /// <param name="Property">The property to set</param>
        /// <param name="Value">Value to set the property to</param>
        /// <param name="Format">Allows for formatting if the destination is a string</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0056:Use index operator", Justification = "<Pending>")]
        public static object Property([NotNull] this object Object, [NotNull] string Property, [NotNull] object Value, string Format = "")
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            if (string.IsNullOrEmpty(Property)) throw new ArgumentNullException(nameof(Property));
            if (Value == null) throw new ArgumentNullException(nameof(Value));
            var Properties = Property.Split(new string[] { "." }, StringSplitOptions.None);
            object TempObject = Object;
            var TempObjectType = TempObject.GetType();
            PropertyInfo DestinationProperty = null;
            for (int x = 0; x < Properties.Length - 1; ++x)
            {
                DestinationProperty = TempObjectType.GetProperty(Properties[x]);
                TempObjectType = DestinationProperty.PropertyType;
                TempObject = DestinationProperty.GetValue(TempObject, null);
                if (TempObject == null)
                    return Object;
            }
            DestinationProperty = TempObjectType.GetProperty(Properties[Properties.Length - 1]);
            if (DestinationProperty == null)
                throw new NullReferenceException("PropertyInfo can't be null");
            TempObject.Property(DestinationProperty, Value, Format);
            return Object;
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static Expression<Func<ClassType, DataType>> PropertyGetter<ClassType, DataType>([NotNull] this PropertyInfo Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            if (!Property.PropertyType.Is(typeof(DataType)))
                throw new ArgumentException("Property is not of the type specified");
            if (!Property.DeclaringType.Is(typeof(ClassType)) && !typeof(ClassType).Is(Property.DeclaringType))
                throw new ArgumentException("Property is not from the declaring class type specified");
            var ObjectInstance = Expression.Parameter(Property.DeclaringType, "x");
            var PropertyGet = Expression.Property(ObjectInstance, Property);
            if (Property.PropertyType != typeof(DataType))
            {
                var Convert = Expression.Convert(PropertyGet, typeof(DataType));
                return Expression.Lambda<Func<ClassType, DataType>>(Convert, ObjectInstance);
            }
            return Expression.Lambda<Func<ClassType, DataType>>(PropertyGet, ObjectInstance);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's getter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's getter function</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static Expression<Func<ClassType, object>> PropertyGetter<ClassType>([NotNull] this PropertyInfo Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            return Property.PropertyGetter<ClassType, object>();
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0083:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0038:Use pattern matching", Justification = "<Pending>")]
        public static string PropertyName([NotNull] this LambdaExpression Expression)
        {
            if (Expression == null) throw new ArgumentNullException(nameof(Expression));
            if (Expression.Body is UnaryExpression && Expression.Body.NodeType == ExpressionType.Convert)
            {
                var Temp = (MemberExpression)((UnaryExpression)Expression.Body).Operand;
                return Temp.Expression.PropertyName() + Temp.Member.Name;
            }
            if (!(Expression.Body is MemberExpression))
                throw new ArgumentException("Expression.Body is not a MemberExpression");
            return ((MemberExpression)Expression.Body).Expression.PropertyName() + ((MemberExpression)Expression.Body).Member.Name;
        }

        /// <summary>
        /// Gets a property name
        /// </summary>
        /// <param name="Expression">LINQ expression</param>
        /// <returns>The name of the property</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "<Pending>")]
        public static string PropertyName(this Expression Expression)
        {
            MemberExpression TempExpression = Expression as MemberExpression;
            if (TempExpression == null)
                return "";
            return TempExpression.Expression.PropertyName() + TempExpression.Member.Name + ".";
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <typeparam name="DataType">Data type expecting</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0056:Use index operator", Justification = "<Pending>")]
        public static Expression<Action<ClassType, DataType>> PropertySetter<ClassType, DataType>([NotNull] this LambdaExpression Property)//Expression<Func<ClassType, DataType>> Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            var PropertyName = Property.PropertyName();
            var SplitName = PropertyName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (SplitName.Length == 0)
                return null;
            var PropertyInfo = typeof(ClassType).GetProperty(SplitName[0]);
            var ObjectInstance = Expression.Parameter(PropertyInfo.DeclaringType, "x");
            var PropertySet = Expression.Parameter(typeof(DataType), "y");
            var DefaultConstant = Expression.Constant(((object)null).To(PropertyInfo.PropertyType, null), PropertyInfo.PropertyType);
            MethodCallExpression SetterCall = null;
            MemberExpression PropertyGet = null;
            if (SplitName.Length > 1)
            {
                PropertyGet = Expression.Property(ObjectInstance, PropertyInfo);
                for (int x = 1; x < SplitName.Length - 1; ++x)
                {
                    PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[x]);
                    if (PropertyInfo == null)
                        throw new NullReferenceException("PropertyInfo can't be null");
                    PropertyGet = Expression.Property(PropertyGet, PropertyInfo);
                }
                PropertyInfo = PropertyInfo.PropertyType.GetProperty(SplitName[SplitName.Length - 1]);
            }
            var SetMethod = PropertyInfo.GetSetMethod();
            if (SetMethod != null)
            {
                if (PropertyInfo.PropertyType != typeof(DataType))
                {
                    var ConversionMethod = typeof(TypeConversionExtensions).GetMethods().FirstOrDefault(x => x.ContainsGenericParameters
                        && x.GetGenericArguments().Length == 2
                        && x.Name == "To"
                        && x.GetParameters().Length == 2);
                    ConversionMethod = ConversionMethod.MakeGenericMethod(typeof(DataType), PropertyInfo.PropertyType);
                    var Convert = Expression.Call(ConversionMethod, PropertySet, DefaultConstant);
                    SetterCall = PropertyGet == null ? Expression.Call(ObjectInstance, SetMethod, Convert) : Expression.Call(PropertyGet, SetMethod, Convert);
                    return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
                }
                SetterCall = PropertyGet == null ? Expression.Call(ObjectInstance, SetMethod, PropertySet) : Expression.Call(PropertyGet, SetMethod, PropertySet);
            }
            else
                return Expression.Lambda<Action<ClassType, DataType>>(Expression.Empty(), ObjectInstance, PropertySet);
            return Expression.Lambda<Action<ClassType, DataType>>(SetterCall, ObjectInstance, PropertySet);
        }

        /// <summary>
        /// Gets a lambda expression that calls a specific property's setter function
        /// </summary>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Property">Property</param>
        /// <returns>A lambda expression that calls a specific property's setter function</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static Expression<Action<ClassType, object>> PropertySetter<ClassType>([NotNull] this LambdaExpression Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            return Property.PropertySetter<ClassType, object>();
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="Object">object who contains the property</param>
        /// <param name="PropertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static Type PropertyType(this object Object, string PropertyPath)
        {
            if (Object == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            return Object.GetType().PropertyType(PropertyPath);
        }

        /// <summary>
        /// Gets a property's type
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="PropertyPath">
        /// Path of the property (ex: Prop1.Prop2.Prop3 would be the Prop1 of the source object,
        /// which then has a Prop2 on it, which in turn has a Prop3 on it.)
        /// </param>
        /// <returns>The type of the property specified or null if it can not be reached.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public static Type PropertyType(this Type ObjectType, string PropertyPath)
        {
            if (ObjectType == null || string.IsNullOrEmpty(PropertyPath))
                return null;
            var SourceProperties = PropertyPath.Split(new string[] { "." }, StringSplitOptions.None);
            PropertyInfo PropertyInfo = null;
            for (int x = 0; x < SourceProperties.Length; ++x)
            {
                PropertyInfo = ObjectType.GetProperty(SourceProperties[x]);
                ObjectType = PropertyInfo.PropertyType;
            }
            return ObjectType;
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="Assembly">Assembly to get version information from</param>
        /// <param name="InfoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString([NotNull] this Assembly Assembly, VersionInfo InfoType)
        {
            if (Assembly == null) throw new ArgumentNullException(nameof(Assembly));
            if (InfoType.HasFlag(VersionInfo.ShortVersion))
            {
                Version Version = Assembly.GetName().Version;
                return Version.Major + "." + Version.Minor;
            }
            else
            {
                return Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets the version information in a string format
        /// </summary>
        /// <param name="Assemblies">Assemblies to get version information from</param>
        /// <param name="InfoType">Version info type</param>
        /// <returns>The version information as a string</returns>
        public static string ToString([NotNull] this IEnumerable<Assembly> Assemblies, VersionInfo InfoType)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            var Builder = new StringBuilder();
            Assemblies.OrderBy(x => x.FullName).ForEach<Assembly>(x => Builder.AppendLine(x.GetName().Name + ": " + x.ToString(InfoType)));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets assembly information for all currently loaded assemblies
        /// </summary>
        /// <param name="Assemblies">Assemblies to dump information from</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>An HTML formatted string containing the assembly information</returns>
        public static string ToString([NotNull] this IEnumerable<Assembly> Assemblies, bool HTMLOutput)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            var Builder = new StringBuilder();
            Builder.Append(HTMLOutput ? "<strong>Assembly Information</strong><br />" : "Assembly Information\r\n");
            Assemblies.ForEach<Assembly>(x => Builder.Append(x.ToString(HTMLOutput)));
            return Builder.ToString();
        }

        /// <summary>
        /// Dumps the property names and current values from an object
        /// </summary>
        /// <param name="Object">Object to dunp</param>
        /// <param name="HTMLOutput">Determines if the output should be HTML or not</param>
        /// <returns>An HTML formatted table containing the information about the object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static string ToString([NotNull] this object Object, bool HTMLOutput)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            var TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            var ObjectType = Object.GetType();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : System.Environment.NewLine).Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                var Parameters = Property.GetIndexParameters();
                if (Property.CanRead && Parameters.Length == 0)
                {
                    try
                    {
                        var Value = Property.GetValue(Object, null);
                        TempValue.Append(Value == null ? "null" : Value.ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Dumps the properties names and current values from an object type (used for static classes)
        /// </summary>
        /// <param name="ObjectType">Object type to dunp</param>
        /// <param name="HTMLOutput">Should this be output as an HTML string</param>
        /// <returns>An HTML formatted table containing the information about the object type</returns>
        public static string ToString([NotNull] this Type ObjectType, bool HTMLOutput)
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            var TempValue = new StringBuilder();
            TempValue.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody>" : "Property Name\t\t\t\tProperty Value");
            var Properties = ObjectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                TempValue.Append(HTMLOutput ? "<tr><td>" : System.Environment.NewLine).Append(Property.Name).Append(HTMLOutput ? "</td><td>" : "\t\t\t\t");
                if (Property.CanRead && Property.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        TempValue.Append(Property.GetValue(null, null) == null ? "null" : Property.GetValue(null, null).ToString());
                    }
                    catch { }
                }
                TempValue.Append(HTMLOutput ? "</td></tr>" : "");
            }
            TempValue.Append(HTMLOutput ? "</tbody></table>" : "");
            return TempValue.ToString();
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<Type> Types<BaseType>([NotNull] this Assembly Assembly)
        {
            if (Assembly == null) throw new ArgumentNullException(nameof(Assembly));
            return Assembly.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assembly">Assembly to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types([NotNull] this Assembly Assembly, [NotNull] Type BaseType)
        {
            if (Assembly == null) throw new ArgumentNullException(nameof(Assembly));
            if (BaseType == null) throw new ArgumentNullException(nameof(BaseType));
            try
            {
                return Assembly.GetTypes().Where(x => x.Is(BaseType) && x.IsClass && !x.IsAbstract);
            }
            catch { return new List<Type>(); }
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <typeparam name="BaseType">Class type to search for</typeparam>
        /// <returns>List of types that use the interface</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<Type> Types<BaseType>([NotNull] this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            return Assemblies.Types(typeof(BaseType));
        }

        /// <summary>
        /// Gets a list of types based on an interface
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <param name="BaseType">Base type to look for</param>
        /// <returns>List of types that use the interface</returns>
        public static IEnumerable<Type> Types([NotNull] this IEnumerable<Assembly> Assemblies, [NotNull] Type BaseType)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            if (BaseType == null) throw new ArgumentNullException(nameof(BaseType));
            var ReturnValues = new List<Type>();
            Assemblies.ForEach(y => ReturnValues.AddRange(y.Types(BaseType)));
            return ReturnValues;
        }

        /// <summary>
        /// Gets a list of types in the assemblies specified
        /// </summary>
        /// <param name="Assemblies">Assemblies to check</param>
        /// <returns>List of types</returns>
        public static IEnumerable<Type> Types([NotNull] this IEnumerable<Assembly> Assemblies)
        {
            if (Assemblies == null) throw new ArgumentNullException(nameof(Assemblies));
            var ReturnValues = new List<Type>();
            Assemblies.ForEach(y =>
            {
                try
                {
                    ReturnValues.AddRange(y.GetTypes());
                }
                catch (ReflectionTypeLoadException) { }
            });
            return ReturnValues;
        }
    }

    /// <summary>
    /// Version info
    /// </summary>
    public enum VersionInfo
    {
        /// <summary>
        /// Short version
        /// </summary>
        ShortVersion = 1,

        /// <summary>
        /// Long version
        /// </summary>
        LongVersion = 2
    }
}