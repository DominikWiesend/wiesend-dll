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
using System.Diagnostics.Contracts;
using System.Globalization;
using Wiesend.DataTypes.Conversion.Converters.Interfaces;

namespace Wiesend.DataTypes.Conversion
{
    /// <summary>
    /// Conversion manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Converters">The converters.</param>
        public Manager(IEnumerable<IConverter> Converters)
        {
            Contract.Requires<ArgumentNullException>(Converters != null, "Converters");
            foreach (IConverter TypeConverter in Converters)
            {
                TypeDescriptor.AddAttributes(TypeConverter.AssociatedType, new TypeConverterAttribute(TypeConverter.GetType()));
            }
        }

        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <typeparam name="R">Resulting type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="DefaultValue">
        /// Default return value if the item is null or can not be converted
        /// </param>
        /// <returns>The value converted to the specified type</returns>
        public static R To<T, R>(T Item, R DefaultValue = default(R))
        {
            return (R)To(Item, typeof(R), DefaultValue);
        }

        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="ResultType">Result type</param>
        /// <param name="DefaultValue">
        /// Default return value if the item is null or can not be converted
        /// </param>
        /// <returns>The value converted to the specified type</returns>
        public static object To<T>(T Item, Type ResultType, object DefaultValue = null)
        {
            try
            {
                if (Item == null)
                {
                    return (DefaultValue == null && ResultType.IsValueType) ?
                        Activator.CreateInstance(ResultType) :
                        DefaultValue;
                }
                var ObjectType = Item.GetType();
                if (ObjectType == typeof(DBNull))
                {
                    return (DefaultValue == null && ResultType.IsValueType) ?
                        Activator.CreateInstance(ResultType) :
                        DefaultValue;
                }
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Item;
                if (Item as IConvertible != null && !ObjectType.IsEnum && !ResultType.IsEnum)
                    return Convert.ChangeType(Item, ResultType, CultureInfo.InvariantCulture);
                var Converter = TypeDescriptor.GetConverter(Item);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Item, ResultType);
                Converter = TypeDescriptor.GetConverter(ResultType);
                if (Converter.CanConvertFrom(ObjectType))
                    return Converter.ConvertFrom(Item);
                if (ResultType.IsEnum)
                {
                    if (ObjectType == ResultType.GetEnumUnderlyingType())
                        return System.Enum.ToObject(ResultType, Item);
                    if (ObjectType == typeof(string))
                        return System.Enum.Parse(ResultType, Item as string, true);
                }
                if (ResultType.IsClass)
                {
                    var ReturnValue = Activator.CreateInstance(ResultType);
                    var TempMapping = ObjectType.MapTo(ResultType);
                    if (TempMapping == null)
                        return ReturnValue;
                    TempMapping
                        .AutoMap()
                        .Copy(Item, ReturnValue);
                    return ReturnValue;
                }
            }
            catch
            {
            }
            return (DefaultValue == null && ResultType.IsValueType) ?
                Activator.CreateInstance(ResultType) :
                DefaultValue;
        }

        /// <summary>
        /// Outputs information about the manager as a string
        /// </summary>
        /// <returns>The string version of the manager</returns>
        public override string ToString()
        {
            return "Conversion Manager: Default\r\n";
        }
    }
}