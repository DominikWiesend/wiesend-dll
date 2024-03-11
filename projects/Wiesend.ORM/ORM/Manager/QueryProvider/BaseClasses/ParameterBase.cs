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

using System.Data;
using System.Data.Common;
using Wiesend.DataTypes;
using Wiesend.DataTypes.Comparison;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;

namespace Wiesend.ORM.Manager.QueryProvider.BaseClasses
{
    /// <summary>
    /// Parameter base class
    /// </summary>
    /// <typeparam name="DataType">Data type of the parameter</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
    public abstract class ParameterBase<DataType> : IParameter<DataType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">
        /// What the database expects as the parameter starting string ("@" for SQL Server, ":" for
        /// Oracle, etc.)
        /// </param>
        protected ParameterBase(string ID, DataType Value, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
            : this(ID, Value == null ? typeof(DataType).To(DbType.Int32) : Value.GetType().To(DbType.Int32), Value, Direction, ParameterStarter)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Type">Database type</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">
        /// What the database expects as the parameter starting string ("@" for SQL Server, ":" for
        /// Oracle, etc.)
        /// </param>
        protected ParameterBase(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
            : this(ID, Type.To(DbType.Int32), Value, Direction, ParameterStarter)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Type">Database type</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">
        /// What the database expects as the parameter starting string ("@" for SQL Server, ":" for
        /// Oracle, etc.)
        /// </param>
        protected ParameterBase(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            this.ID = ID;
            this.Value = (DataType)Value;
            DatabaseType = Type;
            this.Direction = Direction;
            BatchID = ID;
            this.ParameterStarter = ParameterStarter;
        }

        /// <summary>
        /// Database type
        /// </summary>
        public virtual DbType DatabaseType { get; set; }

        /// <summary>
        /// Direction of the parameter
        /// </summary>
        public virtual ParameterDirection Direction { get; set; }

        /// <summary>
        /// The Name that the parameter goes by
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// Gets the internal value.
        /// </summary>
        /// <value>The internal value.</value>
        public object InternalValue { get { return Value; } }

        /// <summary>
        /// Starting string of the parameter
        /// </summary>
        public string ParameterStarter { get; set; }

        /// <summary>
        /// Parameter value
        /// </summary>
        public virtual DataType Value { get; set; }

        /// <summary>
        /// Batch ID
        /// </summary>
        protected virtual string BatchID { get; set; }

        /// <summary>
        /// != operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>returns true if they are not equal, false otherwise</returns>
        public static bool operator !=(ParameterBase<DataType> first, ParameterBase<DataType> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>true if the first and second item are the same, false otherwise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "<Pending>")]
        public static bool operator ==(ParameterBase<DataType> first, ParameterBase<DataType> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if ((object)first == null || (object)second == null)
                return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// Adds this parameter to the SQLHelper
        /// </summary>
        /// <param name="Helper">SQLHelper</param>
        public abstract void AddParameter(DbCommand Helper);

        /// <summary>
        /// Finds itself in the string command and adds the value
        /// </summary>
        /// <param name="Command">Command to add to</param>
        /// <returns>The resulting string</returns>
        public string AddParameter(string Command)
        {
            if (string.IsNullOrEmpty(Command))
                return "";
            string StringValue = Value == null ? "NULL" : Value.ToString();
            return Command.Replace(ParameterStarter + ID, typeof(DataType) == typeof(string) ? "'" + StringValue + "'" : StringValue);
        }

        /// <summary>
        /// Creates a copy of the parameter
        /// </summary>
        /// <param name="Suffix">Suffix to add to the parameter (for batching purposes)</param>
        /// <returns>A copy of the parameter</returns>
        public abstract IParameter CreateCopy(string Suffix);

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var OtherParameter = obj as ParameterBase<DataType>;
            return (OtherParameter != null
                && OtherParameter.DatabaseType == DatabaseType
                && OtherParameter.Direction == Direction
                && OtherParameter.ID == ID
                && new GenericEqualityComparer<DataType>().Equals(OtherParameter.Value, Value));
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (ID.GetHashCode() * 23 + (new GenericEqualityComparer<DataType>().Equals(Value, default) ? 0 : Value.GetHashCode())) * 23 + DatabaseType.GetHashCode();
            }
        }

        /// <summary>
        /// Returns the string version of the parameter
        /// </summary>
        /// <returns>The string representation of the parameter</returns>
        public override string ToString()
        {
            return ParameterStarter + ID;
        }
    }
}