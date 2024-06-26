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
using System.Linq;
using System.Reflection;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.DataTypes.AOP.Interfaces;
using Wiesend.ORM.Manager.Aspect.Interfaces;
using Wiesend.ORM.Manager.Mapper.Interfaces;

namespace Wiesend.ORM.Aspect
{
    /// <summary>
    /// ORM Aspect (used internally)
    /// </summary>
    public class ORMAspect : IAspect
    {
        /// <summary>
        /// Mapper
        /// </summary>
        public static Manager.Mapper.Manager Mapper { get; set; }

        /// <summary>
        /// Assemblies using
        /// </summary>
        public ICollection<Assembly> AssembliesUsing { get { return new Assembly[] { typeof(ORMAspect).Assembly, typeof(INotifyPropertyChanged).Assembly }; } }

        /// <summary>
        /// Interfaces using
        /// </summary>
        public ICollection<Type> InterfacesUsing { get { return new Type[] { typeof(IORMObject) }; } }

        /// <summary>
        /// Usings using
        /// </summary>
        public ICollection<string> Usings
        {
            get
            {
                return new string[] {
                    "Wiesend.DataTypes",
                    "Wiesend.ORM.Manager",
                    "Wiesend.ORM.Manager.Aspect.Interfaces",
                    "System.ComponentModel",
                    "System.Runtime.CompilerServices"
                };
            }
        }

        /// <summary>
        /// Fields that have been completed already
        /// </summary>
        private List<IProperty> Fields { get; set; }

        /// <summary>
        /// Sets up the aspect
        /// </summary>
        /// <param name="Object">Object to set up</param>
        public void Setup(object Object)
        {
            var TempObject = (IORMObject)Object;
            TempObject.Session0 = new Manager.Session();
            TempObject.PropertiesChanged0 = new List<string>();
            TempObject.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                var x = (IORMObject)sender;
                x.PropertiesChanged0.Add(e.PropertyName);
            };
        }

        /// <summary>
        /// Sets up the default constructor
        /// </summary>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupDefaultConstructor(Type BaseType)
        {
            return "";
        }

