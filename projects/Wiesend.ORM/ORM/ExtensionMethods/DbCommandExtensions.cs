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
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Wiesend.DataTypes;
using Wiesend.DataTypes.Comparison;

namespace Wiesend.ORM
{
    /// <summary>
    /// Extension methods for DbCommand objects
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <returns>The DbCommand object</returns>
        public static DbCommand AddParameter([NotNull] this DbCommand Command, [NotNull] string ID, string Value = "", ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Command == null) throw new ArgumentNullException(nameof(Command));
            if (string.IsNullOrEmpty(ID)) throw new ArgumentNullException(nameof(ID));
            int Length = string.IsNullOrEmpty(Value) ? 1 : Value.Length;
            if (Direction == ParameterDirection.Output
                || Direction == ParameterDirection.InputOutput
                || Length > 4000
                || Length < -1)
                Length = -1;
            var Parameter = Command.GetOrCreateParameter(ID);
            Parameter.Value = string.IsNullOrEmpty(Value) ? DBNull.Value : (object)Value;
            Parameter.IsNullable = string.IsNullOrEmpty(Value);
            Parameter.DbType = DbType.String;
            Parameter.Direction = Direction;
            Parameter.Size = Length;
            return Command;
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <returns>The DbCommand object</returns>
        public static DbCommand AddParameter([NotNull] this DbCommand Command, [NotNull] string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Command == null) throw new ArgumentNullException(nameof(Command));
            if (string.IsNullOrEmpty(ID)) throw new ArgumentNullException(nameof(ID));
            return Command.AddParameter(ID, Type.To(DbType.Int32), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Value">Value to add</param>
        /// <returns>The DbCommand object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static DbCommand AddParameter<DataType>([NotNull] this DbCommand Command, [NotNull] string ID, DataType Value = default, ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Command == null) throw new ArgumentNullException(nameof(Command));
            if (string.IsNullOrEmpty(ID)) throw new ArgumentNullException(nameof(ID));
            return Command.AddParameter(ID,
                new GenericEqualityComparer<DataType>().Equals(Value, default) ? typeof(DataType).To(DbType.Int32) : Value.GetType().To(DbType.Int32),
                Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <returns>The DbCommand object</returns>
        public static DbCommand AddParameter([NotNull] this DbCommand Command, [NotNull] string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Command == null) throw new ArgumentNullException(nameof(Command));
            if (string.IsNullOrEmpty(ID)) throw new ArgumentNullException(nameof(ID));
            var Parameter = Command.GetOrCreateParameter(ID);
            Parameter.Value = Value == null || Convert.IsDBNull(Value) ? DBNull.Value : Value;
            Parameter.IsNullable = Value == null || Convert.IsDBNull(Value);
            if (Type != default)
                Parameter.DbType = Type;
            Parameter.Direction = Direction;
            return Command;
        }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>A transaction object</returns>
        public static DbTransaction BeginTransaction(this DbCommand Command)
        {
            if (Command == null || Command.Connection == null)
                return null;
            Command.Open();
            Command.Transaction = Command.Connection.BeginTransaction();
            return Command.Transaction;
        }

        /// <summary>
        /// Clears the parameters
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The DBCommand object</returns>
        public static DbCommand ClearParameters(this DbCommand Command)
        {
            if (Command != null && Command.Parameters != null)
                Command.Parameters.Clear();
            return Command;
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The DBCommand object</returns>
        public static DbCommand Close(this DbCommand Command)
        {
            if (Command != null
                && Command.Connection != null
                && Command.Connection.State != ConnectionState.Closed)
                Command.Connection.Close();
            return Command;
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The DBCommand object</returns>
        public static DbCommand Commit(this DbCommand Command)
        {
            if (Command != null && Command.Transaction != null)
                Command.Transaction.Commit();
            return Command;
        }

        /// <summary>
        /// Executes the query and returns a data set
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <param name="Factory">DbProviderFactory being used</param>
        /// <returns>A dataset filled with the results of the query</returns>
        public static DataSet ExecuteDataSet(this DbCommand Command, DbProviderFactory Factory)
        {
            if (Command == null)
                return null;
            Command.Open();
            using DbDataAdapter Adapter = Factory.CreateDataAdapter();
            Adapter.SelectCommand = Command;
            var ReturnSet = new DataSet { Locale = CultureInfo.CurrentCulture };
            Adapter.Fill(ReturnSet);
            return ReturnSet;
        }

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <param name="Default">Default value if there is an issue</param>
        /// <returns>The object of the first row and first column</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static DataType ExecuteScalar<DataType>(this DbCommand Command, DataType Default = default)
        {
            if (Command == null)
                return Default;
            Command.Open();
            return Command.ExecuteScalar().To<object, DataType>(Default);
        }

        /// <summary>
        /// Gets a parameter or creates it, if it is not found
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Command">Command object</param>
        /// <returns>The DbParameter associated with the ID</returns>
        public static DbParameter GetOrCreateParameter(this DbCommand Command, string ID)
        {
            if (Command == null)
                return null;
            if (Command.Parameters.Contains(ID))
                return Command.Parameters[ID];
            var Parameter = Command.CreateParameter();
            Parameter.ParameterName = ID;
            Command.Parameters.Add(Parameter);
            return Parameter;
        }

        /// <summary>
        /// Returns an output parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="ID">Parameter name</param>
        /// <param name="Command">Command object</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>
        /// if the parameter exists (and isn't null or empty), it returns the parameter's value.
        /// Otherwise the default value is returned.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static DataType GetOutputParameter<DataType>(this DbCommand Command, string ID, DataType Default = default)
        {
            return Command != null && Command.Parameters[ID] != null ?
                Command.Parameters[ID].Value.To<object, DataType>(Default) :
                Default;
        }

        /// <summary>
        /// Opens the connection
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The DBCommand object</returns>
        public static DbCommand Open(this DbCommand Command)
        {
            if (Command != null
                && Command.Connection != null
                && Command.Connection.State != ConnectionState.Open)
                Command.Connection.Open();
            return Command;
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The DBCommand object</returns>
        public static DbCommand Rollback(this DbCommand Command)
        {
            if (Command != null && Command.Transaction != null)
                Command.Transaction.Rollback();
            return Command;
        }
    }
}