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

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wiesend.DataTypes;
using Wiesend.IO.Serializers.Interfaces;

namespace Wiesend.IO.Serializers
{
    /// <summary>
    /// Serialization manager class
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Serializers">The serializers.</param>
        public Manager([NotNull] IEnumerable<ISerializer> Serializers)
        {
            if (Serializers == null) throw new ArgumentNullException(nameof(Serializers));
            this.Serializers = Serializers.Where(x => !x.GetType().Namespace.StartsWith("WIESEND", StringComparison.OrdinalIgnoreCase))
                                          .ToDictionary(x => x.ContentType);
            Serializers.Where(x => x.GetType().Namespace.StartsWith("WIESEND", StringComparison.OrdinalIgnoreCase))
                       .ForEach(x =>
                       {
                           if (!this.Serializers.ContainsKey(x.ContentType))
                               this.Serializers.Add(x.ContentType, x);
                       });
        }

        /// <summary>
        /// Serializers
        /// </summary>
        protected IDictionary<string, ISerializer> Serializers { get; private set; }

        /// <summary>
        /// Determines if the system can serialize/deserialize the content type
        /// </summary>
        /// <param name="ContentType">Content type</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanSerialize([NotNull] string ContentType)
        {
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            return Serializers.ContainsKey(ContentType.Split(';')[0]);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="R">Return object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public R Deserialize<T, R>(T Data, [NotNull] string ContentType = "application/json")
        {
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            return (R)Deserialize<T>(Data, typeof(R), ContentType);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ObjectType">Object type requested</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize<T>(T Data, [NotNull] Type ObjectType, [NotNull] string ContentType = "application/json")
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            ContentType = ContentType.Split(';')[0];
            if (!Serializers.ContainsKey(ContentType) || Serializers[ContentType].ReturnType != typeof(T))
                return null;
            return ((ISerializer<T>)Serializers[ContentType]).Deserialize(ObjectType, Data);
        }

        /// <summary>
        /// File type to content type
        /// </summary>
        /// <param name="FileType">File type</param>
        /// <returns>Content type</returns>
        public string FileTypeToContentType(string FileType)
        {
            return Serializers.FirstOrDefault(x => string.Equals(x.Value.FileType, FileType, StringComparison.OrdinalIgnoreCase)).Chain(x => x.Value).Chain(x => x.ContentType, "");
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <typeparam name="R">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public R Serialize<T, R>(T Object, [NotNull] string ContentType = "application/json")
        {
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            return Serialize<R>(Object, typeof(T), ContentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <typeparam name="T">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public T Serialize<T>(object Object, [NotNull] Type ObjectType, [NotNull] string ContentType = "application/json")
        {
            if (ObjectType == null) throw new ArgumentNullException(nameof(ObjectType));
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            ContentType = ContentType.Split(';')[0];
            if (!Serializers.ContainsKey(ContentType) || Serializers[ContentType].ReturnType != typeof(T))
                return default;
            return ((ISerializer<T>)Serializers[ContentType]).Serialize(ObjectType, Object);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.Append("Serializers: ").AppendLine(Serializers.ToString(x => x.Value.Name));
            return Builder.ToString();
        }
    }
}