        /// <summary>
        /// Sets up the end of a method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <param name="ReturnValueName">Return value name</param>
        /// <returns>The code used</returns>
        public string SetupEndMethod(MethodInfo Method, Type BaseType, string ReturnValueName)
        {
            var Builder = new StringBuilder();
            if (Mapper[BaseType] != null && Method.Name.StartsWith("get_", StringComparison.Ordinal))
            {
                foreach (IMapping Mapping in Mapper[BaseType])
                {
                    var Property = Mapping.Properties.FirstOrDefault(x => x.Name == Method.Name.Replace("get_", ""));
                    if (Property != null)
                    {
                        if (Property is IManyToOne || Property is IMap)
                            Builder.AppendLine(SetupSingleProperty(ReturnValueName, Property));
                        else if (Property is IIEnumerableManyToOne || Property is IManyToMany
                            || Property is IIListManyToMany || Property is IIListManyToOne
                            || Property is ICollectionManyToMany || Property is ICollectionManyToOne)
                            Builder.AppendLine(SetupIEnumerableProperty(ReturnValueName, Property));
                        else if (Property is IListManyToMany || Property is IListManyToOne)
                            Builder.AppendLine(SetupListProperty(ReturnValueName, Property));
                        return Builder.ToString();
                    }
                }
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Sets up exception method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupExceptionMethod(MethodInfo Method, Type BaseType)
        {
            return "var Exception=CaughtException;";
        }

        /// <summary>
        /// Sets up the interfaces used
        /// </summary>
        /// <param name="Type">The object type</param>
        /// <returns>The code used</returns>
        public string SetupInterfaces(Type Type)
        {
            var Builder = new StringBuilder();
            Builder.AppendLine(@"public Session Session0{ get; set; }");
            Builder.AppendLine(@"public IList<string> PropertiesChanged0{ get; set; }");
            if (!Type.Is<INotifyPropertyChanged>())
            {
                Builder.AppendLine(@"private PropertyChangedEventHandler propertyChanged_;
public event PropertyChangedEventHandler PropertyChanged
{
    add
    {
        propertyChanged_-=value;
        propertyChanged_+=value;
    }

    remove
    {
        propertyChanged_-=value;
    }
}");
                Builder.AppendLine(@"private void NotifyPropertyChanged0([CallerMemberName]string propertyName="""")
{
    var Handler = propertyChanged_;
    if (Handler != null)
        Handler(this, new PropertyChangedEventArgs(propertyName));
}");
            }
            Builder.AppendLine(SetupFields(Type));
            return Builder.ToString();
        }

        /// <summary>
        /// Sets up the start of the method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupStartMethod(MethodInfo Method, Type BaseType)
        {
            var Builder = new StringBuilder();
            if (Mapper[BaseType] != null && Method.Name.StartsWith("set_", StringComparison.Ordinal))
            {
                foreach (IMapping Mapping in Mapper[BaseType])
                {
                    var Property = Mapping.Properties.FirstOrDefault(x => x.Name == Method.Name.Replace("set_", ""));
                    if (Fields.Contains(Property))
                    {
                        Builder.AppendLineFormat("{0}=value;", Property.DerivedFieldName);
                    }
                    if (Property != null)
                        Builder.AppendLineFormat("NotifyPropertyChanged0(\"{0}\");", Property.Name);
                }
            }
            return Builder.ToString();
        }

        private static string SetupIEnumerableProperty(string ReturnValueName, [NotNull] IProperty Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping != null)) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping.ObjectType != null)) throw new ArgumentNullException(nameof(Property));
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperties<{1},{2}>(this,\"{3}\");",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("((ObservableList<{1}>){0}).CollectionChanged += (x, y) => NotifyPropertyChanged0(\"{2}\");", Property.DerivedFieldName, Property.Type.GetName(), Property.Name)
                .AppendLineFormat("((ObservableList<{1}>){0}).ForEach(TempObject => {{ ((IORMObject)TempObject).PropertyChanged += (x, y) => ((ObservableList<{1}>){0}).NotifyObjectChanged(x); }});", Property.DerivedFieldName, Property.Type.GetName())
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private static string SetupListProperty(string ReturnValueName, [NotNull] IProperty Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping != null)) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping.ObjectType != null)) throw new ArgumentNullException(nameof(Property));
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperties<{1},{2}>(this,\"{3}\").ToList();",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("NotifyPropertyChanged0(\"{0}\");", Property.Name)
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private static string SetupSingleProperty(string ReturnValueName, [NotNull] IProperty Property)
        {
            if (Property == null) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping != null)) throw new ArgumentNullException(nameof(Property));
            if (!(Property.Mapping.ObjectType != null)) throw new ArgumentNullException(nameof(Property));
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperty<{1},{2}>(this,\"{3}\");",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("if({0}!=null)", Property.DerivedFieldName)
                .AppendLine("{")
                .AppendLineFormat("({0} as INotifyPropertyChanged).PropertyChanged+=(x,y)=>NotifyPropertyChanged0(\"{1}\");", Property.DerivedFieldName, Property.Name)
                .AppendLine("}")
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private string SetupFields(Type Type)
        {
            Fields = new List<IProperty>();
            var Builder = new StringBuilder();
            if (Mapper[Type] != null)
            {
                foreach (IMapping Mapping in Mapper[Type])
                {
                    foreach (IProperty Property in Mapping.Properties.Where(x => !Fields.Contains(x)))
                    {
                        Fields.Add(Property);
                        Builder.AppendLineFormat("private {0} {1};", Property.TypeName, Property.DerivedFieldName);
                        Builder.AppendLineFormat("private bool {0};", Property.DerivedFieldName + "Loaded");
                    }
                }
            }
            return Builder.ToString();
        }
    }
}