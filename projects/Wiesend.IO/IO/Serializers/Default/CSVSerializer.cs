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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.IO.FileFormats.Delimited;
using Wiesend.IO.Serializers.BaseClasses;

namespace Wiesend.IO.Serializers.Default
{
    /// <summary>
    /// CSV serializer
    /// </summary>
    public class CSVSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType { get { return "text/csv"; } }

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType { get { return ".csv"; } }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "CSV"; } }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public override object Deserialize(Type ObjectType, string Data)
        {
            if (string.IsNullOrEmpty(Data) || ObjectType == null)
                return null;
            var Method = typeof(TypeConversionExtensions).GetMethods().FirstOrDefault(x => x.Name == "To" && x.GetParameters().Any(y => y.ParameterType == typeof(DataTable)));
            Method = Method.MakeGenericMethod(ObjectType.Is<IEnumerable>() ? ObjectType.GetGenericArguments()[0] : ObjectType);
            var Table = new Delimited(Data).ToDataTable();
            var ReturnValue = Method.Invoke(null, new object[] { Table, null });
            return ObjectType.Is<IEnumerable>() ? ReturnValue : ((IEnumerable)ReturnValue).GetEnumerator().Chain(x =>
            {
                x.MoveNext();
                return x.Current;
            });
        }

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public override string Serialize(Type ObjectType, object Data)
        {
            if (Data == null || ObjectType == null)
                return null;
            if (Data.Is<IEnumerable>())
                return ((IEnumerable)Data).ToDelimitedFile();
            else
            {
                var Temp = new List<object> { Data };
                return ((IEnumerable)Temp).ToDelimitedFile();
            }
        }
    }
}