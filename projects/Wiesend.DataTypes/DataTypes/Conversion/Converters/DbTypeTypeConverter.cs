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
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Wiesend.DataTypes.Conversion.Converters.BaseClasses;

namespace Wiesend.DataTypes.Conversion.Converters
{
    /// <summary>
    /// DbType converter
    /// </summary>
    public class DbTypeTypeConverter : TypeConverterBase<DbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DbTypeTypeConverter()
        {
            ConvertToTypes.Add(typeof(Type), DbTypeToType);
            ConvertToTypes.Add(typeof(SqlDbType), DbTypeToSqlDbType);
            ConvertFromTypes.Add(typeof(Type).GetType(), TypeToDbType);
            ConvertFromTypes.Add(typeof(SqlDbType), SqlDbTypeToDbType);
        }

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get { return new EnumConverter(typeof(DbType)); } }

        private static object DbTypeToSqlDbType(object value)
        {
            if (value is not DbType)
                return SqlDbType.Int;
            var TempValue = (DbType)value;
            var Parameter = new SqlParameter { DbType = TempValue };
            return Parameter.SqlDbType;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "<Pending>")]
        private static object DbTypeToType(object value)
        {
            if (value is not DbType)
                return typeof(int);
            var TempValue = (DbType)value;
            switch (TempValue)
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

        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (sqlDbType is not SqlDbType)
                return DbType.Int32;
            var Temp = (SqlDbType)sqlDbType;
            var Parameter = new SqlParameter { SqlDbType = Temp };
            return Parameter.DbType;
        }

        private static object TypeToDbType(object Object)
        {
            var TempValue = Object as Type;
            if (TempValue == null)
                return DbType.Int32;
            if (TempValue.IsEnum)
                TempValue = Enum.GetUnderlyingType(TempValue);
            if (TempValue == typeof(byte)) return DbType.Byte;
            else if (TempValue == typeof(sbyte)) return DbType.SByte;
            else if (TempValue == typeof(short)) return DbType.Int16;
            else if (TempValue == typeof(ushort)) return DbType.UInt16;
            else if (TempValue == typeof(int)) return DbType.Int32;
            else if (TempValue == typeof(uint)) return DbType.UInt32;
            else if (TempValue == typeof(long)) return DbType.Int64;
            else if (TempValue == typeof(ulong)) return DbType.UInt64;
            else if (TempValue == typeof(float)) return DbType.Single;
            else if (TempValue == typeof(double)) return DbType.Double;
            else if (TempValue == typeof(decimal)) return DbType.Decimal;
            else if (TempValue == typeof(bool)) return DbType.Boolean;
            else if (TempValue == typeof(string)) return DbType.String;
            else if (TempValue == typeof(char)) return DbType.StringFixedLength;
            else if (TempValue == typeof(Guid)) return DbType.Guid;
            else if (TempValue == typeof(DateTime)) return DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (TempValue == typeof(byte[])) return DbType.Binary;
            else if (TempValue == typeof(byte?)) return DbType.Byte;
            else if (TempValue == typeof(sbyte?)) return DbType.SByte;
            else if (TempValue == typeof(short?)) return DbType.Int16;
            else if (TempValue == typeof(ushort?)) return DbType.UInt16;
            else if (TempValue == typeof(int?)) return DbType.Int32;
            else if (TempValue == typeof(uint?)) return DbType.UInt32;
            else if (TempValue == typeof(long?)) return DbType.Int64;
            else if (TempValue == typeof(ulong?)) return DbType.UInt64;
            else if (TempValue == typeof(float?)) return DbType.Single;
            else if (TempValue == typeof(double?)) return DbType.Double;
            else if (TempValue == typeof(decimal?)) return DbType.Decimal;
            else if (TempValue == typeof(bool?)) return DbType.Boolean;
            else if (TempValue == typeof(char?)) return DbType.StringFixedLength;
            else if (TempValue == typeof(Guid?)) return DbType.Guid;
            else if (TempValue == typeof(DateTime?)) return DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }
    }
}