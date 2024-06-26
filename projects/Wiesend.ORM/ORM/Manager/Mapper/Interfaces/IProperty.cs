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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using Wiesend.DataTypes;
using Wiesend.DataTypes.Patterns;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.Mapper.Interfaces
{
    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    /// <typeparam name="ReturnType">Return type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public interface IProperty<ClassType, DataType, ReturnType> : IFluentInterface
        where ClassType : class
        where ReturnType : IProperty<ClassType, DataType, ReturnType>
    {
        /// <summary>
        /// Turns on autoincrement for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType SetAutoIncrement();

        /// <summary>
        /// Turns on cascade for saving/deleting
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetCascade();

        /// <summary>
        /// Sets the default value of the property
        /// </summary>
        /// <param name="Value">Default value</param>
        /// <returns>This IProperty object</returns>
        ReturnType SetDefaultValue(Func<DataType> Value);

        /// <summary>
        /// Sets the name of the field in the database
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>this</returns>
        ReturnType SetFieldName(string FieldName);

        /// <summary>
        /// Turns on indexing for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType SetIndex();

        /// <summary>
        /// Allows you to load a property based on a specified command
        /// </summary>
        /// <param name="Command">Command used to load the property</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>this</returns>
        ReturnType SetLoadUsingCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets the max length for the property (or precision for items like decimal values)
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>this</returns>
        ReturnType SetMaxLength(int MaxLength);

        /// <summary>
        /// Sets the field such that null values are not allowed
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetNotNull();

        /// <summary>
        /// Set database table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>this</returns>
        ReturnType SetTableName(string TableName);

        /// <summary>
        /// Ensures the field is unique
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetUnique();
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType"></typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public interface IProperty<ClassType, DataType> : IProperty<ClassType>
        where ClassType : class
    {
        /// <summary>
        /// Compiled version of the expression
        /// </summary>
        Func<ClassType, DataType> CompiledExpression { get; }

        /// <summary>
        /// Default value for this property
        /// </summary>
        Func<DataType> DefaultValue { get; }

        /// <summary>
        /// Expression pointing to the property
        /// </summary>
        Expression<Func<ClassType, DataType>> Expression { get; }
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public interface IProperty<ClassType> : IProperty
        where ClassType : class
    {
        /// <summary>
        /// Does a cascade delete of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch CascadeDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);

        /// <summary>
        /// Called to create a batch that deletes items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch CascadeJoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);

        /// <summary>
        /// Called to create a batch that saves items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch CascadeJoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);

        /// <summary>
        /// Does a cascade save of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch CascadeSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);

        /// <summary>
        /// Gets the property's value from the object sent in
        /// </summary>
        /// <param name="Object">Object to get the value from</param>
        /// <returns>The value of the property</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        object GetValue(ClassType Object);

        /// <summary>
        /// Called to create a batch that deletes items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch JoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);

        /// <summary>
        /// Called to create a batch that saves items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects that have been visited thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        IBatch JoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen);
    }

    /// <summary>
    /// Property interface
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Auto increment
        /// </summary>
        bool AutoIncrement { get; }

        /// <summary>
        /// Cascade
        /// </summary>
        bool Cascade { get; }

        /// <summary>
        /// Derived field name
        /// </summary>
        string DerivedFieldName { get; }

        /// <summary>
        /// Field name
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// If the property is a class, this is the foreign mapping
        /// </summary>
        IMapping ForeignMapping { get; }

        /// <summary>
        /// Index
        /// </summary>
        bool Index { get; }

        /// <summary>
        /// Load command
        /// </summary>
        string LoadCommand { get; }

        /// <summary>
        /// Load command type
        /// </summary>
        CommandType LoadCommandType { get; }

        /// <summary>
        /// Mapping
        /// </summary>
        IMapping Mapping { get; }

        /// <summary>
        /// Max length
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Not null
        /// </summary>
        bool NotNull { get; }

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Property type
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        string TypeName { get; }

        /// <summary>
        /// Unique
        /// </summary>
        bool Unique { get; }

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        object GetParameter(object Object);

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        object GetParameter(Dynamo Object);

        /// <summary>
        /// Gets the property's value from the object sent in
        /// </summary>
        /// <param name="Object">Object to get the value from</param>
        /// <returns>The value of the property</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        object GetValue(object Object);

        /// <summary>
        /// Gets the property's value from the object sent in
        /// </summary>
        /// <param name="Object">Object to get the value from</param>
        /// <returns>The value of the property</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        object GetValue(Dynamo Object);

        /// <summary>
        /// Sets up the property (used internally)
        /// </summary>
        /// <param name="MappingProvider">Mapping provider</param>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source to use</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Setup(ISourceInfo Source, Mapper.Manager MappingProvider, QueryProvider.Manager QueryProvider);
    }
}