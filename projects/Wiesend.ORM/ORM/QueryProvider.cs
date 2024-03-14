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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;

namespace Wiesend.ORM
{
    /// <summary>
    /// Query provider
    /// </summary>
    public static class QueryProvider
    {
        /// <summary>
        /// Query manager
        /// </summary>
        private static Manager.QueryProvider.Manager QueryManager { get { return IoC.Manager.Bootstrapper.Resolve<Manager.QueryProvider.Manager>(); } }

        /// <summary>
        /// Source manager
        /// </summary>
        private static Manager.SourceProvider.Manager SourceManager { get { return IoC.Manager.Bootstrapper.Resolve<Manager.SourceProvider.Manager>(); } }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The list of objects requested</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> All<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().All<ObjectType>(Parameters);
        }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Command">Command to run</param>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Type">Command type</param>
        /// <returns>The list of objects requested</returns>
        public static IEnumerable<dynamic> All(string Command, CommandType Type, string ConnectionString, params object[] Parameters)
        {
            var TempBatch = Batch(ConnectionString);
            if (Parameters is not IParameter[] TempParameters)
                TempBatch.AddCommand(null, null, Command, Type, Parameters);
            else
                TempBatch.AddCommand(null, null, Command, Type, TempParameters);
            return TempBatch.Execute()[0];
        }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Command">Command to run</param>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Type">Command type</param>
        /// <returns>The list of objects requested</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> All<ObjectType>(string Command, CommandType Type, string ConnectionString, params object[] Parameters)
            where ObjectType : class,new()
        {
            return All(Command, Type, ConnectionString, Parameters)
                .ForEach(x => (ObjectType)x);
        }

        /// <summary>
        /// Returns an object based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <returns>An object requested</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static ObjectType Any<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().Any<ObjectType>(Parameters);
        }

        /// <summary>
        /// Returns an object based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="IDType">ID type</typeparam>
        /// <param name="ID">ID of the object to load</param>
        /// <returns>An object requested</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static ObjectType Any<ObjectType, IDType>(IDType ID)
            where ObjectType : class,new()
            where IDType : IComparable
        {
            return new Session().Any<ObjectType, IDType>(ID);
        }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Command">Command to run</param>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Type">Command type</param>
        /// <returns>The list of objects requested</returns>
        public static dynamic Any(string Command, CommandType Type, string ConnectionString, params object[] Parameters)
        {
            return All(Command, Type, ConnectionString, Parameters).FirstOrDefault();
        }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Command">Command to run</param>
        /// <param name="ConnectionString">Connection string</param>
        /// <param name="Type">Command type</param>
        /// <returns>The list of objects requested</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static ObjectType Any<ObjectType>(string Command, CommandType Type, string ConnectionString, params object[] Parameters)
            where ObjectType : class,new()
        {
            return (ObjectType)Any(Command, Type, ConnectionString, Parameters);
        }

        /// <summary>
        /// Creates a batch object that can be used to run ad hoc queries
        /// </summary>
        /// <param name="ConnectionString">
        /// Connection string (can be the name of connection string in config file or the actual
        /// connection string)
        /// </param>
        /// <returns>An appropriate batch object</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
        public static IBatch Batch(string ConnectionString)
        {
            var Source = SourceManager.GetSource(ConnectionString) ?? throw new NullReferenceException("Source not found");
            var Batch = QueryManager.Batch(Source) ?? throw new NullReferenceException("Batch could not be created");
            return Batch;
        }

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to delete</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static void Delete<ObjectType>(ObjectType Object)
            where ObjectType : class,new()
        {
            new Session().Delete<ObjectType>(Object);
        }

        /// <summary>
        /// Gets the page count based on the page size specified
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The number of pages</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static int PageCount<ObjectType>(int PageSize = 25, params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().PageCount<ObjectType>(PageSize, Parameters);
        }

        /// <summary>
        /// Gets a specific page of objects
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The objects specified</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public static IEnumerable<ObjectType> Paged<ObjectType>(int PageSize = 25, int CurrentPage = 0, string OrderBy = "", params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().Paged<ObjectType>(PageSize, CurrentPage, OrderBy, Parameters);
        }

        /// <summary>
        /// Saves an object
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
        public static void Save<ObjectType, PrimaryKeyType>(ObjectType Object)
            where ObjectType : class,new()
        {
            new Session().Save<ObjectType, PrimaryKeyType>(Object);
        }
    }
}