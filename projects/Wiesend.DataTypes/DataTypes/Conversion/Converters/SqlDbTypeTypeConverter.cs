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
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Wiesend.DataTypes.Conversion.Converters.BaseClasses;

namespace Wiesend.DataTypes.Conversion.Converters
{
    /// <summary>
    /// SqlDbType converter
    /// </summary>
    public class SqlDbTypeTypeConverter : TypeConverterBase<SqlDbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlDbTypeTypeConverter()
        {
            ConvertToTypes.Add(typeof(Type), SqlDbTypeToType);
            ConvertToTypes.Add(typeof(DbType), SqlDbTypeToDbType);
            ConvertFromTypes.Add(typeof(Type).GetType(), TypeToSqlDbType);
            ConvertFromTypes.Add(typeof(DbType), DbTypeToSqlDbType);
            Conversions = new ConcurrentDictionary<Type, DbType>();
            Conversions.AddOrUpdate(typeof(byte), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte?), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte), DbType.SByte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte?), DbType.SByte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short?), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort), DbType.UInt16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort?), DbType.UInt16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int?), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint), DbType.UInt32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint?), DbType.UInt32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long?), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong), DbType.UInt64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong?), DbType.UInt64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float?), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double?), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal?), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool?), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(string), DbType.String, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char?), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid?), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime?), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset?), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte[]), DbType.Binary, (x, y) => y);
        }

        /// <summary>
        /// Conversions
        /// </summary>
        protected static ConcurrentDictionary<Type, DbType> Conversions { get; private set; }

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get { return new EnumConverter(typeof(SqlDbType)); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0083:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        private static object DbTypeToSqlDbType(object value)
        {
            if (!(value is DbType))
                return SqlDbType.Int;
            var TempValue = (DbType)value;
            var Parameter = new SqlParameter();
            Parameter.DbType = TempValue;
            return Parameter.SqlDbType;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0083:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (!(sqlDbType is SqlDbType))
                return DbType.Int32;
            var Temp = (SqlDbType)sqlDbType;
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Temp;
            return Parameter.DbType;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0083:Use pattern matching", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        private static object SqlDbTypeToType(object arg)
        {
            if (!(arg is SqlDbType))
                return typeof(int);
            var Item = (SqlDbType)arg;
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Item;
            switch (Parameter.DbType)
            {
                case DbType.Byte:
                    return typeof(byte);
                case DbType.SByte:
                    return typeof(sbyte);
                case DbType.Int16:
                    return typeof(short);
                case DbType.UInt16:
                    return typeof(ushort);
                case DbType.Int32:
                    return typeof(int);
                case DbType.UInt32:
                    return typeof(uint);
                case DbType.Int64:
                    return typeof(long);
                case DbType.UInt64:
                    return typeof(ulong);
                case DbType.Single:
                    return typeof(float);
                case DbType.Double:
                    return typeof(double);
                case DbType.Decimal:
                    return typeof(decimal);
                case DbType.Boolean:
                    return typeof(bool);
                case DbType.String:
                    return typeof(string);
                case DbType.StringFixedLength:
                    return typeof(char);
                case DbType.Guid:
                    return typeof(Guid);
                case DbType.DateTime2:
                    return typeof(DateTime);
                case DbType.DateTime:
                    return typeof(DateTime);
                case DbType.DateTimeOffset:
                    return typeof(DateTimeOffset);
                case DbType.Binary:
                    return typeof(byte[]);
            }

            return typeof(int);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:Simplify object initialization", Justification = "<Pending>")]
        private static object TypeToSqlDbType(object arg)
        {
            var TempValue = arg as Type;
            if (TempValue == null)
                return SqlDbType.Int;
            DbType Item = DbType.Int32;
            if (TempValue.IsEnum)
                TempValue = Enum.GetUnderlyingType(TempValue);
            Item = Conversions.GetValue(TempValue, DbType.Int32);
            var Parameter = new SqlParameter();
            Parameter.DbType = Item;
            return Parameter.SqlDbType;
        }
    }
}