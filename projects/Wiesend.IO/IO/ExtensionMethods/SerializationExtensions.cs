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
using System.ComponentModel;
using Wiesend.IO.Serializers;

namespace Wiesend.IO
{
    /// <summary>
    /// Extension methods dealing with serialization
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SerializationExtensions
    {
        /// <summary>
        /// Deserializes the data based on the MIME content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static R Deserialize<R, T>(this T Data, [NotNull] string ContentType = "application/json")
        {
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return default;
            return (R)TempManager.Deserialize<T>(Data, typeof(R), ContentType);
        }

        /// <summary>
        /// Deserializes the data based on the content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Data">Data to deserialize</param>
        /// <param name="ContentType">Content type</param>
        /// <returns>The deserialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        public static R Deserialize<R, T>(this T Data, SerializationType ContentType)
        {
            if (ContentType == null)
                ContentType = SerializationType.JSON;
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return default;
            return (R)TempManager.Deserialize<T>(Data, typeof(R), ContentType);
        }

        /// <summary>
        /// Serializes the data based on the MIME content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type (MIME type)</param>
        /// <returns>The serialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static R Serialize<R, T>(this T Object, [NotNull] string ContentType = "application/json")
        {
            if (string.IsNullOrEmpty(ContentType)) throw new ArgumentNullException(nameof(ContentType));
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return default;
            return TempManager.Serialize<T, R>(Object, ContentType);
        }

        /// <summary>
        /// Serializes the data based on the type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type</param>
        /// <returns>The serialized object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0074:Use compound assignment", Justification = "<Pending>")]
        public static R Serialize<R, T>(this T Object, SerializationType ContentType)
        {
            if (ContentType == null)
                ContentType = SerializationType.JSON;
            var TempManager = IoC.Manager.Bootstrapper.Resolve<Manager>();
            if (TempManager == null)
                return default;
            return TempManager.Serialize<T, R>(Object, ContentType);
        }
    }

    /// <summary>
    /// Serialization enum like class
    /// </summary>
    public class SerializationType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        protected SerializationType(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Binary
        /// </summary>
        public static SerializationType Binary { get { return new SerializationType("application/octet-stream"); } }

        /// <summary>
        /// JSON
        /// </summary>
        public static SerializationType JSON { get { return new SerializationType("application/json"); } }

        /// <summary>
        /// SOAP
        /// </summary>
        public static SerializationType SOAP { get { return new SerializationType("application/soap+xml"); } }

        /// <summary>
        /// XML
        /// </summary>
        public static SerializationType XML { get { return new SerializationType("text/xml"); } }

        private string Name { get; set; }

        /// <summary>
        /// Converts the object to a string implicitly
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <returns>The string version of the serialization type</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public static implicit operator string(SerializationType Object)
        {
            if (Object == null) throw new ArgumentNullException(nameof(Object));
            return Object.ToString();
        }

        /// <summary>
        /// Returns the name of the serialization type
        /// </summary>
        /// <returns>Name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}