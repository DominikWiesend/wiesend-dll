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

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Wiesend.DataTypes;
using Wiesend.ORM.Manager.Mapper.Interfaces;
using Wiesend.ORM.Manager.QueryProvider.Interfaces;
using Wiesend.ORM.Manager.SourceProvider.Interfaces;

namespace Wiesend.ORM.Manager.QueryProvider.Default.LDAP
{
    /// <summary>
    /// SQL Server generator
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    public class LDAPGenerator<T> : IGenerator<T>
        where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source info</param>
        /// <param name="Mapping">Mapping info</param>
        public LDAPGenerator(LDAPQueryProvider QueryProvider, ISourceInfo Source, IMapping Mapping)
        {
            this.QueryProvider = QueryProvider;
            this.Source = Source;
            this.Mapping = Mapping;
        }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        private IMapping Mapping { get; set; }

        /// <summary>
        /// Gets or sets the query provider.
        /// </summary>
        /// <value>The query provider.</value>
        private LDAPQueryProvider QueryProvider { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        private ISourceInfo Source { get; set; }

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch All(params IParameter[] Parameters)
        {
            if (Mapping == null)
                return QueryProvider.Batch(Source);
#if NET45
            Parameters = Parameters.Check(new IParameter[] { });
#else
            Parameters = Parameters.Check(System.Array.Empty<IParameter>());
#endif
            string Command = "(*)";
            Parameters.ForEach(x =>
            {
                Command = string.Format(CultureInfo.InvariantCulture, "(&({0}={1})({2}))", x.ID, x.InternalValue.ToString(), Command);
            });
            return QueryProvider.Batch(Source)
                .AddCommand(null,
                    null,
                    Command,
                    CommandType.Text,
                    Parameters);
        }

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Limit">Max number of items to return</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch All(int Limit, params IParameter[] Parameters)
        {
            return All(Parameters);
        }

        /// <summary>
        /// Generates a batch that will get the first item that satisfies the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Any(params IParameter[] Parameters)
        {
            return All(Parameters);
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Object">Object to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(T Object)
        {
            return QueryProvider
                .Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(IEnumerable<T> Objects)
        {
            var TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Delete(Object));
            }
            return TempBatch;
        }

        /// <summary>
        /// Generates a batch that will insert the data from the object
        /// </summary>
        /// <param name="Object">Object to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Insert(T Object)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will insert the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Insert(IEnumerable<T> Objects)
        {
            var TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Insert(Object));
            }
            return TempBatch;
        }

        /// <summary>
        /// Deletes items from the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public IBatch JoinsDelete<P>(IProperty<T, P> Property, T Object)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Saves items to the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <typeparam name="ItemType">Item type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public IBatch JoinsSave<P, ItemType>(IProperty<T, P> Property, T Object)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will get the specific property for the object
        /// </summary>
        /// <typeparam name="P">Property type</typeparam>
        /// <param name="Object">Object to get the property for</param>
        /// <param name="Property">Property to get</param>
        /// <returns>Batch with the appropriate commands</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public IBatch LoadProperty<P>(T Object, IProperty Property)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will get the number of pages for a given page size given the
        /// parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="PageSize">Page size</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch PageCount(int PageSize, params IParameter[] Parameters)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will get a specific page of data that satisfies the parameters specified
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Paged(int PageSize, int CurrentPage, string OrderBy, params IParameter[] Parameters)
        {
            return All(Parameters);
        }

        /// <summary>
        /// Saves the object to the source
        /// </summary>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object to save</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public IBatch Save<PrimaryKeyType>(T Object)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Sets up the various default commands for the mapping
        /// </summary>
        /// <param name="Mapping"></param>
        public void SetupCommands(IMapping<T> Mapping)
        {
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.Map<T, D> Property) where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a IEnumerableManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">IEnumerableManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.IEnumerableManyToOne<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a ListManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ListManyToOne<T, D> Property)
                    where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a ListManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ListManyToMany<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a ManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToOne property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ManyToOne<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a ManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToMany property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ManyToMany<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.IListManyToMany<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.IListManyToOne<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ICollectionManyToMany<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Justification = "<Pending>")]
        public void SetupLoadCommands<D>(Mapper.Default.ICollectionManyToOne<T, D> Property)
            where D : class
        {
        }

        /// <summary>
        /// Generates a batch that will update the data from the object
        /// </summary>
        /// <param name="Object">Object to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(T Object)
        {
            return QueryProvider.Batch(Source);
        }

        /// <summary>
        /// Generates a batch that will update the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(IEnumerable<T> Objects)
        {
            var TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Update(Object));
            }
            return TempBatch;
        }
    }
}